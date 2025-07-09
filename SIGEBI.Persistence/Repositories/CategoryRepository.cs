using Microsoft.EntityFrameworkCore;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities;
using SIGEBI.Persistence.Base;
using SIGEBI.Persistence.Context;
using SIGEBI.Persistence.Interfaces;
using System.Linq.Expressions;

namespace SIGEBI.Persistence.Repositories
{
    public class CategoryRepository : BaseRepository<Category, int>, ICategoryRepository
    {
        private readonly SIGEBIDbContext _context;

        public CategoryRepository(SIGEBIDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Category?> GetCategoryByName(string name)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(c => c.CategoryName.ToLower() == name.ToLower());
        }

        public async Task<List<Category>> GetAllWithBooks()
        {
            return await _context.Categories
                .Include(c => c.Books)
                .AsNoTracking()
                .ToListAsync();
        }

        public override async Task<bool> Exists(Expression<Func<Category, bool>> filter)
        {
            return await _context.Categories.AnyAsync(filter);
        }

        public override async Task<List<Category>> GetAllAsync()
        {
            return await _context.Categories
                .AsNoTracking()
                .ToListAsync();
        }

        public override async Task<Category?> GetEntityByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public override async Task<OperationResult<Category>> SaveEntityAsync(Category entity)
        {
            try
            {
                _context.Categories.Add(entity);
                await _context.SaveChangesAsync();
                return OperationResult<Category>.SuccessResult(entity, "Categoría guardada correctamente.");
            }
            catch (Exception ex)
            {
                return OperationResult<Category>.FailResult($"Error guardando categoría: {ex.Message}");
            }
        }

        public override async Task<OperationResult<Category>> UpdateEntityAsync(Category entity)
        {
            try
            {
                _context.Categories.Update(entity);
                await _context.SaveChangesAsync();
                return OperationResult<Category>.SuccessResult(entity, "Categoría actualizada correctamente.");
            }
            catch (Exception ex)
            {
                return OperationResult<Category>.FailResult($"Error actualizando categoría: {ex.Message}");
            }
        }

        public override async Task<OperationResult<Category>> RemoveEntityAsync(Category entity)
        {
            try
            {
                _context.Categories.Remove(entity);
                await _context.SaveChangesAsync();
                return OperationResult<Category>.SuccessResult(entity, "Categoría eliminada correctamente.");
            }
            catch (Exception ex)
            {
                return OperationResult<Category>.FailResult($"Error eliminando categoría: {ex.Message}");
            }
        }
    }
}
