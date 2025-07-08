﻿using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SIGEBI.Application.Contracts;
using SIGEBI.Persistence.Interfaces;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities.circulation;
using SIGEBI.Persistence.Context;
using SIGEBI.Application.DTOs;

namespace SIGEBI.Persistence.Repositories
{
    public class ReservationStatusesRepository : IReservationStatusesRepository
    {
        private readonly SIGEBIContext _context;
        private readonly IAppLogger<ReservationStatusesRepository> _logger;

        public ReservationStatusesRepository(SIGEBIContext context, IAppLogger<ReservationStatusesRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        private async Task<ReservationStatus?> FindReservationStatusAsync(int id)
        {
            var status = await _context.ReservationStatuses.FindAsync(id);
            if (status == null)
            {
                _logger.LogWarning("Reservation status with Id {Id} not found.", id);
            }
            return status;
        }

        public async Task<bool> ExistsAsync(Expression<Func<ReservationStatus, bool>> filter)
        {
            return await _context.ReservationStatuses.AnyAsync(filter);
        }

        public async Task<OperationResult> GetAllAsync(Expression<Func<ReservationStatus, bool>> filter)
        {
            OperationResult operationResult = new OperationResult();

            try
            {
                _logger.LogInformation("Retrieving all reservation statuses.");
<<<<<<< HEAD
                var statuses = await _context.Set<ReservationStatusViewDto>()
=======
                var statuses = await _context.Set<ReservationDto>()
>>>>>>> c9a5f6 (Fix: cambios en capa de persistencia)
                    .FromSqlRaw("EXEC GetReservationStatuses")
                    .ToListAsync();

                if (!statuses.Any())
                {
                    _logger.LogInformation("No reservation statuses found with the provided filter.");
                    operationResult = OperationResult.Failure("No reservation statuses found.");
                }
                else
                {

                    _logger.LogInformation("Retrieved reservation statuses succesfully.");
                    operationResult = OperationResult.Success("Reservation statuses retrieved successfully.", statuses);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving reservation statuses: {Message}", ex.Message);
                operationResult = OperationResult.Failure($"An error occurred retrieving the reservation statuses: {ex.Message}");
            }
            return operationResult;
        }

        public async Task<OperationResult> GetByIdAsync(int id)
        {
            OperationResult operationResult = new OperationResult();

            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Invalid reservation status Id: {Id}", id);
                    return OperationResult.Failure("Invalid reservation status Id.");
                }

                _logger.LogInformation("Retrieving reservation status by Id: {Id}", id);
                var status = await FindReservationStatusAsync(id);

                if (status == null)
                {
                    return OperationResult.Failure("Reservation status not found.");
                }

                _logger.LogInformation("Reservation status retrieved successfully: {@Status}", status);
                operationResult = OperationResult.Success("Reservation status retrieved successfully.", status);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving reservation status by Id: {Id}, Message: {Message}", id, ex.Message);
                 operationResult = OperationResult.Failure($"An error occurred retrieving the reservation status: {ex.Message}");
            }
            return operationResult;

        }
    }
}
