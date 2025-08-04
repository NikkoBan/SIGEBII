using SIGEBI.Application.DTOsAplication.LoanHistoryDTOs;
using SIGEBI.Persistence.Base;

namespace SIGEBI.Application.Interfaces
{
    public interface ILoanHistoryService
    {
        Task<OperationResult<LoanHistoryDisplayDTO>> CreateAsync(LoanHistoryCreationDTO dto);
        Task<OperationResult<LoanHistoryDisplayDTO>> UpdateAsync(int id, LoanHistoryUpdateDTO dto);
        Task<OperationResult<bool>> DeleteAsync(int id);
        Task<OperationResult<LoanHistoryDisplayDTO>> GetByIdAsync(int id);
        Task<OperationResult<List<LoanHistoryDisplayDTO>>> GetAllAsync();
    }
}
