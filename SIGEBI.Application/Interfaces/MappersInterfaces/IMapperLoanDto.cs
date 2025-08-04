using SIGEBI.Application.DTOsAplication.LoanDTOs;
using SIGEBI.Domain.Entities;

namespace SIGEBI.Application.Interfaces.Mappers
{
    public interface IMapperLoanDto
    {
        LoanDisplayDTO ToDisplayDto(Loan loan);
        Loan FromCreationDto(LoanCreationDTO dto);
        void UpdateEntity(Loan loan, LoanUpdateDTO dto);
    }
}
