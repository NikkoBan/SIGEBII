using SIGEBI.Application.Contracts.Repositories;
using SIGEBI.Application.Contracts.Repositories.Reservations;
using SIGEBI.Application.Contracts.Services;
using SIGEBI.Application.DTOs;
using SIGEBI.Application.Validations;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities.circulation;


namespace SIGEBI.Application.Services
{
    public class ReservationStatusService : IReservationStatusService
    {
        private readonly IReservationStatusesRepository _reservationStatusesRepository;

        public ReservationStatusService(IReservationStatusesRepository reservationStatusesRepository)
        {
            _reservationStatusesRepository = reservationStatusesRepository;
        }

        public async Task<OperationResult> GetAllStatusesAsync()
        {
            var result = await _reservationStatusesRepository.GetAllAsync(x => true);

            if (!result.IsSuccess)
            {
                return OperationResult.Failure(result.Message);
            }

            var statuses = new List<ReservationStatusDto>();

            foreach (var item in result.Data) 
            {
                var dto = new ReservationStatusDto
                {
                    Id = item.Id,
                    StatusName = item.StatusName,
                    Description = item.Description
                };
                statuses.Add(dto);
            }

            return OperationResult.Success("Reservation statuses retrieved successfully.", statuses);
        }

        public async Task<OperationResult> GetStatusByIdAsync(int id)
        {
            if (id <= 0)
            {
                return OperationResult.Failure("Invalid reservation status ID.");
            }

            var result = await _reservationStatusesRepository.GetByIdAsync(id);

            if (!result.IsSuccess)
            {
                return OperationResult.Failure(result.Message);
            }

            var status = result.Data as ReservationStatus;

            if (status == null)
            {
                return OperationResult.Failure("Reservation status not found.");
            }

            var dto = new ReservationStatusDto
            {
                Id = status.Id,
                StatusName = status.StatusName,
                Description = status.Description
            };

            return OperationResult.Success("Reservation status retrieved successfully.", dto);
        }
    }
}
