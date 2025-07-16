

using SIGEBI.Domain.Base;

namespace SIGEBI.Domain.IRepository
{
    public interface IBaseRepository<T> where T : AuditableEntity
    {
        Task<OperationResult> GetByIdAsync(int id);

        Task<OperationResult>GetAllAsync();

        Task<OperationResult> CreateAsync(T entity);

        Task<OperationResult> UpdateAsync(T entity);

        Task<OperationResult> DeleteAsync(int id);

        Task<bool> ExistsAsync(int id);
    }


}
