using SIGEBI.Domain.Entities;
using SIGEBI.Persistence.Base;

namespace SIGEBI.Persistence.Interface
{
    public interface ILoanRepository
    {
        Task<OperationResult<List<Loan>>> GetAllAsync();
        Task<OperationResult<Loan>> GetByIdAsync(int id);
        Task<OperationResult<Loan>> AddAsync(Loan entity);
        Task<OperationResult<bool>> UpdateAsync(Loan entity);
        Task<OperationResult<bool>> DeleteAsync(int id);
    }
}
