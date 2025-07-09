using Microsoft.EntityFrameworkCore;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Repository;
using SIGEBI.Persistence.Context;
using System.Linq.Expressions;

namespace SIGEBI.Persistence.Base
{
    public abstract class BaseRepository<TEntity, Ttype> : IBaseRepository<TEntity, Ttype> where TEntity : class
    {
        private readonly SIGEBIDbContext _context;
        protected DbSet<TEntity> Entity { get; }

        protected BaseRepository(SIGEBIDbContext context)
        {
            _context = context;
            Entity = _context.Set<TEntity>();
        }

        public virtual async Task<bool> Exists(Expression<Func<TEntity, bool>> filter)
            => await Entity.AnyAsync(filter);

        public virtual async Task<OperationResult<List<TEntity>>> GetAllAsync(Expression<Func<TEntity, bool>> filter)
        {
            try
            {
                var result = await Entity.Where(filter).ToListAsync();
                return OperationResult<List<TEntity>>.SuccessResult(result, "Consulta realizada con éxito.");
            }
            catch (Exception ex)
            {
                return OperationResult<List<TEntity>>.FailResult("Error al obtener los datos: " + ex.Message);
            }
        }

        public virtual async Task<OperationResult<TEntity>> SaveEntityAsync(TEntity entity)
        {
            try
            {
                await Entity.AddAsync(entity);
                await _context.SaveChangesAsync();
                return OperationResult<TEntity>.SuccessResult(entity, "Entidad guardada correctamente.");
            }
            catch (Exception ex)
            {
                return OperationResult<TEntity>.FailResult("Error al guardar la entidad: " + ex.Message);
            }
        }

        public virtual async Task<OperationResult<TEntity>> UpdateEntityAsync(TEntity entity)
        {
            try
            {
                Entity.Update(entity);
                await _context.SaveChangesAsync();
                return OperationResult<TEntity>.SuccessResult(entity, "Entidad actualizada correctamente.");
            }
            catch (Exception ex)
            {
                return OperationResult<TEntity>.FailResult("Error al actualizar la entidad: " + ex.Message);
            }
        }

        public virtual async Task<OperationResult<TEntity>> RemoveEntityAsync(TEntity entity)
        {
            try
            {
                Entity.Remove(entity);
                await _context.SaveChangesAsync();
                return OperationResult<TEntity>.SuccessResult(entity, "Entidad eliminada correctamente.");
            }
            catch (Exception ex)
            {
                return OperationResult<TEntity>.FailResult("Error al eliminar la entidad: " + ex.Message);
            }
        }

        public virtual async Task<TEntity?> GetEntityByIdAsync(Ttype id)
            => await Entity.FindAsync(id);

        public abstract Task<List<TEntity>> GetAllAsync();
    }
}
