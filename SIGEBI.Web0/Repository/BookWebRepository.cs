using SIGEBI.Web0.Models.Book;

using SIGEBI.Web0.Interfaces.Book;
using Microsoft.AspNetCore.Mvc.Rendering;
using SIGEBI.Application.Dtos.BooksDtos;
using SIGEBI.Web0.Models;
namespace SIGEBI.Web0.Repositories.BookRepository
{
    public class BookWebRepository : BaseWebRepository<BookModel, CreateBookModel, EditBookModel>, IBookWeb
    {
        public BookWebRepository(HttpClient httpClient, ILogger<BookWebRepository> logger)
            : base(httpClient, logger, "api/Book")
        {
        }

        public override async Task<EditBookModel?> GetEditModelByIdAsync(int id)
        {
            var bookModel = await GetBookWithRelationshipsByIdAsync(id);
            if (bookModel == null)
            {
                return null;
            }

            return new EditBookModel
            {
                BookId = bookModel.BookId,
                Title = bookModel.Title,
                ISBN = bookModel.ISBN,
                PublicationDate = bookModel.PublicationDate,
                CategoryId = bookModel.CategoryId,
                PublisherId = bookModel.PublisherId,
                Language = bookModel.Language,
                Summary = bookModel.Summary,
                TotalCopies = bookModel.TotalCopies,
                AuthorIds = bookModel.AuthorIds,
                GeneralStatus = bookModel.GeneralStatus,
                
            };
        }

        public async Task<BookModel?> GetBookWithRelationshipsByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/Book/{id}");

                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<BookModel>>();

                if (apiResponse != null && apiResponse.success)
                {
                    return apiResponse.Data;
                }

                _logger.LogWarning("API retornó una respuesta de error o sin datos para el libro. Status: {StatusCode}", response.StatusCode);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener libro con relaciones por ID desde el WebRepository.");
                return null;
            }
        }

       
        public async Task<BookDTO?> GetBookDtoByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/Book/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<BookDTO>>();

                return apiResponse?.Data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener DTO del libro por ID desde el WebRepository.");
                return null;
            }
        }

        public Task<IEnumerable<SelectListItem>> GetAuthorsAsSelectListAsync()
        {
            return Task.FromResult<IEnumerable<SelectListItem>>(new List<SelectListItem>());
        }

        public Task<IEnumerable<SelectListItem>> GetCategoriesAsSelectListAsync()
        {
            return Task.FromResult<IEnumerable<SelectListItem>>(new List<SelectListItem>());
        }

        public Task<IEnumerable<SelectListItem>> GetPublishersAsSelectListAsync()
        {
            return Task.FromResult<IEnumerable<SelectListItem>>(new List<SelectListItem>());
        }
    }
}
