using SIGEBI.Application.Contracts.Repositories;
using SIGEBI.Application.Contracts.Repositories.Reservations;
using SIGEBI.Application.Contracts.Services;
using SIGEBI.Application.DTOs;
using SIGEBI.Application.Validations;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities.circulation;

namespace SIGEBI.Application.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;

        public ReservationService(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

        public async Task<OperationResult> GetAllReservationsAsync()
        {
            var result = await _reservationRepository.GetAllAsync(x => true);

            if (result.IsSuccess)
            {
                // Aquí mapearías el resultado a DTOs
                // Por ahora retorno el resultado tal como viene del repositorio
                return OperationResult.Success("Reservations retrieved successfully.", result.Data);
            }

            return OperationResult.Failure(result.Message);
        }

        public async Task<OperationResult> GetReservationByIdAsync(int id)
        {
            var validationResult = ReservationValidator.ValidateReservationId(id);
            if (!validationResult.IsSuccess) 
            {
                return OperationResult.Failure(validationResult.Message);
            }

            var result = await _reservationRepository.GetByIdAsync(id);

            if (!result.IsSuccess) 
            {
                return OperationResult.Failure(result.Message);
            }

            var reservation = result.Data as Reservation;
            if (reservation == null)
            {
                return OperationResult.Failure("Reservation not found.");
            }

            // Map Reservation to ReservationDto
            var reservationDto = new ReservationDto
            {
                Id = reservation.Id,
                BookId = reservation.BookId,
                UserId = reservation.UserId,
                StatusId = reservation.StatusId,
                ReservationDate = reservation.ReservationDate,
                ExpirationDate = reservation.ExpirationDate,
                ConfirmationDate = reservation.ConfirmationDate,
                CreatedAt = reservation.CreatedAt,
                CreatedBy = reservation.CreatedBy,
                UpdatedAt = reservation.UpdatedAt,
                UpdatedBy = reservation.UpdatedBy
            };

            return OperationResult.Success("Reservation retrieved successfully.", reservationDto);
        }

        public async Task<OperationResult> CreateReservationAsync(CreateReservationRequestDto request)
        {
            var reservation = new Reservation
            {
                BookId = request.BookId,
                UserId = request.UserId,
                ReservationDate = request.ReservationDate,
                //ExpirationDate = request.ExpirationDate,
                CreatedBy = "System", // Set required CreatedBy property
                UpdatedBy = "System", // Set required UpdatedBy property
                DeletedBy = string.Empty // Set required DeletedBy property
            };

            var validationResult = ReservationValidator.ValidateReservation(reservation);
            if (!validationResult.IsSuccess)
            {
                return OperationResult.Failure(validationResult.Message);
            }

            var result = await _reservationRepository.AddAsync(reservation);

            if (!result.IsSuccess)
            {
                return OperationResult.Failure(result.Message);
            }

            return OperationResult.Success("Reservation created successfully.");
        }

        public async Task<OperationResult> UpdateReservationAsync(UpdateReservationDto request)
        {
            var reservation = new Reservation
            {
                Id = request.Id,
                BookId = request.BookId,
                UserId = request.UserId,
                StatusId = request.StatusId,
                ReservationDate = request.ReservationDate,
                ExpirationDate = request.ExpirationDate,
                CreatedBy = "System",
                UpdatedBy = "System", 
                DeletedBy = string.Empty // Set required DeletedBy property
            };

            var validationResult = ReservationValidator.ValidateReservation(reservation);
            if (!validationResult.IsSuccess)
            {
                return OperationResult.Failure(validationResult.Message);
            }

            var result = await _reservationRepository.UpdateAsync(reservation);

            if (!result.IsSuccess)
            {
                return OperationResult.Failure(result.Message);
            }

            return OperationResult.Success("Reservation updated successfully.");
        }

        public async Task<OperationResult> DeleteReservationAsync(int id)
        {
            var validationResult = ReservationValidator.ValidateReservationId(id);
            if (!validationResult.IsSuccess) 
            {
                return OperationResult.Failure(validationResult.Message);
            }

            var result = await _reservationRepository.DisableAsync(id, "System");

            if (!result.IsSuccess) 
            {
                return OperationResult.Failure(result.Message);
            }

            return OperationResult.Success("Reservation deleted successfully.");
        }

        public async Task<OperationResult> ConfirmReservationAsync(ConfirmReservationRequestDto request)
        {
            var validationResult = ReservationValidator.ValidateReservationId(request.ReservationId);
            if (!validationResult.IsSuccess)
            {
                return OperationResult.Failure(validationResult.Message);
            }

            var result = await _reservationRepository.ConfirmReservationAsync(request.ReservationId);

            if (!result.IsSuccess)
            {
                return OperationResult.Failure(result.Message);
            }

            return OperationResult.Success("Reservation confirmed successfully.");
        }

        public async Task<OperationResult> ExpireConfirmedReservationsAsync()
        {
            var result = await _reservationRepository.ExpireConfirmedReservationsAsync();

            if (!result.IsSuccess)
            {
                return OperationResult.Failure(result.Message);
            }

            return OperationResult.Success("Expired reservations processed successfully.");
        }


        //private readonly IReservationRepository _repository;
        //public ReservationService(IReservationRepository repository)
        //{
        //    _repository = repository;
        //}
        //public async Task<Reservation> CreateAsync(Reservation reservation)
        //{
        //    await _repository.AddAsync(reservation);
        //    return reservation;
        //}
    }
}
    
