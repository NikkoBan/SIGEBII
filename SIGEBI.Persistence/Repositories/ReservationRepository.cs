using System.Data;
using System.Linq.Expressions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.Extensions.Logging;
using SIGEBI.Persistence.Interfaces;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities.circulation;
using SIGEBI.Persistence.Context;
using SIGEBI.Application.Contracts;
using SIGEBI.Application.Validations;

namespace SIGEBI.Persistence.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly SIGEBIContext _context;
        private readonly IAppLogger<ReservationRepository> _logger;

        public ReservationRepository(SIGEBIContext context, IAppLogger<ReservationRepository> logger)
        {
            _context = context;
            _logger = logger;
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

                //var validationResult = Application.Validations.ReservationValidator(entity);
                //if (validationResult != null)
                //{
                //    _logger.LogError("Validation failed for reservation entity: {@Entity}", entity);
                //    return validationResult;
                //}

                entity.StatusId = 1; // Default status if not provided

                var outputMessage = new SqlParameter("@Presult", SqlDbType.VarChar, 1000)
                {
                    Direction = ParameterDirection.Output
                };

                //SPs en vez de EF
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC CreateReservation @BookId, @UserId, @ReservationDate, @ExpirationDate, @StatusId, @CreatedAt, @CreatedBy, @Presult OUTPUT",
                    new SqlParameter("@BookId", entity.BookId),
                    new SqlParameter("@UserId", entity.UserId),
                    new SqlParameter("@ReservationDate", entity.ReservationDate),
                    new SqlParameter("@ExpirationDate", entity.ExpirationDate),
                    new SqlParameter("@StatusId", entity.StatusId),
                    new SqlParameter("@CreatedAt", DateTime.UtcNow),
                    new SqlParameter("@CreatedBy", Environment.UserName),
                    outputMessage
                    );

                var message = outputMessage.Value?.ToString() ?? "Reservation process completed.";


                _logger.LogInformation("Procedure result: {Message}", message);
                operationResult = OperationResult.Success(message);
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
                _logger.LogInformation("Exexuting stored procedure to retrieve reservations.");

                var reservations = await _context.ReservationsView
                    .FromSqlRaw("EXEC GetReservations")
                    .ToListAsync();

                operationResult = OperationResult.Success("Reservations retrieved successfully.", reservations);
                _logger.LogInformation("Reservations retrieved: {@Data}", reservations);

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

                //var validationResult = ValidateReservation(entity); // no es necesario ahora que tenemos las validaciones en application layer
                //if (validationResult != null)
                //{
                //    _logger.LogError("Validation failed.");
                //    return OperationResult.Failure(ValidateReservation(entity).Message);
                //}

                await _context.Database.ExecuteSqlRawAsync("EXEC ModifyReservation @ReservationId, @BookId, @UserId, @ReservationDate, @ExpirationDate, @StatusId, @UpdatedAt, @UpdatedBy",
                    new SqlParameter("@ReservationId", entity.Id),
                    new SqlParameter("@BookId", entity.BookId),
                    new SqlParameter("@UserId", entity.UserId),
                    new SqlParameter("@ReservationDate", entity.ReservationDate),
                    new SqlParameter("@ExpirationDate", entity.ExpirationDate),
                    new SqlParameter("@StatusId", entity.StatusId), // Handle nulls
                    new SqlParameter("@UpdatedAt", DateTime.UtcNow),
                    new SqlParameter("@UpdatedBy", Environment.UserName)
                );

                _logger.LogInformation("Reservation updated successfully: {@Entity}");
                operationResult = OperationResult.Success("Reservation entity updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating reservation entity: {@Entity}", entity);
                operationResult = OperationResult.Failure("An error occurred updating the reservation entity: " + ex.Message);
            }

            return operationResult;
        }
        public async Task<OperationResult> DisableAsync(int reservationId, string deletedBy)
        {
            OperationResult operationResult = new OperationResult();
            try
            {
                var outputMessage = new SqlParameter("@Presult", SqlDbType.VarChar, 1000)
                {
                    Direction = ParameterDirection.Output
                };

                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC DisableReservation @ReservationId, @DeletedBy, @Presult OUTPUT",
                    new SqlParameter("@ReservationId", reservationId),
                    new SqlParameter("@DeletedBy", Environment.UserName),
                    outputMessage
                );

                var message = outputMessage.Value?.ToString() ?? "Reservation deletion process completed.";

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
        public async Task<OperationResult> ConfirmReservationAsync(int reservationId)
        {
            OperationResult operationResult = new OperationResult();
            try
            {
                _logger.LogInformation("Reservation confirmation started with ID: {ReservationId}", reservationId);

                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC ConfirmReservation @ReservationId, @ConfirmedBy",
                    new SqlParameter("@ReservationId", reservationId),
                    new SqlParameter("@ConfirmedBy", Environment.UserName)
                );

                operationResult = OperationResult.Success("Reservation confirmed successfully.");
                _logger.LogInformation("Reservation with ID: {ReservationId} confirmed successfully.", reservationId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error confirming reservation with Id: {Id}", reservationId);
                operationResult = OperationResult.Failure("An error occurred confirming the reservation: " + ex.Message);
            }
            return operationResult;

        }
        public async Task<OperationResult> ExpireConfirmedReservationsAsync()
        {
            OperationResult operationResult = new OperationResult();
            try
            {
                _logger.LogInformation("Running SP: ExpireconfirmedReservations");

                await _context.Database.ExecuteSqlRawAsync("EXEC ExpireConfirmedReservations");

                operationResult = OperationResult.Success("Expired confirmed reservations updated successfully.");
                _logger.LogInformation("Expired confirmed reservations updated.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error expiring confirmed reservations with Id: {Id}");
                operationResult = OperationResult.Failure("An error occurred expiring the confirmed reservations: " + ex.Message);
            }
            return operationResult;
        }
    }
}
