using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities;
using SIGEBI.Persistence.Base;
using SIGEBI.Persistence.Context;
using SIGEBI.Persistence.Interfaces;
using System.Linq.Expressions;

namespace SIGEBI.Persistence.Repositories
{
    public class BookRepository : BaseRepository<Book, int>, IBookRepository
    {
        private readonly SIGEBIDbContext _context;
        private readonly ILogger<BookRepository> _logger;
        private readonly IConfiguration _configuration;

        public BookRepository(SIGEBIDbContext context, ILogger<BookRepository> logger, IConfiguration configuration)
            : base(context)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
        }

        public override async Task<bool> Exists(Expression<Func<Book, bool>> filter)
        {
            return await _context.Books.AnyAsync(filter);
        }

        public override async Task<List<Book>> GetAllAsync()
        {
            return await _context.Books
                                 .Where(b => !b.Borrado)
                                 .AsNoTracking()
                                 .ToListAsync()
                                 .ConfigureAwait(false);
        }

        public override async Task<OperationResult> GetAllAsync(Expression<Func<Book, bool>> filter)
        {
            var result = new OperationResult();
            result.Data = await _context.Books
                                        .Where(filter)
                                        .AsNoTracking()
                                        .ToListAsync()
                                        .ConfigureAwait(false);
            result.Success = true;
            return result;
        }

        public override async Task<Book?> GetEntityByIdAsync(int id)
        {
            if (!RepoValidation.ValidarID(id))
                return null;

            return await _context.Books.FindAsync(id);
        }

        public override async Task<OperationResult> SaveEntityAsync(Book entity)
        {
            var result = new OperationResult();
            if (!RepoValidation.ValidarLibro(entity))
            {
                result.Message = _configuration["ErrorBookRepository:InvalidData"]!;
                result.Success = false;
                return result;
            }

            try
            {
                await _context.Books.AddAsync(entity);
                await _context.SaveChangesAsync();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = _configuration["ErrorBookRepository:SaveEntityAsync"]!;
                result.Success = false;
                _logger.LogError(result.Message, ex.ToString());
            }

            return result;
        }

        public override async Task<OperationResult> UpdateEntityAsync(Book entity, object repoValidation)
        {
            var result = new OperationResult();
            if (!RepoValidation.ValidarLibro(entity))
            {
                result.Message = _configuration["ErrorBookRepository:InvalidData"]!;
                result.Success = false;
                return result;
            }

            try
            {
                _context.Books.Update(entity);
                await _context.SaveChangesAsync();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = _configuration["ErrorBookRepository:UpdateEntityAsync"]!;
                result.Success = false;
                _logger.LogError(result.Message, ex.ToString());
            }

            return result;
        }

        public override async Task<OperationResult> RemoveEntityAsync(Book entity)
        {
            var result = new OperationResult();
            try
            {
                entity.Borrado = true;
                _context.Books.Update(entity);
                await _context.SaveChangesAsync();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = _configuration["ErrorBookRepository:RemoveEntity"]!;
                result.Success = false;
                _logger.LogError(result.Message, ex.ToString());
            }

            return result;
        }

        public async Task<Book?> GetBookByISBN(string isbn)
        {
            return await _context.Books
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync(b => b.ISBN == isbn);
        }

        public async Task<List<Book>> GetBooksByCategory(int categoryId)
        {
            return await _context.Books
                                 .AsNoTracking()
                                 .Where(b => b.CategoryId == categoryId)
                                 .ToListAsync();
        }

        public async Task<List<Book>> GetAvailableBooks()
        {
            return await _context.Books
                                 .AsNoTracking()
                                 .Where(b => b.AvailableCopies > 0 && b.GeneralStatus == "Disponible")
                                 .ToListAsync();
        }
    }
}
