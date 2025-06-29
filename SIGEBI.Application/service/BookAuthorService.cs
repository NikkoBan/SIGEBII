
using Microsoft.Extensions.Logging;
using SIGEBI.Application.Contracts.Repository;
using SIGEBI.Application.Contracts.Service;
using SIGEBI.Application.Dtos;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities.Configuration;
using AutoMapper;

namespace SIGEBI.Application.Services
{
    public class BookAuthorService : BaseService<BooksAuthors, CreateBookAuthorDTO, UpdateBookAuthorDTO, BookAuthorDTO>, IBookAuthorService
    {
        private readonly IBookAuthorRepository _bookAuthorRepository;
        private readonly ILogger<BookAuthorService> _logger;
        public BookAuthorService(
            IBookAuthorRepository bookAuthorRepository,
            IMapper mapper,
            ILogger<BookAuthorService> logger)
            : base(bookAuthorRepository, mapper)
        {
            _bookAuthorRepository = bookAuthorRepository;
            _logger = logger;
        }

        public async Task<OperationResult> AddBookAuthorAsync(CreateBookAuthorDTO dto)
        {
            try
            {
              
                bool isDuplicate = await _bookAuthorRepository.CheckDuplicateBookAuthorCombinationAsync(dto.BookId, dto.AuthorId);
                if (isDuplicate)
                {
                    _logger.LogWarning($"Intento de añadir combinación Libro-Autor fallido: Combinación duplicada para Libro ID {dto.BookId}, Autor ID {dto.AuthorId}.");
                    return new OperationResult { Success = false, Message = "Esta combinación de Libro y Autor ya existe." };
                }

                var result = await base.CreateAsync(dto); 

                if (result.Success)
                {
                    _logger.LogInformation($"Combinación Libro-Autor {dto.BookId}-{dto.AuthorId} añadida exitosamente.");
                }
                else
                {
                    _logger.LogError($"Fallo al añadir combinación Libro-Autor {dto.BookId}-{dto.AuthorId}: {result.Message}");
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Excepción al intentar añadir combinación Libro-Autor {dto.BookId}-{dto.AuthorId}.");
                return new OperationResult { Success = false, Message = $"Error inesperado al añadir la combinación Libro-Autor: {ex.Message}" };
            }
        }
            public override async Task<OperationResult> DeleteAsync(int id)
            {
            
            _logger.LogWarning($"Advertencia: Llamada a DeleteAsync genérico para BooksAuthors con ID simple {id}. " +
                                "Considera implementar un método de eliminación por clave compuesta (BookId, AuthorId) si es necesario.");
            return await base.DeleteAsync(id); 
            }
    }
}

