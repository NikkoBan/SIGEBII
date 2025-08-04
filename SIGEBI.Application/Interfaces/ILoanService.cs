using SIGEBI.Application.DTOsAplication.LoanDTOs;
using SIGEBI.Persistence.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIGEBI.Application.Interfaces
{
    public interface ILoanService
    {
        Task<OperationResult<List<LoanDisplayDTO>>> GetAllAsync();
        Task<OperationResult<LoanDisplayDTO>> GetByIdAsync(int id);
        Task<OperationResult<LoanDisplayDTO>> CreateAsync(LoanCreationDTO dto);
        Task<OperationResult<LoanDisplayDTO>> UpdateAsync(int id, LoanUpdateDTO dto);
        Task<OperationResult<bool>> DeleteAsync(int id);
    }
}
