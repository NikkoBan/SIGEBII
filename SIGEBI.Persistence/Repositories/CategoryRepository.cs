using Castle.Core.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Repository;
using SIGEBI.Persistence.Base;
using SIGEBI.Persistence.Context;
using SIGEBI.Persistence.Interfaces;
using System.Linq.Expressions;

namespace SIGEBI.Persistence.Repositories
{
    public class CategoryRepository : BaseRepository<Category, int>, ICategoryRepository
    {
        private readonly SIGEBIDbContext _context;
        private readonly ILogger<CategoryRepository> _logger;
        private readonly IConfiguration _configuration;

        public CategoryRepository(SIGEBIDbContext context, ILogger<CategoryRepository> logger, IConfiguration configuration)
            : base(context)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
        }

        public override async Task<bool> Exists(Expression<Func<Category, bool>> filter)
        {
            return await _context.Category.AnyAsync(filter);
        }

        public override async Task<List<Category>> GetAllAsync()
        {
            return await _context.Category
                                 .Where(c => !c.Borrado)
                                 .AsNoTracking()
                                 .ToListAsync();
        }

        public override async Task<OperationResult> GetAllAsync(Expression<Func<Category, bool>> filter)
        {
            var data = await _context.Category
                                     .Where(filter)
                                     .Where(c => !c.Borrado)
                                     .AsNoTracking()
                                     .ToListAsync();

            return new OperationResult
            {
                Success = true,
                Data = data
            };
        }

        public override async Task<Category?> GetEntityByIdAsync(int id)
        {
            if (!RepoValidation.ValidarID(id))
                return null;

            return await _context.Category.FindAsync(id);
        }

        public async Task<Category?> GetCategoryByNameAsync(string name)
        {
            return await _context.Category
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync(c => c.CategoryName == name && !c.Borrado);
        }

        public async Task<List<Category>> GetAllActiveCategoriesAsync()
        {
            return await _context.Category
                                 .Where(c => !c.Borrado)
                                 .AsNoTracking()
                                 .ToListAsync();
        }

        public override async Task<OperationResult> SaveEntityAsync(Category entity)
        {
            var result = new OperationResult();

            if (string.IsNullOrWhiteSpace(entity.CategoryName))
            {
                result.Success = false;
                result.Message = _configuration["ErrorCategoryRepository:InvalidData"]!;
                return result;
            }

            try
            {
                _context.Category.Add(entity);
                await _context.SaveChangesAsync();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = _configuration["ErrorCategoryRepository:SaveEntityAsync"]!;
                _logger.LogError(result.Message, ex.ToString());
            }

            return result;
        }

        public override async Task<OperationResult> UpdateEntityAsync(Category entity)
        {
            var result = new OperationResult();

            if (!RepoValidation.ValidarID(entity.Id) || string.IsNullOrWhiteSpace(entity.CategoryName))
            {
                result.Success = false;
                result.Message = _configuration["ErrorCategoryRepository:InvalidData"]!;
                return result;
            }

            try
            {
                _context.Category.Update(entity);
                await _context.SaveChangesAsync();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = _configuration["ErrorCategoryRepository:UpdateEntityAsync"]!;
                _logger.LogError(result.Message, ex.ToString());
            }

            return result;
        }

        public override async Task<OperationResult> RemoveEntityAsync(Category entity)
        {
            var result = new OperationResult();

            if (!RepoValidation.ValidarID(entity.Id) ||
                !RepoValidation.ValidarID(entity.UsuarioMod) ||
                !RepoValidation.ValidarEntidad(entity.FechaModificacion!) ||
                !RepoValidation.ValidarID(entity.BorradoPorU) ||
                !RepoValidation.ValidarEntidad(entity.Borrado!))
            {
                result.Success = false;
                result.Message = _configuration["ErrorCategoryRepository:InvalidData"]!;
                return result;
            }

            try
            {
                _context.Category.Update(entity);
                await _context.SaveChangesAsync();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = _configuration["ErrorCategoryRepository:RemoveEntity"]!;
                _logger.LogError(result.Message, ex.ToString());
            }

            return result;
        }

        Task<List<Category>> ICategoryRepository.GetAllWithBooks()
        {
            throw new NotImplementedException();
        }
    }
}
