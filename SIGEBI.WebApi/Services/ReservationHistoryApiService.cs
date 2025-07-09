using SIGEBI.Application.Contracts.Services;
using SIGEBI.Application.DTOs;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities.circulation;

namespace SIGEBI.WebApi.Services
{
    public class ReservationHistoryApiService
    {
        private readonly IReservationHistoryService _reservationHistoryService;

        public ReservationHistoryApiService(IReservationHistoryService reservationHistoryService)
        {
            _reservationHistoryService = reservationHistoryService;
        }

        public async Task<OperationResult> GetAllAsync() //falta este
        {
            var result = await _reservationHistoryService.GetAllHistoriesAsync();

            if (!result.IsSuccess)
                return result;

            var histories = (List<ReservationHistory>)result.Data;

            var dtoList = histories.Select(h => new GetAllReservationHistoryDto
            {
                HistoryId = h.ReservationHistoryId,
                ReservationId = h.ReservationId,
                BookTitle = h.Book?.Title ?? "", // Evita null
                UserName = h.User?.FullName ?? "",
                StatusName = h.ReservationStatus?.StatusName ?? "",
                ReservationDate = h.ReservationDate,
                ExpirationDate = h.ExpirationDate
            }).ToList();

            return OperationResult.Success("Reservation histories retrieved successfully.", dtoList);
        }

        public async Task<ReservationHistoryByIdDto> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new InvalidOperationException("Invalid ID.");

            var result = await _reservationHistoryService.GetHistoryByIdAsync(id);
            var entity = result.Data as ReservationHistory;

            if (entity == null)
                throw new InvalidOperationException("Reservation history not found.");

            return new ReservationHistoryByIdDto
            {
                HistoryId = entity.ReservationHistoryId,
                ReservationId = entity.ReservationId,
                StatusName = entity.ReservationStatus?.StatusName ?? "",
            };
        }
        //public async Task<OperationResult> GetByIdAsync(int id)
        //{
        //    var result = await _reservationHistoryService.GetHistoryByIdAsync(id);

        //    if (!result.IsSuccess)
        //        return result;

        //    var entity = result.Data as ReservationHistory;

        //    if (entity == null)
        //        return OperationResult.Failure("Invalid entity returned.");

        //    var dto = new ReservationHistoryByIdDto
        //    {
        //        HistoryId = entity.ReservationHistoryId,
        //        ReservationId = entity.ReservationId,
        //        StatusName = entity.ReservationStatus?.StatusName ?? "",

        //    };

        //    return OperationResult.Success("Reservation history retrieved successfully.", dto);
        //}
    }
}
