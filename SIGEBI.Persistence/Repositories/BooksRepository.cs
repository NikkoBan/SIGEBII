using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities.Configuration;

using SIGEBI.Persistence.Context;
using SIGEBI.Persistence.Base;

using SIGEBI.Domain.IRepository;
using SIGEBI.Application.Dtos.BooksDtos;
using SIGEBI.Application.mappers;

namespace SIGEBI.Persistence.Repositories
{
    public class BookRepository : BaseRepositoryEf<Books>, IBookRepository
    {
        public BookRepository(SIGEBIContext context, ILogger<BookRepository> logger)
            : base(context, logger)
        {
        }

        public async Task<bool> CheckDuplicateBookTitleAsync(string title, int? excludeBookId = null)
        {
            return await _dbSet.AnyAsync(b =>
                b.Title == title && (!excludeBookId.HasValue || b.Id != excludeBookId));
        }

        public async Task<bool> CheckDuplicateISBNAsync(string isbn, int? excludeBookId = null)
        {
            return await _dbSet.AnyAsync(b =>
                b.ISBN == isbn && (!excludeBookId.HasValue || b.Id != excludeBookId));
        }

        public async Task<OperationResult> GetBooksByCategoryAsync(int categoryId)
        {
            try
            {
                var books = await _dbSet.Where(b => b.CategoryId == categoryId).ToListAsync();
                var dtos = books.Select(b => b.ToDTO()).ToList();
                return OperationResult.SuccessResult(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error obteniendo libros por categoría {categoryId}.");
                return OperationResult.FailureResult(ex.Message);
            }
        }

        public async Task<OperationResult> GetBooksByPublisherAsync(int publisherId)
        {
            try
            {
                var books = await _dbSet.Where(b => b.PublisherId == publisherId).ToListAsync();
                var dtos = books.Select(b => b.ToDTO()).ToList();
                return OperationResult.SuccessResult(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error obteniendo libros por editorial {publisherId}.");
                return OperationResult.FailureResult(ex.Message);
            }
        }

        public async Task<OperationResult> GetAllBooksWithRelationships()
        {
            try
            {
                var books = await _dbSet
                    .Include(x => x.Category)
                    .Include(x => x.Publisher)
                    .AsNoTracking()
                    .ToListAsync();

                var result = books.Select(book => new BookDTO
                {
                    BookId = book.Id,
                    Title = book.Title,
                    ISBN = book.ISBN,
                    PublicationDate = book.PublicationDate,
                    Language = book.Language,
                    Summary = book.Summary,
                    TotalCopies = book.TotalCopies,
                    AvailableCopies = book.AvailableCopies,
                    GeneralStatus = book.GeneralStatus,
                    CategoryName = book.Category?.CategoryName ?? "N/A",
                    PublisherName = book.Publisher?.PublisherName ?? "N/A",
                    CategoryId = book.CategoryId,
                    PublisherId = book.PublisherId,
                    IsAvailable = book.AvailableCopies > 0
                }).ToList();

                return OperationResult.SuccessResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los libros con relaciones.");
                return OperationResult.FailureResult("Error al obtener los libros.");
            }
        }

        public async Task<OperationResult> SearchBooksAsync(string searchTerm)
        {
            try
            {
                var books = await _dbSet
                    .Where(b =>
                        b.Title.Contains(searchTerm) ||
                        b.ISBN.Contains(searchTerm) ||
                        b.Summary.Contains(searchTerm))
                    .ToListAsync();
                var dtos = books.Select(b => b.ToDTO()).ToList();
                return OperationResult.SuccessResult(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error buscando libros con término '{searchTerm}'.");
                return OperationResult.FailureResult(ex.Message);
            }
        }

        public async Task<OperationResult> GetAvailableBooksAsync()
        {
            try
            {
                var books = await _dbSet.Where(b => !b.IsDeleted).ToListAsync();
                var dtos = books.Select(b => b.ToDTO()).ToList();
                return OperationResult.SuccessResult(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo libros disponibles.");
                return OperationResult.FailureResult(ex.Message);
            }
        }

        public async  Task<OperationResult?> GetBookWithRelationshipsByIdAsync(int id)
        {
            try
            {
                var book = await _dbSet
                    .Include(b => b.Category)
                    .Include(b => b.Publisher)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(b => b.Id == id);

                if (book == null)
                    return OperationResult.FailureResult("Libro no encontrado.");

                var dto = new BookDTO
                {
                    BookId = book.Id,
                    Title = book.Title,
                    ISBN = book.ISBN,
                    PublicationDate = book.PublicationDate,
                    Language = book.Language,
                    Summary = book.Summary,
                    TotalCopies = book.TotalCopies,
                    AvailableCopies = book.AvailableCopies,
                    GeneralStatus = book.GeneralStatus,
                    CategoryName = book.Category?.CategoryName ?? "N/A",
                    PublisherName = book.Publisher?.PublisherName ?? "N/A",
                    CategoryId = book.CategoryId,
                    PublisherId = book.PublisherId,
                    IsAvailable = book.AvailableCopies > 0
                };

                return OperationResult.SuccessResult(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener libro con relaciones por ID.");
                return OperationResult.FailureResult("Error interno al obtener el libro.");
            }
        }
    }
}
