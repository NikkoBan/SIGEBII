using Microsoft.Extensions.Logging;
using SIGEBI.Application.Contracts.Service;
using SIGEBI.Application.Dtos.BookAuthorDTO;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities.Configuration;
using SIGEBI.Domain.IRepository;
using System.Linq;

namespace SIGEBI.Application.Services
{
    public class BookAuthorService : BaseService<BookAuthorDTO, CreateBookAuthorDTO, UpdateBookAuthorDTO, BookAuthor>, IAuthorBookService
    {
        private readonly IBookAuthorRepository _bookAuthorRepository;

        public BookAuthorService(
            IBookAuthorRepository bookAuthorRepository,
            ILogger<BookAuthorService> logger)
            : base(bookAuthorRepository, logger)
        {
            _bookAuthorRepository = bookAuthorRepository;
        }

        public async Task<OperationResult> CheckDuplicateBookAuthorCombinationAsync(int bookId, int authorId)
        {
            try
            {
                bool exists = await _bookAuthorRepository.CheckDuplicateBookAuthorCombinationAsync(bookId, authorId);
                return OperationResult.SuccessResult(exists);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error verificando combinación libro {bookId} - autor {authorId}");
                return OperationResult.FailureResult(ex.Message);
            }
        }

        public async Task<OperationResult> DeleteByBookAndAuthorAsync(int bookId, int authorId)
        {
            try
            {
                return await _bookAuthorRepository.DeleteByBookAndAuthorAsync(bookId, authorId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error eliminando relación libro {bookId} - autor {authorId}");
                return OperationResult.FailureResult(ex.Message);
            }
        }

        public async Task<OperationResult> GetAuthorsByBookAsync(int bookId)
        {
            try
            {
                var result = await _bookAuthorRepository.GetAuthorsByBookAsync(bookId);
                return result.Success ? result : OperationResult.FailureResult("No se encontraron autores para este libro.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error obteniendo autores para el libro {bookId}");
                return OperationResult.FailureResult(ex.Message);
            }
        }

        public async Task<OperationResult> GetBooksByAuthorAsync(int authorId)
        {
            try
            {
                var result = await _bookAuthorRepository.GetBooksByAuthorAsync(authorId);
                return result.Success ? result : OperationResult.FailureResult("No se encontraron libros para este autor.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error obteniendo libros para el autor {authorId}");
                return OperationResult.FailureResult(ex.Message);
            }
        }

        public async Task<OperationResult> AddBookAuthorAsync(int bookId, int authorId)
        {
            try
            {
                return await _bookAuthorRepository.AddBookAuthorAsync(bookId, authorId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error creando relación libro {bookId} - autor {authorId}");
                return OperationResult.FailureResult(ex.Message);
            }
        }

        public async Task<OperationResult> GetByCompositeKeyAsync(int bookId, int authorId)
        {
            try
            {
                var bookAuthorEntity = await _bookAuthorRepository.GetByCompositeKeyAsync(bookId, authorId);
                if (bookAuthorEntity == null)
                    return OperationResult.FailureResult("Relación Libro-Autor no encontrada.");

                var bookAuthorDto = MapToDto(bookAuthorEntity);
                return OperationResult.SuccessResult(bookAuthorDto, "Relación Libro-Autor encontrada exitosamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener la relación Libro-Autor por IDs: {ex.Message}");
                return OperationResult.FailureResult($"Error al obtener la relación Libro-Autor: {ex.Message}");
            }
        }

        public async Task<OperationResult> GetAllBookAuthorsAsync()
        {
            return await _bookAuthorRepository.GetAllBookAuthorsAsync();
        }

        protected override BookAuthorDTO MapToDto(BookAuthor entity)
        {
            return new BookAuthorDTO
            {
                BookId = entity.BookId,
                AuthorId = entity.AuthorId,
                BookTitle = entity.Book?.Title,
                AuthorName = entity.Author != null ? $"{entity.Author.FirstName} {entity.Author.LastName}" : null
            };
        }

        protected override IEnumerable<BookAuthorDTO> MapToDtoList(IEnumerable<BookAuthor> entities)
        {
            return entities.Select(MapToDto);
        }

        protected override BookAuthor MapToEntity(CreateBookAuthorDTO dto)
        {
            return new BookAuthor
            {
                BookId = dto.BookId,
                AuthorId = dto.AuthorId
            };
        }

        protected override BookAuthor MapToEntity(UpdateBookAuthorDTO dto, BookAuthor entity)
        {
            entity.BookId = dto.BookId;
            entity.AuthorId = dto.AuthorId;
            return entity;
        }
    }
}
