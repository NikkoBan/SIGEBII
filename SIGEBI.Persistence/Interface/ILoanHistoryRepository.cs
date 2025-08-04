using SIGEBI.Domain.Entities;
using SIGEBI.Persistence.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIGEBI.Persistence.Interface
{
    public interface ILoanHistoryRepository
    {
        Task<OperationResult<List<LoanHistory>>> GetAllAsync();
        Task<OperationResult<LoanHistory>> GetByIdAsync(int id);
        Task<OperationResult<LoanHistory>> AddAsync(LoanHistory entity);
        Task<OperationResult<bool>> UpdateAsync(LoanHistory entity);
        Task<OperationResult<bool>> DeleteAsync(int id);
    }
}
