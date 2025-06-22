
using System.Linq.Expressions;

using SIGEBI.Domain.Base;



namespace SIGEBI.Application.Contracts.Repository
{
    namespace SIGEBI.Persistence.Repositories
    {
        public interface IBaseRepository<TEntity> where TEntity : class
        {
            Task<OperationResult> CreateAsync(TEntity entity);
            Task<OperationResult> GetByIdAsync(int id);
            Task<OperationResult> GetAllAsync();
            Task<OperationResult> UpdateAsync(TEntity entity);
            Task<OperationResult> DeleteAsync(int id);
            Task<OperationResult> GetAllAsync(Expression<Func<TEntity, bool>> filter);
            Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> filter);
        }
    }
}