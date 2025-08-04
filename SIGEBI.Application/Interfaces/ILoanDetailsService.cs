using SIGEBI.Application.DTOsAplication.LoanDetailsDTOs;
using SIGEBI.Persistence.Base;

namespace SIGEBI.Application.Interfaces
{
    public interface ILoanDetailsService
    {
        Task<OperationResult<LoanDetailsDisplayDTO>> CreateAsync(LoanDetailsCreationDTO dto);
        Task<OperationResult<LoanDetailsDisplayDTO>> UpdateAsync(int id, LoanDetailsUpdateDTO dto);
        Task<OperationResult<bool>> DeleteAsync(int id);
        Task<OperationResult<LoanDetailsDisplayDTO>> GetByIdAsync(int id);
        Task<OperationResult<List<LoanDetailsDisplayDTO>>> GetAllAsync();
    }
}
