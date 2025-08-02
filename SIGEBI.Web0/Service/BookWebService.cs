using Microsoft.AspNetCore.Mvc.Rendering;
using SIGEBI.Web0.Interfaces.Book;
using SIGEBI.Web0.Models.Book;
using SIGEBI.Web0.Interfaz.Author;
using SIGEBI.Application.Dtos.BooksDtos;

namespace SIGEBI.Web0.Services.Book
{
    public class BookWebService : IBookWebService
    {
        private readonly IBookWeb _bookWebRepository;
        private readonly IAuthorWeb _authorWebRepository;
        private readonly ILogger<BookWebService> _logger;

        public BookWebService(IBookWeb bookWebRepository, IAuthorWeb authorWebRepository, ILogger<BookWebService> logger)
        {
            _bookWebRepository = bookWebRepository;
            _authorWebRepository = authorWebRepository;
            _logger = logger;
        }

        public async Task<bool> CreateBookAsync(CreateBookModel model) =>
            await _bookWebRepository.CreateAsync(model);

        public async Task<bool> UpdateBookAsync(int id, EditBookModel model) =>
            await _bookWebRepository.UpdateAsync(id, model);

        public async Task<bool> DeleteBookAsync(int id) =>
            await _bookWebRepository.DeleteAsync(id);

        public async Task<IEnumerable<BookModel>> GetAllBooksAsync() =>
            await _bookWebRepository.GetAllAsync() ?? new List<BookModel>();

        public async Task<BookModel?> GetBookDetailsAsync(int id) =>
            await _bookWebRepository.GetBookWithRelationshipsByIdAsync(id);

        public async Task<EditBookModel?> GetEditBookModelByIdAsync(int id)
        {
            try
            {
                var bookDto = await _bookWebRepository.GetBookDtoByIdAsync(id);
                if (bookDto == null) return null;

                return new EditBookModel
                {
                    BookId = bookDto.BookId,
                    Title = bookDto.Title,
                    ISBN = bookDto.ISBN,
                    PublicationDate = bookDto.PublicationDate,
                    CategoryId = bookDto.CategoryId,
                    PublisherId = bookDto.PublisherId,
                    Language = bookDto.Language,
                    Summary = bookDto.Summary,
                    TotalCopies = bookDto.TotalCopies,
                    AvailableCopies = bookDto.AvailableCopies,
                    GeneralStatus = bookDto.GeneralStatus,
                    IsAvailable = bookDto.IsAvailable,
                    AuthorIds = new List<int>()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener modelo edición libro {BookId}", id);
                return null;
            }
        }

        public async Task PopulateDropdownsForBookForm(dynamic viewBag)
        {
            var authors = await _authorWebRepository.GetAllAsync();

            viewBag.AuthorList = authors != null && authors.Any()
                ? new MultiSelectList(authors, "AuthorId", "FirstName")
                : new MultiSelectList(new List<object>(), "AuthorId", "FirstName");

            if (authors == null || !authors.Any())
                viewBag.WarningMessageAuthors = "No se pudieron cargar los autores.";

            viewBag.GeneralStatusList = new SelectList(new List<string>
            {
        "Disponible", "Prestado", "En Mantenimiento", "Perdido"
             });
        }
    }
}
        

