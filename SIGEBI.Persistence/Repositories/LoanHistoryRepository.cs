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
    public class LoanHistoryRepository : BaseRepository<LoanHistory, int>, ILoanHistoryRepository
    {
        private readonly SIGEBIDbContext _context;
        private readonly ILogger<LoanHistoryRepository> _logger;
        private readonly IConfiguration _configuration;

        public LoanHistoryRepository(SIGEBIDbContext context, ILogger<LoanHistoryRepository> logger, IConfiguration configuration)
            : base(context)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
        }

        public override async Task<bool> Exists(Expression<Func<LoanHistory, bool>> filter)
        {
            return await _context.LoanHistory.AnyAsync(filter);
        }

        public override async Task<List<LoanHistory>> GetAllAsync()
        {
            return await _context.LoanHistory
                                 .Where(l => !l.Borrado)
                                 .AsNoTracking()
                                 .ToListAsync();
        }

        public override async Task<OperationResult> GetAllAsync(Expression<Func<LoanHistory, bool>> filter)
        {
            var list = await _context.LoanHistory
                                     .Where(filter)
                                     .Where(l => !l.Borrado)
                                     .AsNoTracking()
                                     .ToListAsync();

            return new OperationResult
            {
                Success = true,
                Data = list
            };
        }

        public override async Task<LoanHistory?> GetEntityByIdAsync(int id)
        {
            if (!RepoValidation.ValidarID(id))
                return null;

            return await _context.LoanHistory.FindAsync(id);
        }

        public async Task<List<LoanHistory>> GetLoanHistoryByUserAsync(int userId)
        {
            return await _context.LoanHistory
                                 .Where(h => h.UserId == userId && !h.Borrado)
                                 .AsNoTracking()
                                 .ToListAsync();
        }

        public async Task<List<LoanHistory>> GetOverdueReturnsAsync()
        {
            return await _context.LoanHistory
                                 .Where(h => h.ReturnDate > h.DueDate && !h.Borrado)
                                 .AsNoTracking()
                                 .ToListAsync();
        }

        public override async Task<OperationResult> SaveEntityAsync(LoanHistory entity)
        {
            var result = new OperationResult();

            if (!RepoValidation.ValidarID(entity.BookId) || !RepoValidation.ValidarID(entity.UserId))
            {
                result.Success = false;
                result.Message = _configuration["ErrorLoanHistoryRepository:InvalidData"]!;
                return result;
            }

            try
            {
                _context.LoanHistory.Add(entity);
                await _context.SaveChangesAsync();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = _configuration["ErrorLoanHistoryRepository:SaveEntityAsync"]!;
                _logger.LogError(result.Message, ex.ToString());
            }

            return result;
        }

        public override async Task<OperationResult> UpdateEntityAsync(LoanHistory entity)
        {
            var result = new OperationResult();

            if (!RepoValidation.ValidarID(entity.Id) || !RepoValidation.ValidarEntidad(entity.FechaModificacion!) || !RepoValidation.ValidarID(entity.UsuarioMod))
            {
                result.Success = false;
                result.Message = _configuration["ErrorLoanHistoryRepository:InvalidData"]!;
                return result;
            }

            try
            {
                _context.LoanHistory.Update(entity);
                await _context.SaveChangesAsync();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = _configuration["ErrorLoanHistoryRepository:UpdateEntityAsync"]!;
                result.Success = false;
                _logger.LogError(result.Message, ex.ToString());
            }

            return result;
        }

        public override async Task<OperationResult> RemoveEntityAsync(LoanHistory entity)
        {
            var result = new OperationResult();

            if (!RepoValidation.ValidarID(entity.Id) ||
                !RepoValidation.ValidarID(entity.UsuarioMod) ||
                !RepoValidation.ValidarEntidad(entity.FechaModificacion!) ||
                !RepoValidation.ValidarID(entity.BorradoPorU) ||
                !RepoValidation.ValidarEntidad(entity.Borrado!))
            {
                result.Success = false;
                result.Message = _configuration["ErrorLoanHistoryRepository:InvalidData"]!;
                return result;
            }

            try
            {
                _context.LoanHistory.Update(entity);
                await _context.SaveChangesAsync();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = _configuration["ErrorLoanHistoryRepository:RemoveEntity"]!;
                result.Success = false;
                _logger.LogError(result.Message, ex.ToString());
            }

            return result;
        }

        public Task<List<LoanHistory>> GetHistoryByUser(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<LoanHistory>> GetHistoryByBook(int bookId)
        {
            throw new NotImplementedException();
        }
    }
}
