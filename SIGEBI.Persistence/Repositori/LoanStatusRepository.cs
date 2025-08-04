using Microsoft.EntityFrameworkCore;
using SIGEBI.Domain.Entities;
using SIGEBI.Persistence.Base;
using SIGEBI.Persistence.Context;
using SIGEBI.Persistence.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIGEBI.Persistence.Repositories
{
    public class LoanStatusRepository : ILoanStatusRepository
    {
        private readonly SIGEBIDbContext _context;

        public LoanStatusRepository(SIGEBIDbContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<LoanStatus>> AddAsync(LoanStatus entity)
        {
            try
            {
                await _context.LoanStatuses.AddAsync(entity);
                await _context.SaveChangesAsync();
                return OperationResult<LoanStatus>.Ok(entity, "LoanStatus creado exitosamente.");
            }
            catch (Exception ex)
            {
                return OperationResult<LoanStatus>.Fail($"Error al crear LoanStatus: {ex.Message}");
            }
        }

        public async Task<OperationResult<bool>> DeleteAsync(int id)
        {
            var entity = await _context.LoanStatuses.FindAsync(id);
            if (entity == null)
                return OperationResult<bool>.Fail("LoanStatus no encontrado.");

            try
            {
                _context.LoanStatuses.Remove(entity);
                await _context.SaveChangesAsync();
                return OperationResult<bool>.Ok(true, "LoanStatus eliminado correctamente.");
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.Fail($"Error al eliminar LoanStatus: {ex.Message}");
            }
        }

        public async Task<OperationResult<List<LoanStatus>>> GetAllAsync()
        {
            try
            {
                var list = await _context.LoanStatuses.ToListAsync();
                return OperationResult<List<LoanStatus>>.Ok(list);
            }
            catch (Exception ex)
            {
                return OperationResult<List<LoanStatus>>.Fail($"Error al obtener LoanStatuses: {ex.Message}");
            }
        }

        public async Task<OperationResult<LoanStatus>> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _context.LoanStatuses.FindAsync(id);
                if (entity == null)
                    return OperationResult<LoanStatus>.Fail("LoanStatus no encontrado.");

                return OperationResult<LoanStatus>.Ok(entity);
            }
            catch (Exception ex)
            {
                return OperationResult<LoanStatus>.Fail($"Error al obtener LoanStatus: {ex.Message}");
            }
        }

        public async Task<OperationResult<bool>> UpdateAsync(LoanStatus entity)
        {
            try
            {
                _context.LoanStatuses.Update(entity);
                await _context.SaveChangesAsync();
                return OperationResult<bool>.Ok(true, "LoanStatus actualizado correctamente.");
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.Fail($"Error al actualizar LoanStatus: {ex.Message}");
            }
        }
    }
}
