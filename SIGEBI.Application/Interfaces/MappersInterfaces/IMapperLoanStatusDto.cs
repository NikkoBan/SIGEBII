using SIGEBI.Application.DTOsAplication.LoanStatusDTOs;
using SIGEBI.Domain.Entities;

namespace SIGEBI.Application.Interfaces.MappersInterfaces
{
    public interface IMapperLoanStatusDto
    {
        LoanStatusDisplayDTO ToDisplayDto(LoanStatus entity);
        LoanStatus FromCreationDto(LoanStatusCreationDTO dto);
        void UpdateEntity(LoanStatus entity, LoanStatusUpdateDTO dto);
    }
}
