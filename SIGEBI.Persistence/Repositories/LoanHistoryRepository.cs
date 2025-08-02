using Microsoft.EntityFrameworkCore;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities;
using SIGEBI.Persistence.Base;
using SIGEBI.Persistence.Context;
using SIGEBI.Persistence.Interfaces;
using System.Linq.Expressions;

namespace SIGEBI.Persistence.Repositories
{
    public class LoanHistoryRepository : BaseRepository<LoanHistory, int>, ILoanHistoryRepository
    {
        private readonly SIGEBIDbContext _context;

        public LoanHistoryRepository(SIGEBIDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<LoanHistory?> GetEntityByIdAsync(int id)
        {
            return await _context.LoanHistories.FindAsync(id);
        }

        public override async Task<bool> Exists(Expression<Func<LoanHistory, bool>> filter)
        {
            return await _context.LoanHistories.AnyAsync(filter);
        }

        public override async Task<List<LoanHistory>> GetAllAsync()
        {
            return await _context.LoanHistories
                .OrderByDescending(h => h.LoanDate)
                .AsNoTracking()
                .ToListAsync();
        }

        public override async Task<OperationResult<LoanHistory>> SaveEntityAsync(LoanHistory entity)
        {
            try
            {
                _context.LoanHistories.Add(entity);
                await _context.SaveChangesAsync();
                return OperationResult<LoanHistory>.SuccessResult(entity, "Historial de préstamo guardado correctamente.");
            }
            catch (Exception ex)
            {
                return OperationResult<LoanHistory>.FailResult($"Error guardando historial de préstamo: {ex.Message}");
            }
        }

        public override async Task<OperationResult<LoanHistory>> UpdateEntityAsync(LoanHistory entity)
        {
            try
            {
                _context.LoanHistories.Update(entity);
                await _context.SaveChangesAsync();
                return OperationResult<LoanHistory>.SuccessResult(entity, "Historial de préstamo actualizado correctamente.");
            }
            catch (Exception ex)
            {
                return OperationResult<LoanHistory>.FailResult($"Error actualizando historial de préstamo: {ex.Message}");
            }
        }

        public override async Task<OperationResult<LoanHistory>> RemoveEntityAsync(LoanHistory entity)
        {
            try
            {
                _context.LoanHistories.Remove(entity);
                await _context.SaveChangesAsync();
                return OperationResult<LoanHistory>.SuccessResult(entity, "Historial de préstamo eliminado correctamente.");
            }
            catch (Exception ex)
            {
                return OperationResult<LoanHistory>.FailResult($"Error eliminando historial de préstamo: {ex.Message}");
            }
        }

        public async Task<List<LoanHistory>> GetHistoryByUser(int userId)
        {
            return await _context.LoanHistories
                .Where(h => h.UserId == userId)
                .OrderByDescending(h => h.LoanDate)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<LoanHistory>> GetHistoryByBook(int bookId)
        {
            return await _context.LoanHistories
                .Where(h => h.BookId == bookId)
                .OrderByDescending(h => h.LoanDate)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
