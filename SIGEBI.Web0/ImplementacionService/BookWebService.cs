using Microsoft.AspNetCore.Mvc.Rendering;
using SIGEBI.Web0.Interfaces.Book;
using SIGEBI.Web0.Models.Book;
using SIGEBI.Web0.Interfaz.Author;


namespace SIGEBI.Web0.Services.Book
{
    public class BookWebService : BaseWebService<BookModel, CreateBookModel, EditBookModel, IBookWeb>, IBookWebService
    {
        private readonly IAuthorWeb _authorWebRepository;

        public BookWebService(IBookWeb bookWebRepository, IAuthorWeb authorWebRepository, ILogger<BookWebService> logger)
            : base(bookWebRepository, logger)
        {
            _authorWebRepository = authorWebRepository;
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

        public async Task<EditBookModel?> GetEditBookModelByIdAsync(int id)
        {
            try
            {
                var bookDto = await _repository.GetBookDtoByIdAsync(id);
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
                  
                    AuthorIds = new List<int>()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener modelo edición libro {BookId}", id);
                return null;
            }
        }

        public async Task<BookModel?> GetBookDetailsAsync(int id)
        {
            return await _repository.GetBookWithRelationshipsByIdAsync(id);
        }
    }
}
