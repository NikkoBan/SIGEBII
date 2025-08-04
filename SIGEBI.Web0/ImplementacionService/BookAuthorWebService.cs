using Microsoft.AspNetCore.Mvc.Rendering;
using SIGEBI.Web0.Interfaz.BookAuthor;
using SIGEBI.Web0.Models.BookAuthor;
using SIGEBI.Web0.Interfaces.Book;
using SIGEBI.Web0.Interfaz.Author;
using SIGEBI.Application.Services;

namespace SIGEBI.Web0.Services.BookAuthor
{
    public class BookAuthorWebService : IBookAuthorWebService
    {
        private readonly IBookAuthorWeb _bookAuthorWebRepository;
        private readonly IBookWeb _bookWebRepository;
        private readonly IAuthorWeb _authorWebRepository;
        private readonly ILogger<BookAuthorService> _logger;

        public BookAuthorWebService(
            IBookAuthorWeb bookAuthorWebRepository,
            IBookWeb bookWebRepository,
            IAuthorWeb authorWebRepository,
            ILogger<BookAuthorService> logger)
        {
            _bookAuthorWebRepository = bookAuthorWebRepository;
            _bookWebRepository = bookWebRepository;
            _authorWebRepository = authorWebRepository;
            _logger = logger;
        }

        public async Task<List<BookAuthorModel>> GetAllRelationshipsAsync()
        {
            try
            {
                var relationships = await _bookAuthorWebRepository.GetAllAsync();
                return relationships ?? new List<BookAuthorModel>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las relaciones Libro-Autor.");
                throw;
            }
        }

        public async Task<BookAuthorModel?> GetRelationshipByIdsAsync(int bookId, int authorId)
        {
            try
            {
                return await _bookAuthorWebRepository.GetByIdsAsync(bookId, authorId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la relación Libro-Autor: LibroId {BookId}, AuthorId {AuthorId}", bookId, authorId);
                throw;
            }
        }

        public async Task<bool> CreateRelationshipAsync(CreateBookAuthorModel model)
        {
            try
            {
                return await _bookAuthorWebRepository.CreateAsync(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la relación Libro-Autor.");
                throw;
            }
        }

        public async Task<bool> DeleteRelationshipAsync(int bookId, int authorId)
        {
            try
            {
                return await _bookAuthorWebRepository.DeleteAsync(bookId, authorId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la relación Libro-Autor: LibroId {BookId}, AuthorId {AuthorId}", bookId, authorId);
                throw;
            }
        }

        public async Task<DeleteBookAuthorModel?> GetDeleteModelAsync(int bookId, int authorId)
        {
            try
            {
                return await _bookAuthorWebRepository.GetDeleteModelAsync(bookId, authorId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el modelo de eliminación para BookId: {BookId}, AuthorId: {AuthorId}", bookId, authorId);
                throw;
            }
        }

        public async Task PopulateDropdownsForBookAuthorForm(dynamic viewBag)
        {
            try
            {
                var booksResult = await _bookWebRepository.GetAllAsync();
                viewBag.BookList = booksResult != null
                    ? new SelectList(booksResult, "BookId", "Title")
                    : new SelectList(new List<SelectListItem>());

                if (booksResult == null)
                {
                    viewBag.WarningMessageBooks = "No se pudieron cargar los libros.";
                }

                var authorsResult = await _authorWebRepository.GetAllAsync();
                viewBag.AuthorList = authorsResult != null
                    ? new SelectList(authorsResult.Select(a => new SelectListItem
                    {
                        Value = a.AuthorId.ToString(),
                        Text = $"{a.FirstName} {a.LastName}"
                    }).ToList(), "Value", "Text")
                    : new SelectList(new List<SelectListItem>());

                if (authorsResult == null)
                {
                    viewBag.WarningMessageAuthors = "No se pudieron cargar los autores.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar los dropdowns para el formulario de Libro-Autor.");
                viewBag.BookList = new SelectList(new List<SelectListItem>());
                viewBag.AuthorList = new SelectList(new List<SelectListItem>());
                viewBag.ErrorMessage = $"Error al cargar los dropdowns: {ex.Message}";
            }
        }
    }
}
