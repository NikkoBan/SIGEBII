using SIGEBI.Domain.Entities;
using SIGEBI.Persistence.Base;

namespace SIGEBI.Persistence.Interface
{
    public interface ILoanStatusRepository
    {
        Task<OperationResult<List<LoanStatus>>> GetAllAsync();
        Task<OperationResult<LoanStatus>> GetByIdAsync(int id);
        Task<OperationResult<LoanStatus>> AddAsync(LoanStatus entity);
        Task<OperationResult<bool>> UpdateAsync(LoanStatus entity);
        Task<OperationResult<bool>> DeleteAsync(int id);
    }
}
