using SIGEBI.Application.DTOsAplication.LoanStatusDTOs;
using SIGEBI.Persistence.Base;

namespace SIGEBI.Application.Interfaces
{
    public interface ILoanStatusService
    {
        Task<OperationResult<LoanStatusDisplayDTO>> CreateAsync(LoanStatusCreationDTO dto);
        Task<OperationResult<LoanStatusDisplayDTO>> UpdateAsync(int id, LoanStatusUpdateDTO dto);
        Task<OperationResult<bool>> DeleteAsync(int id);
        Task<OperationResult<LoanStatusDisplayDTO>> GetByIdAsync(int id);
        Task<OperationResult<List<LoanStatusDisplayDTO>>> GetAllAsync();
    }
}
