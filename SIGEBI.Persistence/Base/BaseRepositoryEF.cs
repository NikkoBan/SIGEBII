using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.IRepository;
using SIGEBI.Persistence.Context;



using System.Linq.Expressions;

namespace SIGEBI.Persistence.Base
{
    public abstract class BaseRepositoryEf<TEntity> : IBaseRepository<TEntity>
     where TEntity : AuditableEntity
    {
        protected readonly SIGEBIContext _context;
        protected readonly DbSet<TEntity> _dbSet;
        protected readonly ILogger<BaseRepositoryEf<TEntity>> _logger;

        protected BaseRepositoryEf(SIGEBIContext context, ILogger<BaseRepositoryEf<TEntity>> logger)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
            _logger = logger;
        }

        public virtual async Task<OperationResult> CreateAsync(TEntity entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);
                await _context.SaveChangesAsync();

                return OperationResult.SuccessResult(entity, $"{typeof(TEntity).Name} creado exitosamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear {Entity}", typeof(TEntity).Name);
                return OperationResult.FailureResult($"Error al crear {typeof(TEntity).Name}: {ex.Message}");
            }
        }

        public virtual async Task<OperationResult> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _dbSet.FindAsync(id);
                return entity != null
                    ? OperationResult.SuccessResult(entity)
                    : OperationResult.FailureResult($"{typeof(TEntity).Name} no encontrado.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar {Entity} por ID", typeof(TEntity).Name);
                return OperationResult.FailureResult(ex.Message);
            }
        }

        public virtual async Task<OperationResult> GetAllAsync()
        {
            try
            {
                var list = await _dbSet.ToListAsync();
                return OperationResult.SuccessResult(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los registros de {Entity}", typeof(TEntity).Name);
                return OperationResult.FailureResult(ex.Message);
            }
        }

        public virtual async Task<OperationResult> GetAllAsync(Expression<Func<TEntity, bool>> filter)
        {
            try
            {
                var list = await _dbSet.Where(filter).ToListAsync();
                return OperationResult.SuccessResult(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al filtrar registros de {Entity}", typeof(TEntity).Name);
                return OperationResult.FailureResult(ex.Message);
            }
        }

        public virtual async Task<OperationResult> UpdateAsync(TEntity entity)
        {
            try
            {
                _dbSet.Update(entity);
                await _context.SaveChangesAsync();
                return OperationResult.SuccessResult(entity, $"{typeof(TEntity).Name} actualizado correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar {Entity}", typeof(TEntity).Name);
                return OperationResult.FailureResult($"Error al actualizar {typeof(TEntity).Name}: {ex.Message}");
            }
        }

        public virtual async Task<OperationResult> DeleteAsync(int id)
        {
            try
            {
                var entity = await _dbSet.FindAsync(id);
                if (entity == null)
                    return OperationResult.FailureResult($"{typeof(TEntity).Name} no encontrado.");

                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();

                return OperationResult.SuccessResult(null, $"{typeof(TEntity).Name} eliminado exitosamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar {Entity}", typeof(TEntity).Name);
                return OperationResult.FailureResult(ex.Message);
            }
        }

        public virtual async Task<bool> ExistsAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            return entity != null;
        }

        public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await _dbSet.AnyAsync(filter);
        }
    }
}

