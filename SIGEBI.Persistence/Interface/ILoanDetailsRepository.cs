using SIGEBI.Domain.Entities;
using SIGEBI.Persistence.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIGEBI.Persistence.Interface
{
    public interface ILoanDetailsRepository
    {
        Task<OperationResult<List<LoanDetail>>> GetAllAsync();
        Task<OperationResult<LoanDetail>> GetByIdAsync(int id);
        Task<OperationResult<LoanDetail>> AddAsync(LoanDetail entity);
        Task<OperationResult<bool>> UpdateAsync(LoanDetail entity);
        Task<OperationResult<bool>> DeleteAsync(int id);
    }
}
