
using Microsoft.Extensions.Logging;
using SIGEBI.Application.Contracts.Service;
using SIGEBI.Application.Dtos.BookAuthorDTO;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities.Configuration;
using SIGEBI.Domain.IRepository;


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
                return exists
                    ? OperationResult.SuccessResult(true)
                    : OperationResult.SuccessResult(false);
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
                return await _bookAuthorRepository.GetAuthorsByBookAsync(bookId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error obteniendo autores para libro {bookId}");
                return OperationResult.FailureResult(ex.Message);
            }
        }

        public async Task<OperationResult> GetBooksByAuthorAsync(int authorId)
        {
            try
            {
                return await _bookAuthorRepository.GetBooksByAuthorAsync(authorId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error obteniendo libros para autor {authorId}");
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

        protected override BookAuthorDTO MapToDto(BookAuthor entity)
        {
            return new BookAuthorDTO
            {
                BookId = entity.BookId,
                AuthorId = entity.AuthorId
            };
        }

        protected override IEnumerable<BookAuthorDTO> MapToDtoList(IEnumerable<BookAuthor> entities)
        {
            var list = new List<BookAuthorDTO>();
            foreach (var entity in entities)
            {
                list.Add(MapToDto(entity));
            }
            return list;
        }

        protected override BookAuthor MapToEntity(CreateBookAuthorDTO dto)
        {
            return new BookAuthor
            {
                BookId = dto.BookId,
                AuthorId = dto.AuthorId,
               
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
