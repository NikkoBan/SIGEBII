using SIGEBI.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Domain.Repository
{
    public interface IBaseRepository<TEntity, Ttype> where TEntity : class
    {
        Task<OperationResult> SaveEntityAsync(TEntity entity);
        Task<OperationResult> UpdateEntityAsync(TEntity entity);
        Task<TEntity?> GetEntityByIdAsync(Ttype id);
        Task<List<TEntity>> GetAllAsync();
        Task<OperationResult> GetAllAsync(Expression<Func<TEntity, bool>> filter);
        Task<bool> Exists(Expression<Func<TEntity, bool>> filter);
        Task<OperationResult> RemoveEntityAsync(TEntity entity);
    }
}
