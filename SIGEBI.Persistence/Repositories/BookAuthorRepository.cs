using Microsoft.Extensions.Logging;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities.Configuration;
using SIGEBI.Persistence.Base;
using SIGEBI.Domain.IRepository;
using SIGEBI.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using SIGEBI.Application.Dtos.BookAuthorDTO;

namespace SIGEBI.Persistence.Repositories
{
    public class BookAuthorRepository : BaseRepositoryEf<BookAuthor>, IBookAuthorRepository
    {
        private readonly SIGEBIContext _context;

        public BookAuthorRepository(SIGEBIContext context, ILogger<BookAuthorRepository> logger)
            : base(context, logger)
        {
            _context = context;
        }

        public async Task<OperationResult> GetAllBookAuthorsAsync()
        {
            try
            {
                var bookAuthors = await _context.BookAuthor
                    .Include(ba => ba.Book)
                    .Include(ba => ba.Author)
                    .Select(ba => new BookAuthorDTO
                    {
                        BookId = ba.BookId,
                        AuthorId = ba.AuthorId,
                        BookTitle = ba.Book != null ? ba.Book.Title : null,
                        AuthorName = ba.Author != null ? $"{ba.Author.FirstName} {ba.Author.LastName}" : null
                    })
                    .ToListAsync();

                return new OperationResult { Success = true, Data = bookAuthors };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las relaciones libro-autor.");
                return new OperationResult { Success = false, Message = ex.Message };
            }
        }

        public async Task<bool> CheckDuplicateBookAuthorCombinationAsync(int bookId, int authorId)
        {
            return await _dbSet.AnyAsync(ba => ba.BookId == bookId && ba.AuthorId == authorId);
        }

        public async Task<OperationResult> DeleteByBookAndAuthorAsync(int bookId, int authorId)
        {
            try
            {
                var entity = await _dbSet.FirstOrDefaultAsync(ba => ba.BookId == bookId && ba.AuthorId == authorId);
                if (entity == null)
                    return new OperationResult { Success = false, Message = "La relación libro-autor no existe." };

                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();

                return new OperationResult { Success = true, Message = "Relación eliminada correctamente." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error eliminando relación libro {bookId} - autor {authorId}.");
                return new OperationResult { Success = false, Message = $"Error: {ex.Message}" };
            }
        }

        public async Task<OperationResult> GetAuthorsByBookAsync(int bookId)
        {
            try
            {
                var bookAuthors = await _dbSet
                    .Where(ba => ba.BookId == bookId)
                    .Include(ba => ba.Author)  
                    .Include(ba => ba.Book)   
                    .Select(ba => new BookAuthorDTO
                    {
                        BookId = ba.BookId,
                        AuthorId = ba.AuthorId,
                        BookTitle = ba.Book.Title,
                        AuthorName = $"{ba.Author.FirstName} {ba.Author.LastName}" 
                    })
                    .ToListAsync();

                return new OperationResult { Success = true, Data = bookAuthors };
            }
            catch (Exception ex)
            {
                return new OperationResult { Success = false, Message = ex.Message };
            }
        }

        public async Task<OperationResult> GetBooksByAuthorAsync(int authorId)
        {
            try
            {
                var books = await _dbSet
                    .Where(ba => ba.AuthorId == authorId)
                    .Include(ba => ba.Book)
                    .Include(ba => ba.Author)
                    .Select(ba => new BookAuthorDTO
                    {
                        BookId = ba.BookId,
                        AuthorId = ba.AuthorId,
                        BookTitle = ba.Book != null ? ba.Book.Title : null,
                        AuthorName = $"{ba.Author.FirstName} {ba.Author.LastName}"
                    })
                    .ToListAsync();

                return new OperationResult { Success = true, Data = books };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error obteniendo libros para el autor {authorId}.");
                return new OperationResult { Success = false, Message = ex.Message };
            }
        }

        public async Task<OperationResult> AddBookAuthorAsync(int bookId, int authorId)
        {
            try
            {
                bool exists = await _dbSet.AnyAsync(ba => ba.BookId == bookId && ba.AuthorId == authorId);
                if (exists)
                    return new OperationResult { Success = false, Message = "La relación ya existe." };

                var entity = new BookAuthor
                {
                    BookId = bookId,
                    AuthorId = authorId,
                };

                await _dbSet.AddAsync(entity);
                await _context.SaveChangesAsync();

                return new OperationResult { Success = true, Message = "Relación creada correctamente.", Data = entity };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error creando relación libro {bookId} - autor {authorId}.");
                return new OperationResult { Success = false, Message = ex.Message };
            }
        }

        public async Task<BookAuthor?> GetByCompositeKeyAsync(int bookId, int authorId)
        {
            try
            {
                return await _context.BookAuthor
                    .Include(ba => ba.Book)
                    .Include(ba => ba.Author)
                    .FirstOrDefaultAsync(ba => ba.BookId == bookId && ba.AuthorId == authorId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error obteniendo relación libro {bookId} - autor {authorId}.");
                return null;
            }
        }
    }
}
