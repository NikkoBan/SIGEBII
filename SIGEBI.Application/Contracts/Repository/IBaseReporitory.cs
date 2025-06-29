using SIGEBI.Domain.Base;
using System.Threading.Tasks;
using System.Linq.Expressions;

using SIGEBI.Domain.Base;



namespace SIGEBI.Application.Contracts.Repository
{



    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task<OperationResult> CreateAsync(TEntity entity);
        Task<OperationResult> GetByIdAsync(int id);
        Task<OperationResult> GetAllAsync(); 
        Task<OperationResult> UpdateAsync(TEntity entity);
        Task<OperationResult> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id); 
        Task<OperationResult> CreateAsync(AuditableEntity entity);
    }

}