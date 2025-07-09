using Microsoft.EntityFrameworkCore;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities;
using SIGEBI.Persistence.Base;
using SIGEBI.Persistence.Context;
using SIGEBI.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SIGEBI.Persistence.Repositories
{
        public class PublishersRepository : BaseRepository<Publishers, int>, IPublishersRepository
        {
            private readonly SIGEBIDbContext _dbContext;

            public PublishersRepository(SIGEBIDbContext context) : base(context)
            {
                _dbContext = context;
            }

            public override async Task<OperationResult> SaveEntityAsync(Publishers entity)
            {
                var validation = ValidatePublisher(entity);
                if (!validation.Success)
                    return validation;

                await _dbContext.Publishers.AddAsync(entity);
                await _dbContext.SaveChangesAsync();
                return OperationResult.Ok("Editorial guardada correctamente.", entity);
            }

            public override async Task<OperationResult> UpdateEntityAsync(Publishers entity)
            {
                var validation = ValidatePublisher(entity);
                if (!validation.Success)
                    return validation;

                var existing = await _dbContext.Publishers.FindAsync(entity.ID);
                if (existing == null)
                    return OperationResult.Fail("Editorial no encontrada.");

                MapPublisher(existing, entity);
                _dbContext.Publishers.Update(existing);
                await _dbContext.SaveChangesAsync();
                return OperationResult.Ok("Editorial actualizada correctamente.", existing);
            }

            public override async Task<Publishers?> GetEntityByIdAsync(int id)
            {
                return await _dbContext.Publishers
                    .Include(p => p.Books)
                    .FirstOrDefaultAsync(p => p.ID == id);
            }

            public override async Task<List<Publishers>> GetAllAsync()
            {
                return await _dbContext.Publishers
                    .Include(p => p.Books)
                    .ToListAsync();
            }

            public override async Task<bool> Exists(System.Linq.Expressions.Expression<Func<Publishers, bool>> filter)
            {
                return await _dbContext.Publishers.AnyAsync(filter);
            }

            public override async Task<OperationResult> RemoveEntityAsync(Publishers entity)
            {
                var existing = await _dbContext.Publishers.FindAsync(entity.ID);
                if (existing == null)
                    return OperationResult.Fail("Editorial no encontrada.");

                _dbContext.Publishers.Remove(existing);
                await _dbContext.SaveChangesAsync();
                return OperationResult.Ok("Editorial eliminada correctamente.");
            }

            public async Task<List<Publishers>> SearchByNameAsync(string name)
            {
                return await _dbContext.Publishers
                    .Where(p => p.PublisherName.Contains(name))
                    .ToListAsync();
            }

            public async Task<Publishers?> GetByEmailAsync(string email)
            {
                return await _dbContext.Publishers
                    .FirstOrDefaultAsync(p => p.Email == email);
            }

            private OperationResult ValidatePublisher(Publishers entity)
            {
                if (string.IsNullOrWhiteSpace(entity.PublisherName))
                    return OperationResult.Fail("El nombre de la editorial es obligatorio.");

                if (!string.IsNullOrWhiteSpace(entity.Email) &&
                    !Regex.IsMatch(entity.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                    return OperationResult.Fail("El correo electrónico no es válido.");

                if (!string.IsNullOrWhiteSpace(entity.PhoneNumber) &&
                    !Regex.IsMatch(entity.PhoneNumber, @"^\+?\d{7,15}$"))
                    return OperationResult.Fail("El número de teléfono no es válido.");

                return OperationResult.Ok();
            }

            private void MapPublisher(Publishers target, Publishers source)
            {
                target.PublisherName = source.PublisherName;
                target.Address = source.Address;
                target.PhoneNumber = source.PhoneNumber;
                target.Email = source.Email;
                target.Website = source.Website;
                
                /* */
               
            }
        public async Task<bool> ExistsByNameOrEmailAsync(string name, string email)
        {
            return await _dbContext.Publishers
                .AnyAsync(p => p.PublisherName == name || p.Email == email);
        }

        public async Task<bool> ExistsByNameOrEmailExceptIdAsync(string name, string email, int excludeId)
        {
            return await _dbContext.Publishers
                .AnyAsync(p => (p.PublisherName == name || p.Email == email) && p.ID != excludeId);
        }
    }
    }
