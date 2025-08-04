using SIGEBI.Application.DTOsAplication.LoanHistoryDTOs;
using SIGEBI.Domain.Entities;

namespace SIGEBI.Application.Interfaces.MappersInterfaces
{
    public interface IMapperLoanHistoryDto
    {
        LoanHistoryDisplayDTO ToDisplayDto(LoanHistory entity);
        LoanHistory FromCreationDto(LoanHistoryCreationDTO dto);
        void UpdateEntity(LoanHistory entity, LoanHistoryUpdateDTO dto);
    }
}
