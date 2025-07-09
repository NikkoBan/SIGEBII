using SIGEBI.Domain.Entities.circulation;
using SIGEBI.Application.Contracts.Services;
using SIGEBI.Application.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;

namespace SIGEBI.api.Services
{
    public class ReservationApiService
    {
        private readonly IReservationService _reservationService;

        public ReservationApiService(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        public async Task<ReservationDto> CreateReservationAsync(CreateReservationRequestDto requestDto)
        {
            var operationResult = await _reservationService.CreateReservationAsync(requestDto);

            if (!operationResult.IsSuccess)
            {
                throw new InvalidOperationException("Failed to create reservation.");
            }

            var createdReservation = operationResult.Data as Reservation;

            if (createdReservation == null)
            {
                throw new InvalidOperationException("Reservation was created but no data was returned.");
            }

            // Fix: Add null checks to prevent dereferencing null references
            var resultDto = new ReservationDto
            {
                ReservationId = createdReservation.Id,
                UserName = createdReservation.User?.FullName ?? "N/A",
                BookTitle = createdReservation.Book?.Title ?? "N/A",
                ReservationDate = createdReservation.ReservationDate,
                StatusName = createdReservation.ReservationStatus?.StatusName ?? "Pendiente",
                ExpirationDate = null
            };
            return resultDto;
        }
        public async Task<List<ReservationDto>> GetAllReservationsAsync()
        {
            var operationResult = await _reservationService.GetAllReservationsAsync();

            if (!operationResult.IsSuccess)
            {
                throw new InvalidOperationException("Failed to retrieve reservations."); //linea 51
            }

            if (operationResult.Data is List<ReservationDto> reservations)
            {
                return reservations;
            }
            throw new InvalidOperationException("No reservations found."); 

        }
        public async Task<ReservationDto> GetReservationByIdAsync(int id)
        {
            var operationResult = await _reservationService.GetReservationByIdAsync(id);
            if (!operationResult.IsSuccess)
            {
                throw new InvalidOperationException("Failed to retrieve reservation.");
            }

            var reservation = operationResult.Data as ReservationDto;
            if (reservation == null)
            {
                throw new InvalidOperationException("Reservation not found.");
            }
            return reservation;
        }
        public async Task DeleteReservationAsync(int id, string deletedBy)
        {
            var operationResult = await _reservationService.DeleteReservationAsync(id, deletedBy);
            if (!operationResult.IsSuccess)
            {
                throw new InvalidOperationException("Failed to delete reservation.");
            }
        }
    }
}

