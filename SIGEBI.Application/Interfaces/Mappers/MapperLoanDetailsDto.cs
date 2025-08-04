using SIGEBI.Application.DTOsAplication.LoanDetailsDTOs;
using SIGEBI.Application.Interfaces.Mappers;
using SIGEBI.Domain.Entities;

namespace SIGEBI.Application.Mappers
{
    public class MapperLoanDetailsDto : IMapperLoanDetailsDto
    {
        public LoanDetailsDisplayDTO ToDisplayDto(LoanDetail entity)
        {
            return new LoanDetailsDisplayDTO
            {
                LoanDetailId = entity.LoanDetailId,
                BookId = entity.BookId,
                LoanId = entity.LoanId,
                Quantity = entity.Quantity
            };
        }

        public LoanDetail FromCreationDto(LoanDetailsCreationDTO dto)
        {
            return new LoanDetail
            {
                BookId = dto.BookId,
                LoanId = dto.LoanId,
                Quantity = dto.Quantity
            };
        }

        public void UpdateEntity(LoanDetail entity, LoanDetailsUpdateDTO dto)
        {
            entity.BookId = dto.BookId;
            entity.LoanId = dto.LoanId;
            entity.Quantity = dto.Quantity;
        }
    }
}
