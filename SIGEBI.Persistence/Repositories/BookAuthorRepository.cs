
using Microsoft.Extensions.Logging;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities.Configuration;

using SIGEBI.Persistence.Base;

using SIGEBI.Domain.IRepository;
using SIGEBI.Persistence.Context;
using Microsoft.EntityFrameworkCore;




namespace SIGEBI.Persistence.Repositories
{
    public class BookAuthorRepository : BaseRepositoryEf<BookAuthor>, IBookAuthorRepository
    {
        public BookAuthorRepository(SIGEBIContext context, ILogger<BookAuthorRepository> logger)
            : base(context, logger)
        {
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
                var authors = await _dbSet
                    .Where(ba => ba.BookId == bookId)
                    .Select(ba => ba.Author)  // propiedad Author, no Authors
                    .ToListAsync();

                return new OperationResult { Success = true, Data = authors };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error obteniendo autores para el libro {bookId}.");
                return new OperationResult { Success = false, Message = ex.Message };
            }
        }

        public async Task<OperationResult> GetBooksByAuthorAsync(int authorId)
        {
            try
            {
                var books = await _dbSet
                    .Where(ba => ba.AuthorId == authorId)
                    .Select(ba => ba.Book) // propiedad Book, no Books
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
    }
}