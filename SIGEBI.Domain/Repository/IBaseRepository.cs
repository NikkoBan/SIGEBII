using SIGEBI.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SIGEBI.Domain.Repository
{
    /// <summary>
    /// Interfaz de herencia para los repositorios.
    /// </summary>
    /// <typeparam name="TEntity">Entidad</typeparam>
    /// <typeparam name="Ttype">Tipo de la llave primaria</typeparam>
    public interface IBaseRepository<TEntity, Ttype> where TEntity : class
    {
        Task<OperationResult<TEntity>> SaveEntityAsync(TEntity entity);
        Task<OperationResult<TEntity>> UpdateEntityAsync(TEntity entity);
        Task<TEntity?> GetEntityByIdAsync(Ttype id);
        Task<List<TEntity>> GetAllAsync();
        Task<OperationResult<List<TEntity>>> GetAllAsync(Expression<Func<TEntity, bool>> filter);
        Task<bool> Exists(Expression<Func<TEntity, bool>> filter);
        Task<OperationResult<TEntity>> RemoveEntityAsync(TEntity entity);
    }
}
