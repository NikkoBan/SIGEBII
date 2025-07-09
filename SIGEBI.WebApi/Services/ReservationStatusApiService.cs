using SIGEBI.Application.Contracts.Services;
using SIGEBI.Application.DTOs;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities.circulation;

namespace SIGEBI.WebApi.Services
{
    public class ReservationStatusApiService
    {
        private readonly IReservationStatusService _reservationStatusService;

        public ReservationStatusApiService(IReservationStatusService reservationStatusService)
        {
            _reservationStatusService = reservationStatusService;
        }

        public async Task<IEnumerable<ReservationStatusDto>> GetAllAsync()
        {
            var result = await _reservationStatusService.GetAllStatusesAsync();

            if (!result.IsSuccess || result.Data == null)
                throw new InvalidOperationException(result.Message);

            var statuses = (List<ReservationStatus>)result.Data;

            return statuses.Select(s => new ReservationStatusDto
            {
                StatusId = s.Id,
                StatusName = s.StatusName
            }).ToList();
        }

        public async Task<ReservationStatusDto> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new InvalidOperationException("Invalid reservation status ID.");

            var result = await _reservationStatusService.GetStatusByIdAsync(id);

            var status = result.Data as ReservationStatus;
            if (status == null)
                throw new InvalidOperationException("Reservation status not found.");

            return new ReservationStatusDto
            {
                StatusId = status.Id,
                StatusName = status.StatusName
            };

            //public async Task<OperationResult> GetAllStatusesAsync()
            //{
            //    return await _reservationStatusService.GetAllStatusesAsync();

            //}

            //public async Task<OperationResult> GetStatusByIdAsync(int id)
            //{
            //    return await _reservationStatusService.GetStatusByIdAsync(id);
            //}
        }
    }
}
   


