using System.Data;
using System.Linq.Expressions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.Extensions.Logging;
using SIGEBI.Application.Contracts;
using SIGEBI.Application.DTOs;
using SIGEBI.Application.Validations;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities.circulation;
using SIGEBI.Persistence.Context;
using SIGEBI.Persistence.Interfaces;

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

               if (entity == null)
               {
                    _logger.LogError("Attemted to add a null Reservation entity.");
                    return OperationResult.Failure("Reservation cannot be null");        
               }
                //EF               

                _logger.LogInformation("Adding reservation entity: {@Reservation}", entity);

                await _context.Reservations.AddAsync(entity);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Reservation entity added successfully: {@Entity}", entity);
                operationResult = OperationResult.Success("Reservation entity added succesfully", entity);
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

        public async Task<OperationResult> GetAllAsync(Expression<Func<Reservation, bool>> filter = null)
        {
            try
            {
                _logger.LogInformation("GetAllAsync: comenzando la consulta de reservas.");

                var query = _context.Reservations
                    .Include(r => r.Book)
                    .Include(r => r.User)
                    .Include(r => r.ReservationStatus)
                    .AsQueryable();

                if (filter != null)
                {
                    _logger.LogInformation("Aplicando filtro: {Filter}", filter.ToString());
                    query = query.Where(filter);
                }

                List<Reservation> reservations;

                try
                {
                    _logger.LogInformation("Ejecutando ToListAsync...");
                    reservations = await query.ToListAsync();
                    _logger.LogInformation("Cantidad de reservas obtenidas: {Count}", reservations.Count);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Fallo durante ToListAsync.");
                    return OperationResult.Failure("Error durante la ejecución de la consulta: " + ex.Message);
                }

                return OperationResult.Success("Consulta completada.", reservations ?? new List<Reservation>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado en GetAllAsync.");
                return OperationResult.Failure("Error inesperado: " + ex.Message);
            }
        }
        public async Task<OperationResult> GetByIdAsync(int id)
        {
            OperationResult operationResult = new OperationResult();

            try
            {
                _logger.LogInformation("Retrieving reservation entity by Id: {Id}", id);
                var reservation = await _context.Reservations.FindAsync(id);

                if (reservation == null || reservation.IsDeleted)
                {
                    _logger.LogWarning("Reservation with Id {Id} not found or is deleted.", id);
                    return OperationResult.Failure("Reservation not found.");
                }

                operationResult = OperationResult.Success("Reservation retrieved successfully.", reservation);
                _logger.LogInformation("Reservation retrieved successfully: {@Data}", reservation);
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
                if (entity == null)
                {
                    _logger.LogError("Attempted to update a null Reservation entity.");
                    return OperationResult.Failure("Reservation entity cannot be null.");
                }

                Reservation? reservation = await _context.Reservations.FindAsync(entity.Id);

                if (reservation is null)
                {
                    _logger.LogWarning("Reservation with Id {Id} not found for update.", entity.Id);
                    return OperationResult.Failure("Reservation not found for update.");
                }

                reservation.BookId = entity.BookId;
                reservation.UserId = entity.UserId;
                reservation.StatusId = entity.StatusId;
                reservation.ReservationDate = entity.ReservationDate;
                reservation.ExpirationDate = entity.ExpirationDate;
                reservation.ConfirmationDate = entity.ConfirmationDate;


                _context.Reservations.Update(entity);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Reservation updated successfully: {@Entity}", entity);
                return OperationResult.Success("Reservation entity updated successfully.", entity);
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
                    _logger.LogError("Attempted to delete a null Reservation entity.");
                    return OperationResult.Failure("Reservation cannot be null");
                }
                _logger.LogInformation("Deleting reservation entity: {@Entity}", entity);

                Reservation? existingEntity = await _context.Reservations.FindAsync(entity.Id);

                if (existingEntity is null || existingEntity.IsDeleted)
                {
                    _logger.LogWarning("Reservation with Id {Id} not found for deletion.", entity.Id);
                    return OperationResult.Failure("Reservation not found for deletion.");
                }
                existingEntity.IsDeleted = true;
                existingEntity.DeletedBy = Environment.UserName;
                _context.Reservations.Update(existingEntity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting reservation entity.");
                operationResult = OperationResult.Failure("An error occurred deleting the reservation entity: " + ex.Message);
              
            }
            return operationResult;
        }
        //public async Task<OperationResult> ConfirmReservationAsync(int reservationId, string confirmedBy) //tampoco va en el API endpoint.
        //{
        //    OperationResult operationResult = new OperationResult();
        //    try
        //    {
        //        _logger.LogInformation("Reservation confirmation started with ID: {ReservationId}", reservationId);

        //        await _context.Database.ExecuteSqlRawAsync(
        //            "EXEC ConfirmReservation @ReservationId, @ConfirmedBy",
        //            new SqlParameter("@ReservationId", reservationId),
        //            new SqlParameter("@ConfirmedBy", confirmedBy)
        //        );

        //        operationResult = OperationResult.Success("Reservation confirmed successfully.");
        //        _logger.LogInformation("Reservation with ID: {ReservationId} confirmed successfully.", reservationId);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error confirming reservation with Id: {Id}", reservationId);
        //        operationResult = OperationResult.Failure("An error occurred confirming the reservation: " + ex.Message);
        //    }
        //    return operationResult;

        //}
        //public async Task<OperationResult> ExpireConfirmedReservationsAsync() // tampoco va en el API endpoint. 
        //{
        //    OperationResult operationResult = new OperationResult();
        //    try
        //    {
        //        _logger.LogInformation("Running SP: ExpireconfirmedReservations");

        //        await _context.Database.ExecuteSqlRawAsync("EXEC ExpireConfirmedReservations");

        //        operationResult = OperationResult.Success("Expired confirmed reservations updated successfully.");
        //        _logger.LogInformation("Expired confirmed reservations updated.");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error expiring confirmed reservations with Id: {Id}");
        //        operationResult = OperationResult.Failure("An error occurred expiring the confirmed reservations: " + ex.Message);
        //    }
        //    return operationResult;
        //}
    }
}
