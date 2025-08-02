using SIGEBI.Web0.Interfaz.BookAuthor;
using SIGEBI.Web0.Models.BookAuthor;
using SIGEBI.Web0.Models;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIGEBI.Web0.Repositories.BookAuthorRepository
{
    public class BookAuthorWebRepository : IBookAuthorWeb
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<BookAuthorWebRepository> _logger;
        private readonly string _baseApiPath = "api/AuthorBook";

        public BookAuthorWebRepository(HttpClient httpClient, ILogger<BookAuthorWebRepository> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

       
        public async Task<List<BookAuthorModel>?> GetAllAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseApiPath}/with-details");
                response.EnsureSuccessStatusCode();

                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<List<BookAuthorModel>>>();
                return apiResponse?.Data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las relaciones Libro-Autor con detalles.");
                throw new ApplicationException($"Error al obtener relaciones Libro-Autor: {ex.Message}");
            }
        }

        public async Task<bool> CreateAsync(CreateBookAuthorModel model)
        {
            try
            {
                var payload = new { model.BookId, model.AuthorId };
                var response = await _httpClient.PostAsJsonAsync(_baseApiPath, payload);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Error al crear relación Libro-Autor: {StatusCode} - {Content}", response.StatusCode, errorContent);
                    throw new ApplicationException($"Error al crear relación Libro-Autor: {errorContent}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al crear relación Libro-Autor.");
                throw new ApplicationException($"Error inesperado al crear relación Libro-Autor: {ex.Message}");
            }
        }

        public async Task<bool> DeleteAsync(int bookId, int authorId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_baseApiPath}/{bookId}/{authorId}");
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Error al eliminar relación Libro-Autor: {StatusCode} - {Content}", response.StatusCode, errorContent);
                    throw new ApplicationException($"Error al eliminar relación Libro-Autor: {errorContent}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al eliminar relación Libro-Autor.");
                throw new ApplicationException($"Error inesperado al eliminar relación Libro-Autor: {ex.Message}");
            }
        }

        public async Task<List<BookAuthorModel>?> GetAuthorsByBookAsync(int bookId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseApiPath}/authors-by-book/{bookId}");
                response.EnsureSuccessStatusCode();
                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<List<BookAuthorModel>>>();
                return apiResponse?.Data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener autores por libro ID {BookId}.", bookId);
                throw new ApplicationException($"Error al obtener autores por libro: {ex.Message}");
            }
        }

        public async Task<List<BookAuthorModel>?> GetBooksByAuthorAsync(int authorId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseApiPath}/books-by-author/{authorId}");
                response.EnsureSuccessStatusCode();
                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<List<BookAuthorModel>>>();
                return apiResponse?.Data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener libros por autor ID {AuthorId}.", authorId);
                throw new ApplicationException($"Error al obtener libros por autor: {ex.Message}");
            }
        }

        public async Task<BookAuthorModel?> GetByIdsAsync(int bookId, int authorId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseApiPath}/{bookId}/{authorId}");
                response.EnsureSuccessStatusCode();
                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<BookAuthorModel>>();
                return apiResponse?.Data;
            }
            catch (HttpRequestException httpEx) when (httpEx.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _logger.LogWarning("Relación Libro-Autor no encontrada para BookId: {BookId}, AuthorId: {AuthorId}", bookId, authorId);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener relación Libro-Autor por IDs: BookId {BookId}, AuthorId {AuthorId}.", bookId, authorId);
                throw new ApplicationException($"Error al obtener relación Libro-Autor: {ex.Message}");
            }
        }

        public async Task<DeleteBookAuthorModel?> GetDeleteModelAsync(int bookId, int authorId)
        {
            try
            {
                var bookAuthorModel = await GetByIdsAsync(bookId, authorId);

                if (bookAuthorModel != null)
                {
                    return new DeleteBookAuthorModel
                    {
                        BookId = bookAuthorModel.BookId,
                        AuthorId = bookAuthorModel.AuthorId,
                        BookTitle = bookAuthorModel.BookTitle,
                        AuthorName = bookAuthorModel.AuthorName
                    };
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener modelo de eliminación para BookId: {BookId}, AuthorId: {AuthorId}", bookId, authorId);
                throw new ApplicationException($"Error al preparar datos para eliminación: {ex.Message}");
            }
        }
    }
}
