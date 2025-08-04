using SIGEBI.Application.DTOsAplication.LoanStatusDTOs;
using SIGEBI.Application.Interfaces.MappersInterfaces;
using SIGEBI.Domain.Entities;

namespace SIGEBI.Application.Mappers
{
    public class MapperLoanStatusDto : IMapperLoanStatusDto
    {
        public LoanStatusDisplayDTO ToDisplayDto(LoanStatus entity)
        {
            return new LoanStatusDisplayDTO
            {
                LoanStatusId = entity.StatusId,
                Name = entity.StatusName
            };
        }

        public LoanStatus FromCreationDto(LoanStatusCreationDTO dto)
        {
            return new LoanStatus
            {
                StatusName = dto.Name
            };
        }

        public void UpdateEntity(LoanStatus entity, LoanStatusUpdateDTO dto)
        {
            entity.StatusName = dto.Name;
        }
    }
}
