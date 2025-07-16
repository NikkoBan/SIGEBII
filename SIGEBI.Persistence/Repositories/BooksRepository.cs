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

        public async Task<OperationResult> GetBooksByCategoryAsync(int categoryId)
        {
            try
            {
                var books = await _dbSet.Where(b => b.CategoryId == categoryId).ToListAsync();
                var BookDTO = books.Select(b => b.ToDTO()).ToList();
                return new OperationResult { Success = true, Data = BookDTO };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error obteniendo libros por categoría {categoryId}.");
                return new OperationResult { Success = false, Message = ex.Message };
            }
        }

        public async Task<OperationResult> GetBooksByPublisherAsync(int publisherId)
        {
            try
            {
                var books = await _dbSet.Where(b => b.PublisherId == publisherId).ToListAsync();
                var BookDTO = books.Select(b => b.ToDTO()).ToList();
                return new OperationResult { Success = true, Data = BookDTO };
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error obteniendo libros por editorial {publisherId}.");
                return new OperationResult { Success = false, Message = ex.Message };
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
                var BookDTO = books.Select(b => b.ToDTO()).ToList();
                return new OperationResult { Success = true, Data = BookDTO };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error buscando libros con término '{searchTerm}'.");
                return new OperationResult { Success = false, Message = ex.Message };
            }
        }

        public async Task<OperationResult> GetAvailableBooksAsync()
        {
            try
            {
                var books = await _dbSet.Where(b => !b.IsDeleted).ToListAsync();
                var BookDTO = books.Select(b => b.ToDTO()).ToList();
                return new OperationResult { Success = true, Data = BookDTO };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo libros disponibles.");
                return new OperationResult
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<bool> CheckDuplicateISBNAsync(string isbn, int? excludeBookId = null)
        {
            return await _dbSet.AnyAsync(b =>
                b.ISBN == isbn && (!excludeBookId.HasValue || b.Id != excludeBookId));
        }
    }

}


