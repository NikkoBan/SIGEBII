using Castle.Core.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Repository;
using SIGEBI.Persistence.Base;
using SIGEBI.Persistence.Context;
using SIGEBI.Persistence.Interfaces;
using System.Linq.Expressions;

namespace SIGEBI.Persistence.Repositories
{
    public class LoanRepository : BaseRepository<Loan, int>, ILoanRepository
    {
        private readonly SIGEBIDbContext _context;
        private readonly ILogger<LoanRepository> _logger;
        private readonly IConfiguration _configuration;

        public LoanRepository(SIGEBIDbContext context, ILogger<LoanRepository> logger, IConfiguration configuration)
            : base(context)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
        }

        public override async Task<bool> Exists(Expression<Func<Loan, bool>> filter)
        {
            return await _context.Loan.AnyAsync(filter);
        }

        public override async Task<List<Loan>> GetAllAsync()
        {
            return await _context.Loan
                .Where(l => !l.Borrado)
                .Include(l => l.Book)
                .Include(l => l.User)
                .AsNoTracking()
                .ToListAsync();
        }

        public override async Task<OperationResult> GetAllAsync(Expression<Func<Loan, bool>> filter)
        {
            var data = await _context.Loan
                .Where(filter)
                .Where(l => !l.Borrado)
                .Include(l => l.Book)
                .Include(l => l.User)
                .AsNoTracking()
                .ToListAsync();

            return new OperationResult
            {
                Success = true,
                Data = data
            };
        }

        public override async Task<Loan?> GetEntityByIdAsync(int id)
        {
            if (!RepoValidation.ValidarID(id))
                return null;

            return await _context.Loan
                .Include(l => l.Book)
                .Include(l => l.User)
                .FirstOrDefaultAsync(l => l.ID == id);
        }

        public async Task<Loan?> GetActiveLoanByIdAsync(int id)
        {
            return await _context.Loan
                .Where(l => l.ID == id && l.ReturnDate == null)
                .Include(l => l.Book)
                .Include(l => l.User)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task<List<Loan>> GetActiveLoansByUserAsync(int userId)
        {
            return await _context.Loan
                .Where(l => l.UserId == userId && l.ReturnDate == null && !l.Borrado)
                .Include(l => l.Book)
                .AsNoTracking()
                .ToListAsync();
        }

        public override async Task<OperationResult> SaveEntityAsync(Loan entity)
        {
            var result = new OperationResult();

            if (!RepoValidation.ValidarID(entity.UserId) || !RepoValidation.ValidarID(entity.BookId))
            {
                result.Success = false;
                result.Message = _configuration["ErrorLoanRepository:InvalidData"]!;
                return result;
            }

            try
            {
                entity.LoanDate = DateTime.Now;
                entity.LoanStatus = "En curso";

                _context.Loan.Add(entity);
                await _context.SaveChangesAsync();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = _configuration["ErrorLoanRepository:SaveEntityAsync"]!;
                _logger.LogError(result.Message, ex.ToString());
            }

            return result;
        }

        public override async Task<OperationResult> UpdateEntityAsync(Loan entity)
        {
            var result = new OperationResult();

            if (!RepoValidation.ValidarID(entity.Id) ||
                !RepoValidation.ValidarEntidad(entity.FechaModificacion!) ||
                !RepoValidation.ValidarID(entity.UsuarioMod))
            {
                result.Success = false;
                result.Message = _configuration["ErrorLoanRepository:InvalidData"]!;
                return result;
            }

            try
            {
                _context.Loan.Update(entity);
                await _context.SaveChangesAsync();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = _configuration["ErrorLoanRepository:UpdateEntityAsync"]!;
                result.Success = false;
                _logger.LogError(result.Message, ex.ToString());
            }

            return result;
        }

        public override async Task<OperationResult> RemoveEntityAsync(Loan entity)
        {
            var result = new OperationResult();

            if (!RepoValidation.ValidarID(entity.Id) ||
                !RepoValidation.ValidarID(entity.UsuarioMod) ||
                !RepoValidation.ValidarEntidad(entity.FechaModificacion!) ||
                !RepoValidation.ValidarID(entity.BorradoPorU) ||
                !RepoValidation.ValidarEntidad(entity.Borrado!))
            {
                result.Message = _configuration["ErrorLoanRepository:InvalidData"]!;
                result.Success = false;
                return result;
            }

            try
            {
                _context.Loan.Update(entity);
                await _context.SaveChangesAsync();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = _configuration["ErrorLoanRepository:RemoveEntity"]!;
                result.Success = false;
                _logger.LogError(result.Message, ex.ToString());
            }

            return result;
        }

        public async Task<OperationResult> RegisterReturnAsync(int loanId, DateTime returnDate, string observations)
        {
            var result = new OperationResult();

            var loan = await _context.Loan.FindAsync(loanId);
            if (loan == null || loan.ReturnDate != null)
            {
                result.Success = false;
                result.Message = "Préstamo no encontrado o ya devuelto.";
                return result;
            }

            loan.ReturnDate = returnDate;
            loan.LoanStatus = "Devuelto";

            try
            {
                _context.Loan.Update(loan);
                await _context.SaveChangesAsync();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = _configuration["ErrorLoanRepository:RegisterReturn"]!;
                result.Success = false;
                _logger.LogError(result.Message, ex.ToString());
            }

            return result;
        }

        public Task<List<Loan>> GetLoansByUser(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Loan>> GetOverdueLoans()
        {
            throw new NotImplementedException();
        }

        public Task<Loan?> GetActiveLoanByBook(int bookId)
        {
            throw new NotImplementedException();
        }
    }
}
