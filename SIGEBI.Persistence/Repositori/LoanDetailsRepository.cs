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
    public class LoanDetailsRepository : ILoanDetailsRepository
    {
        private readonly SIGEBIDbContext _context;

        public LoanDetailsRepository(SIGEBIDbContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<LoanDetail>> AddAsync(LoanDetail entity)
        {
            try
            {
                await _context.LoanDetails.AddAsync(entity);
                await _context.SaveChangesAsync();
                return OperationResult<LoanDetail>.Ok(entity, "LoanDetail creado exitosamente.");
            }
            catch (Exception ex)
            {
                return OperationResult<LoanDetail>.Fail($"Error al crear LoanDetail: {ex.Message}");
            }
        }

        public async Task<OperationResult<bool>> DeleteAsync(int id)
        {
            var entity = await _context.LoanDetails.FindAsync(id);
            if (entity == null)
                return OperationResult<bool>.Fail("LoanDetail no encontrado.");

            try
            {
                _context.LoanDetails.Remove(entity);
                await _context.SaveChangesAsync();
                return OperationResult<bool>.Ok(true, "LoanDetail eliminado correctamente.");
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.Fail($"Error al eliminar LoanDetail: {ex.Message}");
            }
        }

        public async Task<OperationResult<List<LoanDetail>>> GetAllAsync()
        {
            try
            {
                var list = await _context.LoanDetails.ToListAsync();
                return OperationResult<List<LoanDetail>>.Ok(list);
            }
            catch (Exception ex)
            {
                return OperationResult<List<LoanDetail>>.Fail($"Error al obtener LoanDetails: {ex.Message}");
            }
        }

        public async Task<OperationResult<LoanDetail>> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _context.LoanDetails.FindAsync(id);
                if (entity == null)
                    return OperationResult<LoanDetail>.Fail("LoanDetail no encontrado.");

                return OperationResult<LoanDetail>.Ok(entity);
            }
            catch (Exception ex)
            {
                return OperationResult<LoanDetail>.Fail($"Error al obtener LoanDetail: {ex.Message}");
            }
        }

        public async Task<OperationResult<bool>> UpdateAsync(LoanDetail entity)
        {
            try
            {
                _context.LoanDetails.Update(entity);
                await _context.SaveChangesAsync();
                return OperationResult<bool>.Ok(true, "LoanDetail actualizado correctamente.");
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.Fail($"Error al actualizar LoanDetail: {ex.Message}");
            }
        }
    }
}
