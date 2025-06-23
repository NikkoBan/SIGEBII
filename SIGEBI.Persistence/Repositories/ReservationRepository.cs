using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using SIGEBI.Application.Contracts.Repositories.Reservations;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities.circulation;
using SIGEBI.Persistence.Context;
using SIGEBI.Application.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;

namespace SIGEBI.Persistence.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly SIGEBIContext _context;
        private readonly IAppLogger<ReservationRepository> _logger; // El profe dijo que abstrayeramos Logger siguiendo Dependendy Injection.

        public ReservationRepository(SIGEBIContext context, IAppLogger<ReservationRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        private OperationResult ValidateReservation(Reservation entity)
        {
            if (entity == null)
            {
                return OperationResult.Failure("Reservation entity cannot be null.");
            }
            if (entity.BookId <= 0)
            {
                return OperationResult.Failure("BookId is required.");
            }
            if (entity.UserId <= 0)
            {
                return OperationResult.Failure("UserId is required.");
            }
            if (entity.ReservationDate < DateTime.Today)
            {
                return OperationResult.Failure("Reservation date cannot be in the past.");
            }
            if (entity.ExpirationDate <= entity.ReservationDate)
            {
                return OperationResult.Failure("Expiration date must be after the reservation date.");
            }
            return null;
        }
        private async Task<Reservation?> FindReservationAsync(int id)
        {
            _logger.LogInformation("Searching for reservation with Id: {Id}", id);
            var reservation = await _context.Reservations.FindAsync(id);

            if (reservation == null)
            {
                _logger.LogWarning("Reservation with Id {Id} not found.", id);
            }
            return reservation;
        }
        public async Task<OperationResult> AddAsync(Reservation entity)
        {
            OperationResult operationResult = new OperationResult();
            try 
            {
                _logger.LogInformation("Adding reservation entity: {@Reservation}", entity);

                var validationResult = ValidateReservation(entity);
                if (validationResult != null)
                {
                    _logger.LogError("Validation failed for reservation entity: {@Entity}", entity);
                    return validationResult;
                }

                entity.ReservationStatus ??= "Pending"; // Default status if not provided
                await _context.Reservations.AddAsync(entity);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Reservation added successfully.");
                operationResult = OperationResult.Success("Reservation added successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding reservation entity.");
                operationResult = OperationResult.Failure("An Error occurred adding the reservation entity: " + ex.Message);
            }
            return operationResult;

        }
        public async Task<bool> ExistsAsync(Expression<Func<Reservation, bool>> filter)
        {
            return await _context.Reservations.AnyAsync(filter);
        }

        public async Task<OperationResult> GetAllAsync(Expression<Func<Reservation, bool>> filter)
        {
            OperationResult operationResult = new OperationResult();

            try
            {
                _logger.LogInformation("Retrieving reservations with filter: {@Filter}", filter);
                operationResult.Data = await _context.Reservations.Where(filter).ToListAsync();

                operationResult = OperationResult.Success("Retrieving Reservations entities", operationResult.Data);

                _logger.LogInformation("Reservations retrieved successfully: {@Data}", operationResult.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving reservations.");
                operationResult = OperationResult.Failure("An error occurred retrieving reservations: " + ex.Message);
            }

           return operationResult; 
        }
        public async Task<OperationResult> GetByIdAsync(int id)
        {
            OperationResult operationResult = new OperationResult();

            try
            {
                _logger.LogInformation("Retrieving reservation by Id: {Id}", id);
                var reservation = await FindReservationAsync(id);
                if (reservation == null)
                {
                    _logger.LogWarning("Reservation with Id {Id} not found.", id);
                }

                operationResult = OperationResult.Success("Reservation retrieved successfully.", reservation);
                _logger.LogInformation("Reservation retrieved successfully: {@Data}", operationResult.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving reservation by Id: {Id}", id);
                operationResult = OperationResult.Failure("An error occurred retrieving the reservation: " + ex.Message);
            }
            return operationResult;
        }
        public async Task<OperationResult> UpdateAsync(Reservation entity)
        {
            OperationResult operationResult = new OperationResult();

            try
            {
                _logger.LogInformation("Updating reservation entity: {@Entity}", entity);

                var validationResult = ValidateReservation(entity);
                if (validationResult != null)
                {
                    _logger.LogError("Validation failed.");
                    return OperationResult.Failure(ValidateReservation(entity).Message);
                }

                var reservation = await FindReservationAsync(entity.Id);
                if (reservation == null)
                {
                    return OperationResult.Failure("Reservation entity not found.");
                }

                reservation.BookId = entity.BookId; 
                reservation.UserId = entity.UserId;
                reservation.ReservationDate = entity.ReservationDate;
                reservation.ExpirationDate = entity.ExpirationDate;
                reservation.ConfirmationDate = entity.ConfirmationDate;
                reservation.ReservationStatus = entity.ReservationStatus ?? "Pending";
                reservation.UpdatedAt = DateTime.UtcNow;
                reservation.UpdatedBy = Environment.UserName;

                _context.Reservations.Update(reservation);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Reservation updated successfully: {@Entity}", reservation);
                operationResult = OperationResult.Success("Reservation entity updated successfully.", reservation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating reservation entity: {@Entity}", entity);
                operationResult = OperationResult.Failure("An error occurred updating the reservation entity: " + ex.Message);
            }

            return operationResult;
        }
        public async Task<OperationResult> DeleteAsync(Reservation entity)
        {
            OperationResult operationResult = new OperationResult();
            try
            {
                if (entity == null)
                {
                    _logger.LogError("Attempted to delete a null reservation entity.");
                    return operationResult = OperationResult.Failure("Reservation entity cannot be null.");
                }

                _logger.LogInformation("Deleting Reservation entity: {@Entity}", entity);

                var existingEntity = await FindReservationAsync(entity.Id); // Corrected variable name
                if (existingEntity == null)
                {
                    return operationResult = OperationResult.Failure("Reservation entity not found.");
                }

                existingEntity.IsDeleted = true;
                existingEntity.DeletedBy = Environment.UserName;
                // añadir quiza fecha de borrado en la bd

                _context.Reservations.Update(existingEntity);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Reservation entity deleted successfully.");
                operationResult = OperationResult.Success("Reservation entity deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting reservation entity.");
                operationResult = OperationResult.Failure("An error occurred deleting the reservation entity: " + ex.Message);
            }
            return operationResult;
        }

    }
}
