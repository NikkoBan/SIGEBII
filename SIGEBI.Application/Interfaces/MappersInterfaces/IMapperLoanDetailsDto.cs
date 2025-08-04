using SIGEBI.Application.DTOsAplication.LoanDetailsDTOs;
using SIGEBI.Domain.Entities;

namespace SIGEBI.Application.Interfaces.Mappers
{
    public interface IMapperLoanDetailsDto
    {
        LoanDetailsDisplayDTO ToDisplayDto(LoanDetail entity);
        LoanDetail FromCreationDto(LoanDetailsCreationDTO dto);
        void UpdateEntity(LoanDetail entity, LoanDetailsUpdateDTO dto);
    }
}
