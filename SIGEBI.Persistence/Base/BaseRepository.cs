using Microsoft.EntityFrameworkCore;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Repository;
using SIGEBI.Persistence.Context;
using System.Collections.Generic;
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

            public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> filter)
            {
                return await Entity.AnyAsync(filter);
            }

            public virtual async Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? filter = null)
            {
                if (filter != null)
                    return await Entity.Where(filter).ToListAsync();
                return await Entity.ToListAsync();
            }

            public virtual async Task<TEntity?> GetByIdAsync(Ttype id)
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
                catch (Exception)
                {
                    result.Success = false;
                    result.Message = "Ocurrió un error guardando los datos.";
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
                catch (Exception)
                {
                    result.Success = false;
                    result.Message = "Ocurrió un error actualizando los datos.";
                }
                return result;
            }

            public virtual async Task<OperationResult> RemoveAsync(Ttype id)
            {
                var result = new OperationResult();
                try
                {
                    var entity = await Entity.FindAsync(id);
                    if (entity == null)
                    {
                        result.Success = false;
                        result.Message = "Entidad no encontrada.";
                        return result;
                    }
                    Entity.Remove(entity);
                    await _context.SaveChangesAsync();
                    result.Success = true;
                }
                catch (Exception)
                {
                    result.Success = false;
                    result.Message = "Ocurrió un error eliminando los datos.";
                }
                return result;
            }

            public abstract Task<OperationResult> SaveEntityAsync(TEntity entity);
            public abstract Task<OperationResult> UpdateEntityAsync(TEntity entity);
            public abstract Task<TEntity?> GetEntityByIdAsync(Ttype id);
            public abstract Task<List<TEntity>> GetAllAsync();

            Task<OperationResult> IBaseRepository<TEntity, Ttype>.GetAllAsync(Expression<Func<TEntity, bool>> filter)
            {
                throw new NotImplementedException();
            }

            public abstract Task<bool> Exists(Expression<Func<TEntity, bool>> filter);
            public abstract Task<OperationResult> RemoveEntityAsync(TEntity entity);
        }
    }

