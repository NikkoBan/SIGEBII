using SIGEBI.Application.DTOsAplication.LoanHistoryDTOs;
using SIGEBI.Application.Interfaces.Mappers;
using SIGEBI.Application.Interfaces.MappersInterfaces;
using SIGEBI.Domain.Entities;

namespace SIGEBI.Application.Mappers
{
    public class MapperLoanHistoryDto : IMapperLoanHistoryDto
    {
        public LoanHistoryDisplayDTO ToDisplayDto(LoanHistory entity)
        {
            return new LoanHistoryDisplayDTO
            {
                HistoryId = entity.HistoryId,
                LoanId = entity.LoanId,
                StatusId = entity.StatusId,
                ChangedAt = entity.ChangedAt,
                ChangedBy = entity.ChangedBy,
                Notes = entity.Notes
            };
        }

        public LoanHistory FromCreationDto(LoanHistoryCreationDTO dto)
        {
            return new LoanHistory
            {
                LoanId = dto.LoanId,
                StatusId = dto.StatusId,
                ChangedAt = dto.ChangedAt,
                ChangedBy = dto.ChangedBy,
                Notes = dto.Notes
            };
        }

        public void UpdateEntity(LoanHistory entity, LoanHistoryUpdateDTO dto)
        {
            entity.LoanId = dto.LoanId;
            entity.StatusId = dto.StatusId;
            entity.ChangedAt = dto.ChangedAt;
            entity.ChangedBy = dto.ChangedBy;
            entity.Notes = dto.Notes;
        }
    }
}
