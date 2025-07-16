



using Microsoft.Extensions.Logging;
using SIGEBI.Application.Contracts.Service;
using SIGEBI.Application.Dtos.BooksDtos;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities.Configuration;
using SIGEBI.Domain.IRepository;

namespace SIGEBI.Application.Services
{
    public class BookService : BaseService<BookDTO, CreateBookDTO, UpdateBookDto, Books>, IBookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(
            IBookRepository bookRepository,
            ILogger<BookService> logger)
            : base(bookRepository, logger)
        {
            _bookRepository = bookRepository;
        }

        public async Task<OperationResult> CheckDuplicateBookTitleAsync(string title, int? excludeBookId = null)
        {
            try
            {
                var exists = await _bookRepository.CheckDuplicateBookTitleAsync(title, excludeBookId);
                return OperationResult.SuccessResult(exists);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error verificando título duplicado del libro");
                return OperationResult.FailureResult($"Error interno: {ex.Message}");
            }
        }
        

        public async Task<OperationResult> CheckDuplicateISBNAsync(string isbn, int? excludeBookId = null)
        {
            try
            {
                var exists = await _bookRepository.CheckDuplicateISBNAsync(isbn, excludeBookId);
                return OperationResult.SuccessResult(exists);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error verificando ISBN duplicado");
                return OperationResult.FailureResult($"Error interno: {ex.Message}");
            }
        }

        public async Task<OperationResult> GetBooksByCategoryAsync(int categoryId)
        {
            try
            {
                return await _bookRepository.GetBooksByCategoryAsync(categoryId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener libros de la categoría {categoryId}.");
                return OperationResult.FailureResult(ex.Message);
            }
        }

        public async Task<OperationResult> GetBooksByPublisherAsync(int publisherId)
        {
            try
            {
                return await _bookRepository.GetBooksByPublisherAsync(publisherId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener libros del publicador {publisherId}.");
                return OperationResult.FailureResult(ex.Message);
            }
        }

        public async Task<OperationResult> SearchBooksAsync(string searchTerm)
        {
            try
            {
                return await _bookRepository.SearchBooksAsync(searchTerm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al buscar libros con término '{searchTerm}'.");
                return OperationResult.FailureResult(ex.Message);
            }
        }

        public async Task<OperationResult> GetAvailableBooksAsync()
        {
            try
            {
                return await _bookRepository.GetAvailableBooksAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener libros disponibles.");
                return OperationResult.FailureResult(ex.Message);
            }
        }
        public override async Task<OperationResult> CreateAsync(CreateBookDTO dto)
        {

            try
            {
                var entity = MapToEntity(dto);

                var created = await _bookRepository.CreateAsync(entity);

                if (!created.Success || created.Data is not Books bookEntity)
                    return OperationResult.FailureResult("Libro creado, pero no se pudo obtener el ID.");

                var dtoResult = MapToDto(bookEntity);

                return OperationResult.SuccessResult(dtoResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el libro.");
                return OperationResult.FailureResult("Ocurrió un error interno al crear el libro.");
            }
        }




        protected override BookDTO MapToDto(Books entity)
        {
            return new BookDTO
            {
                BookId = entity.Id,
                Title = entity.Title,
                ISBN = entity.ISBN,
                PublicationDate = entity.PublicationDate,
                Language = entity.Language,
                Summary = entity.Summary,
                TotalCopies = entity.TotalCopies,
                AvailableCopies = entity.AvailableCopies,
                GeneralStatus = entity.GeneralStatus,
               
                CategoryId = entity.CategoryId,
                PublisherId = entity.PublisherId,
                IsAvailable = entity.AvailableCopies > 0


            };
        }

        protected override IEnumerable<BookDTO> MapToDtoList(IEnumerable<Books> entities)
        {
            return entities.Select(MapToDto);
        }

        protected override Books MapToEntity(CreateBookDTO dto)
        {
            return new Books
            {
                Title = dto.Title,
                ISBN = dto.ISBN,
                PublicationDate = dto.PublicationDate,
                CategoryId = dto.CategoryId,
                PublisherId = dto.PublisherId,
                Language = dto.Language,
                Summary = dto.Summary,
                TotalCopies = dto.TotalCopies,
                AvailableCopies = dto.TotalCopies,
                GeneralStatus = "Disponible",


            };
        }

        protected override Books MapToEntity(UpdateBookDto dto, Books entity)
        {
            entity.Title = dto.Title;
            entity.ISBN = dto.ISBN;
            entity.PublicationDate = dto.PublicationDate;
            entity.Language = dto.Language;
            entity.Summary = dto.Summary;
            entity.TotalCopies = dto.TotalCopies;
            entity.AvailableCopies = dto.AvailableCopies;
            entity.GeneralStatus = dto.GeneralStatus;
            
            entity.CategoryId = dto.CategoryId;
            entity.PublisherId = dto.PublisherId;
            return entity;
        }
      


    }
}