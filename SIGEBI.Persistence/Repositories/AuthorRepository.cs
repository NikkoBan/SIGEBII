using Microsoft.EntityFrameworkCore;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities;
using SIGEBI.Persistence.Base;
using SIGEBI.Persistence.Context;
using SIGEBI.Persistence.Interfaces;
using System.Linq.Expressions;

namespace SIGEBI.Persistence.Repositories
{
    public class AuthorRepository : BaseRepository<Author, int>, IAuthorRepository
    {
        public readonly SIGEBIDbContext _context;

        public AuthorRepository(SIGEBIDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<Author?> GetEntityByIdAsync(int id)
        {
            return await _context.Authors.FindAsync(id);
        }

        public override async Task<bool> Exists(Expression<Func<Author, bool>> filter)
        {
            return await _context.Authors.AnyAsync(filter);
        }

        public override async Task<List<Author>> GetAllAsync()
        {
            return await _context.Authors.ToListAsync();
        }

        public override async Task<OperationResult<Author>> SaveEntityAsync(Author entity)
        {
            try
            {
                _context.Authors.Add(entity);
                await _context.SaveChangesAsync();
                return OperationResult<Author>.SuccessResult(entity, "Autor guardado correctamente.");
            }
            catch (Exception ex)
            {
                return OperationResult<Author>.FailResult($"Error guardando autor: {ex.Message}");
            }
        }

        public override async Task<OperationResult<Author>> UpdateEntityAsync(Author entity)
        {
            try
            {
                _context.Authors.Update(entity);
                await _context.SaveChangesAsync();
                return OperationResult<Author>.SuccessResult(entity, "Autor actualizado correctamente.");
            }
            catch (Exception ex)
            {
                return OperationResult<Author>.FailResult($"Error actualizando autor: {ex.Message}");
            }
        }

        public override async Task<OperationResult<Author>> RemoveEntityAsync(Author entity)
        {
            try
            {
                _context.Authors.Remove(entity);
                await _context.SaveChangesAsync();
                return OperationResult<Author>.SuccessResult(entity, "Autor eliminado correctamente.");
            }
            catch (Exception ex)
            {
                return OperationResult<Author>.FailResult($"Error eliminando autor: {ex.Message}");
            }
        }

        public async Task<List<Author>> GetAuthorsByNationality(string nationality)
        {
            return await _context.Authors
                .Where(a => a.Nationality != null && a.Nationality.ToLower() == nationality.ToLower())
                .ToListAsync();
        }

        public async Task<Author?> GetAuthorByFullName(string firstName, string lastName)
        {
            return await _context.Authors
                .FirstOrDefaultAsync(a =>
                    a.FirstName.ToLower() == firstName.ToLower() &&
                    a.LastName.ToLower() == lastName.ToLower());
        }

        public async Task<OperationResult<Author>> UpdateNationality(Author author, string newNationality)
        {
            try
            {
                author.Nationality = newNationality;
                _context.Authors.Update(author);
                await _context.SaveChangesAsync();
                return OperationResult<Author>.SuccessResult(author, "Nacionalidad actualizada correctamente.");
            }
            catch (Exception ex)
            {
                return OperationResult<Author>.FailResult($"Error actualizando nacionalidad: {ex.Message}");
            }
        }
    }
}
