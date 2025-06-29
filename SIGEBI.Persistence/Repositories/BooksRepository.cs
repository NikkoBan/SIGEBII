using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SIGEBI.Application.Contracts.Repository;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities.Configuration;
using SIGEBI.Persistence.Base.SIGEBI.Persistence.Repositories;
using SIGEBI.Persistence.Context;

namespace SIGEBI.Persistence.Repositories
{
    public class BookRepository : BaseRepositoryEf<Books>, IBookRepository
    {
        public BookRepository(SIGEBIContext context, ILogger<BaseRepositoryEf<Books>> logger)
            : base(context, logger) { }

        public async Task<bool> CheckDuplicateBookTitleAsync(string title, int? excludeBookId = null)
        {
            if (excludeBookId.HasValue)
            {
                return await _dbSet.AnyAsync(b => b.Title == title && b.Id != excludeBookId.Value);
            }
            return await _dbSet.AnyAsync(b => b.Title == title);
        }

        public async Task<OperationResult> GetBooksByAuthorIdAsync(int authorId)
        {
            try
            {
                var books = await _dbSet
                                .Include(b => b.BooksAuthors) 
                                .ThenInclude(ba => ba.Authors) 
                                .Where(b => b.BooksAuthors.Any(ba => ba.AuthorId == authorId))
                                .ToListAsync();

                if (!books.Any())
                {
                    return new OperationResult { Success = false, Message = $"No se encontraron libros para el autor con ID {authorId}." };
                }
                return new OperationResult { Success = true, Data = books };
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener libros por AuthorId {authorId}.");
                return new OperationResult { Success = false, Message = $"Error al obtener libros por autor: {ex.Message}" };
            }
        }



      public override async Task<OperationResult> GetByIdAsync(int id)
        {
            try
            {
                var book = await _dbSet
                                .Include(b => b.BooksAuthors)
                                .ThenInclude(ba => ba.Authors)
                                .FirstOrDefaultAsync(b => b.Id == id); 

                if (book == null)
                {
                    return new OperationResult { Success = false, Message = $"Libro con ID {id} no encontrado." };
                }
                return new OperationResult { Success = true, Data = book };
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener libro con ID {id} con detalles.");
                return new OperationResult { Success = false, Message = $"Error al obtener libro: {ex.Message}" };
            }
      }
    }
}




