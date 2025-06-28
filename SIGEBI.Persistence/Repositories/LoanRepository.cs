using Microsoft.EntityFrameworkCore;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities;
using SIGEBI.Persistence.Base;
using SIGEBI.Persistence.Context;
using SIGEBI.Persistence.Interfaces;
using System.Linq.Expressions;

namespace SIGEBI.Persistence.Repositories
{
    public class LoanRepository : BaseRepository<Loan, int>, ILoanRepository
    {
        private readonly SIGEBIDbContext _context;

        public LoanRepository(SIGEBIDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<Loan?> GetEntityByIdAsync(int id)
        {
            return await _context.Loans
                .Include(l => l.Book)
                .Include(l => l.User)
                .FirstOrDefaultAsync(l => l.ID == id);
        }

        public override async Task<bool> Exists(Expression<Func<Loan, bool>> filter)
        {
            return await _context.Loans.AnyAsync(filter);
        }

        public override async Task<List<Loan>> GetAllAsync()
        {
            return await _context.Loans
                .Include(l => l.Book)
                .Include(l => l.User)
                .OrderByDescending(l => l.LoanDate)
                .AsNoTracking()
                .ToListAsync();
        }

        public override async Task<OperationResult> SaveEntityAsync(Loan entity)
        {
            var result = new OperationResult();
            try
            {
                _context.Loans.Add(entity);
                await _context.SaveChangesAsync();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error guardando préstamo: {ex.Message}";
            }
            return result;
        }

        public override async Task<OperationResult> UpdateEntityAsync(Loan entity)
        {
            var result = new OperationResult();
            try
            {
                _context.Loans.Update(entity);
                await _context.SaveChangesAsync();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error actualizando préstamo: {ex.Message}";
            }
            return result;
        }

        public override async Task<OperationResult> RemoveEntityAsync(Loan entity)
        {
            var result = new OperationResult();
            try
            {
                _context.Loans.Remove(entity);
                await _context.SaveChangesAsync();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error eliminando préstamo: {ex.Message}";
            }
            return result;
        }

        public async Task<List<Loan>> GetLoansByUser(int userId)
        {
            return await _context.Loans
                .Where(l => l.UserId == userId)
                .Include(l => l.Book)
                .OrderByDescending(l => l.LoanDate)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Loan>> GetOverdueLoans()
        {
            return await _context.Loans
                .Where(l => l.DueDate < DateTime.Now && l.ReturnDate == null)
                .Include(l => l.User)
                .Include(l => l.Book)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Loan?> GetActiveLoanByBook(int bookId)
        {
            return await _context.Loans
                .Where(l => l.BookId == bookId && l.ReturnDate == null)
                .Include(l => l.User)
                .FirstOrDefaultAsync();
        }
    }
}
