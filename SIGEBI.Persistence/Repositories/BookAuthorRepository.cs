using System.Linq.Expressions;
using Castle.Core.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities;
using SIGEBI.Persistence.Base;
using SIGEBI.Persistence.Context;
using SIGEBI.Persistence.Interfaces;

namespace SIGEBI.Persistence.Repositories
{
    public class BookAuthorRepository : BaseRepository<BookAuthor, int>, IBookAuthorRepository
    {
        private readonly SIGEBIDbContext _context;
        private readonly ILogger<BookAuthorRepository> _logger;
        private readonly IConfiguration _configuration;

        public BookAuthorRepository(SIGEBIDbContext context,
                                    ILogger<BookAuthorRepository> logger,
                                    IConfiguration configuration)
            : base(context)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
        }

        public override async Task<bool> Exists(Expression<Func<BookAuthor, bool>> filter)
        {
            return await _context.BookAuthors.AnyAsync(filter);
        }

        public override async Task<List<BookAuthor>> GetAllAsync()
        {
            return await _context.BookAuthors
                .Where(b => !b.Borrado)
                .AsNoTracking()
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public override async Task<OperationResult> GetAllAsync(Expression<Func<BookAuthor, bool>> filter)
        {
            var result = new OperationResult();
            result.Data = await _context.BookAuthors
                .Where(b => !b.Borrado)
                .Where(filter)
                .AsNoTracking()
                .ToListAsync();
            result.Success = true;
            return result;
        }

        public override async Task<BookAuthor?> GetEntityByIdAsync(int id)
        {
            if (!RepoValidation.ValidarID(id))
                return null;

            return await _context.BookAuthors.FindAsync(id);
        }

        public override async Task<OperationResult> SaveEntityAsync(BookAuthor entity)
        {
            var result = new OperationResult();

            if (!RepoValidation.ValidarID(entity.AuthorId) || !RepoValidation.ValidarEntidad(entity.FechaRegistro))
            {
                result.Message = _configuration["ErrorBookAuthorRepository:InvalidData"]!;
                result.Success = false;
                return result;
            }

            try
            {
                await _context.BookAuthors.AddAsync(entity);
                await _context.SaveChangesAsync();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = _configuration["ErrorBookAuthorRepository:SaveEntityAsync"]!;
                result.Success = false;
                _logger.LogError(result.Message, ex);
            }

            return result;
        }

        public override async Task<OperationResult> UpdateEntityAsync(BookAuthor entity)
        {
            var result = new OperationResult();

            if (!RepoValidation.ValidarID(entity.Id) || !RepoValidation.ValidarID(entity.AuthorId) || !RepoValidation.ValidarEntidad(entity.FechaModificacion!))
            {
                result.Message = _configuration["ErrorBookAuthorRepository:InvalidData"]!;
                result.Success = false;
                return result;
            }

            try
            {
                _context.BookAuthors.Update(entity);
                await _context.SaveChangesAsync();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = _configuration["ErrorBookAuthorRepository:UpdateEntityAsync"]!;
                result.Success = false;
                _logger.LogError(result.Message, ex);
            }

            return result;
        }

        public override async Task<OperationResult> RemoveEntityAsync(BookAuthor entity)
        {
            var result = new OperationResult();

            if (!RepoValidation.ValidarID(entity.Id) ||
                !RepoValidation.ValidarEntidad(entity.FechaModificacion!) ||
                !RepoValidation.ValidarID(entity.BorradoPorU) ||
                !RepoValidation.ValidarEntidad(entity.Borrado!))
            {
                result.Message = _configuration["ErrorBookAuthorRepository:InvalidData"]!;
                result.Success = false;
                return result;
            }

            try
            {
                entity.Borrado = true;
                _context.BookAuthors.Update(entity);
                await _context.SaveChangesAsync();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = _configuration["ErrorBookAuthorRepository:RemoveEntity"]!;
                result.Success = false;
                _logger.LogError(result.Message, ex);
            }

            return result;
        }

        public async Task<List<BookAuthor>> GetByBookIdAsync(int bookId)
        {
            return await _context.BookAuthors
                .Where(b => b.Book != null && b.Book.ID == bookId && !b.Borrado)
                .ToListAsync();
        }

        public async Task<List<BookAuthor>> GetByAuthorIdAsync(int authorId)
        {
            return await _context.BookAuthors
                .Where(b => b.Author != null && b.Author.ID == authorId && !b.Borrado)
                .ToListAsync();
        }

        Task<List<BookAuthor>> IBookAuthorRepository.GetByAuthorId(int authorId)
        {
            throw new NotImplementedException();
        }

        Task<List<BookAuthor>> IBookAuthorRepository.GetByBookId(int bookId)
        {
            throw new NotImplementedException();
        }
    }
}

