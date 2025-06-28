using Microsoft.EntityFrameworkCore;
using SIGEBI.Domain.Entities;
using SIGEBI.Persistence.Context;
using SIGEBI.Persistence.Interfaces;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SIGEBI.Persistence.Base
{
    public abstract class BaseRepository<TEntity, TType> : IBaseRepository<TEntity, TType> where TEntity : class
    {
        private readonly SIGEBIDbContext _context;
        protected DbSet<TEntity> Entity { get; }

        protected BaseRepository(SIGEBIDbContext context)
        {
            _context = context;
            Entity = _context.Set<TEntity>();
        }

        public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await Entity.AnyAsync(filter);
        }

        public virtual async Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? filter = null)
        {
            return filter != null ? await Entity.Where(filter).ToListAsync() : await Entity.ToListAsync();
        }

        public virtual async Task<TEntity?> GetByIdAsync(TType id)
        {
            return await Entity.FindAsync(id);
        }

        public virtual async Task<OperationResult> AddAsync(TEntity entity)
        {
            var result = new OperationResult();
            try
            {
                await Entity.AddAsync(entity);
                await _context.SaveChangesAsync();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Ocurrió un error guardando los datos: {ex.Message}";
            }
            return result;
        }

        public virtual async Task<OperationResult> UpdateAsync(TEntity entity)
        {
            var result = new OperationResult();
            try
            {
                Entity.Update(entity);
                await _context.SaveChangesAsync();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Ocurrió un error actualizando los datos: {ex.Message}";
            }
            return result;
        }

        public virtual async Task<OperationResult> RemoveAsync(TType id)
        {
            var result = new OperationResult();
            try
            {
                var entity = await Entity.FindAsync(id);
                if (entity == null)
                {
                    result.Success = false;
                    result.Message = "Entidad no encontrada para eliminar.";
                    return result;
                }
                Entity.Remove(entity);
                await _context.SaveChangesAsync();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Ocurrió un error eliminando los datos: {ex.Message}";
            }
            return result;
        }
    }
}