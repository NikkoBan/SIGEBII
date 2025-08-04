using SIGEBI.Application.DTOsAplication.LoanDTOs;
using SIGEBI.Application.Interfaces.Mappers;
using SIGEBI.Domain.Entities;

namespace SIGEBI.Application.Mappers
{
    public class MapperLoanDto : IMapperLoanDto
    {
        public LoanDisplayDTO ToDisplayDto(Loan loan)
        {
            return new LoanDisplayDTO
            {
                Id = loan.LoanId,
                LoanDate = loan.LoanDate,
                UserId = loan.UserId,
                StatusId = loan.LoanStatusId,
                StatusName = loan.LoanStatus?.StatusName,
                // BookTitles: no mapeamos aquí, asumo se llena aparte
            };
        }

        public Loan FromCreationDto(LoanCreationDTO dto)
        {
            return new Loan
            {
                LoanDate = dto.LoanDate,
                UserId = dto.UserId,
                LoanStatusId = dto.StatusId,
                // Los BookIds se manejan en LoanDetails, no aquí directamente
            };
        }

        public void UpdateEntity(Loan loan, LoanUpdateDTO dto)
        {
            loan.LoanId = dto.Id;  // Por si acaso, aunque normalmente no se cambia el Id
            loan.LoanDate = dto.LoanDate;
            loan.UserId = dto.UserId;
            loan.LoanStatusId = dto.StatusId;
            // Lo mismo con BookIds
        }
    }
}
