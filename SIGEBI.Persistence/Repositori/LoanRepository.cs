using Microsoft.EntityFrameworkCore;
using SIGEBI.Persistence.Interface;
using SIGEBI.Domain.Entities;
using SIGEBI.Persistence.Base;
using SIGEBI.Persistence.Context;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIGEBI.Persistence.Repositories
{
    public class LoanRepository : ILoanRepository
    {
        private readonly SIGEBIDbContext _context;

        public LoanRepository(SIGEBIDbContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<List<Loan>>> GetAllAsync()
        {
            try
            {
                var loans = await _context.Loans.AsNoTracking().ToListAsync();
                return OperationResult<List<Loan>>.Ok(loans);
            }
            catch (Exception ex)
            {
                return OperationResult<List<Loan>>.Fail($"Error retrieving loans: {ex.Message}");
            }
        }

        public async Task<OperationResult<Loan>> GetByIdAsync(int id)
        {
            try
            {
                var loan = await _context.Loans.FindAsync(id);
                if (loan == null)
                    return OperationResult<Loan>.Fail("Loan not found.");

                return OperationResult<Loan>.Ok(loan);
            }
            catch (Exception ex)
            {
                return OperationResult<Loan>.Fail($"Error retrieving loan: {ex.Message}");
            }
        }

        public async Task<OperationResult<Loan>> AddAsync(Loan entity)
        {
            try
            {
                await _context.Loans.AddAsync(entity);
                await _context.SaveChangesAsync();
                return OperationResult<Loan>.Ok(entity, "Loan added successfully.");
            }
            catch (Exception ex)
            {
                return OperationResult<Loan>.Fail($"Error adding loan: {ex.Message}");
            }
        }

        public async Task<OperationResult<bool>> UpdateAsync(Loan entity)
        {
            try
            {
                _context.Loans.Update(entity);
                await _context.SaveChangesAsync();
                return OperationResult<bool>.Ok(true, "Loan updated successfully.");
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.Fail($"Error updating loan: {ex.Message}");
            }
        }

        public async Task<OperationResult<bool>> DeleteAsync(int id)
        {
            try
            {
                var loan = await _context.Loans.FindAsync(id);
                if (loan == null)
                    return OperationResult<bool>.Fail("Loan not found.");

                _context.Loans.Remove(loan);
                await _context.SaveChangesAsync();
                return OperationResult<bool>.Ok(true, "Loan deleted successfully.");
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.Fail($"Error deleting loan: {ex.Message}");
            }
        }
    }
}
