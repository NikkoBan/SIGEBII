using SIGEBI.Persistence.Base;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace SIGEBI.Persistence.Interfaces
{
    public interface IBaseRepository<TEntity, TType> where TEntity : class
    {
        Task<TEntity?> GetByIdAsync(TType id);
        Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? filter = null);
        Task<OperationResult> AddAsync(TEntity entity);
        Task<OperationResult> UpdateAsync(TEntity entity);
        Task<OperationResult> RemoveAsync(TType id);
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> filter);
    }
}