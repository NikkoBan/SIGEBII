using SIGEBI.Web0.Interfaz;
using SIGEBI.Web0.Models.Book;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Json;
using SIGEBI.Web0.Interfaces.Book;
using Microsoft.AspNetCore.Mvc.Rendering;
using SIGEBI.Application.Dtos.BooksDtos;
using Newtonsoft.Json;

namespace SIGEBI.Web0.Repositories.BookRepository
{
    public class BookWebRepository : BaseWebRepository<BookModel, CreateBookModel, EditBookModel>, IBookWeb
    {
        public BookWebRepository(HttpClient httpClient, ILogger<BookWebRepository> logger)
            : base(httpClient, logger, "api/Book")
        {
        }

        public async Task<BookModel?> GetBookWithRelationshipsByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/Book/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<BookModel>();
                }
                _logger.LogWarning("No se pudo obtener el libro con relaciones. Status: {StatusCode}", response.StatusCode);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener libro con relaciones por ID desde el WebRepository.");
                return null;
            }
        }

        // Nuevo método implementado de la interfaz
        public async Task<BookDTO?> GetBookDtoByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/Book/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<BookDTO>(json);
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
