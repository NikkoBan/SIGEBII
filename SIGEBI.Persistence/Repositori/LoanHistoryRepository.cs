using Microsoft.EntityFrameworkCore;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Validations;
using SIGEBI.Persistence.Base;
using SIGEBI.Persistence.Interface;
using SIGEBI.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIGEBI.Persistence.Repositories
{
    public class LoanHistoryRepository : ILoanHistoryRepository
    {
        private readonly SIGEBIDbContext _context;

        public LoanHistoryRepository(SIGEBIDbContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<LoanHistory>> AddAsync(LoanHistory entity)
        {
            var validationErrors = LoanHistoryValidation.ValidateLoanHistory(entity);
            if (validationErrors.Count > 0)
            {
                return OperationResult<LoanHistory>.Fail(validationErrors, "Validación fallida.");
            }

            try
            {
                await _context.LoanHistories.AddAsync(entity);
                await _context.SaveChangesAsync();

                return OperationResult<LoanHistory>.Ok(entity, "LoanHistory creado exitosamente.");
            }
            catch (Exception ex)
            {
                return OperationResult<LoanHistory>.Fail($"Error al crear LoanHistory: {ex.Message}");
            }
        }

        public async Task<OperationResult<bool>> DeleteAsync(int id)
        {
            var entity = await _context.LoanHistories.FindAsync(id);
            if (entity == null)
            {
                return OperationResult<bool>.Fail("LoanHistory no encontrado.");
            }

            try
            {
                _context.LoanHistories.Remove(entity);
                await _context.SaveChangesAsync();

                return OperationResult<bool>.Ok(true, "LoanHistory eliminado correctamente.");
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.Fail($"Error al eliminar LoanHistory: {ex.Message}");
            }
        }

        public async Task<OperationResult<List<LoanHistory>>> GetAllAsync()
        {
            try
            {
                var list = await _context.LoanHistories.ToListAsync();
                return OperationResult<List<LoanHistory>>.Ok(list);
            }
            catch (Exception ex)
            {
                return OperationResult<List<LoanHistory>>.Fail($"Error al obtener LoanHistories: {ex.Message}");
            }
        }

        public async Task<OperationResult<LoanHistory>> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _context.LoanHistories.FindAsync(id);
                if (entity == null)
                    return OperationResult<LoanHistory>.Fail("LoanHistory no encontrado.");

                return OperationResult<LoanHistory>.Ok(entity);
            }
            catch (Exception ex)
            {
                return OperationResult<LoanHistory>.Fail($"Error al obtener LoanHistory: {ex.Message}");
            }
        }

        public async Task<OperationResult<bool>> UpdateAsync(LoanHistory entity)
        {
            var validationErrors = LoanHistoryValidation.ValidateLoanHistory(entity);
            if (validationErrors.Count > 0)
            {
                return OperationResult<bool>.Fail(validationErrors, "Validación fallida.");
            }

            try
            {
                _context.LoanHistories.Update(entity);
                await _context.SaveChangesAsync();

                return OperationResult<bool>.Ok(true, "LoanHistory actualizado correctamente.");
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.Fail($"Error al actualizar LoanHistory: {ex.Message}");
            }
        }
    }
}
