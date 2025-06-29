using SIGEBI.Application.Contracts.Repositories;
using SIGEBI.Application.Contracts.Repositories.Reservations;
using SIGEBI.Application.Contracts.Services;
using SIGEBI.Application.DTOs;
using SIGEBI.Application.Validations;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities.circulation;

namespace SIGEBI.Application.Services
{
    public class ReservationHistoryService : IReservationHistoryService
    {
        private readonly IReservationHistoryRepository _reservationHistoryRepository;
        public ReservationHistoryService(IReservationHistoryRepository reservationHistoryRepository)
        {
            _reservationHistoryRepository = reservationHistoryRepository;
        }
        public async Task<OperationResult> GetAllHistoriesAsync()
        {
            var result = await _reservationHistoryRepository.GetAllAsync();

            if (result.IsSuccess)
            {
                return OperationResult.Success("Reservation histories retrieved successfully.", result.Data);
            }
            return OperationResult.Failure(result.Message);
        }
        public async Task<OperationResult> GetHistoryByIdAsync(int id)
        {
            var validationResult = ReservationValidator.ValidateReservationId(id);

            if (id <= 0) 
            {
                return OperationResult.Failure("Invalid history ID");
            }

            var result = await _reservationHistoryRepository.GetByIdAsync(id);

            if (!result.IsSuccess) 
            {
                return OperationResult.Failure(result.Message);
            }

            var history = result.Data as ReservationHistory;

            if (history == null)
            {
                return OperationResult.Failure("Reservation history not found.");
            }
            // Map ReservationHistory to ReservationHistoryDto
            var historyDto = new ReservationHistoryDto
            {
                HistoryId = history.HistoryId,
                ReservationId = history.ReservationId,
                StatusId = history.StatusId,
                ReservationDate = history.ReservationDate,
                ExpirationDate = history.ExpirationDate,
            };
            return OperationResult.Success("Reservation history retrieved successfully.", historyDto);
        }
    }
}
