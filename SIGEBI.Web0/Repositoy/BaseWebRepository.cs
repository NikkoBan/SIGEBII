using SIGEBI.Web0.Interfaz;
using System.Net.Http;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using SIGEBI.Web0.Models;

namespace SIGEBI.Web0.Repositories
{

    public abstract class BaseWebRepository<TModel, TCreateModel, TEditModel> : IBaseWebRepository<TModel, TCreateModel, TEditModel>
        where TModel : class
        where TCreateModel : class
        where TEditModel : class
    {
        protected readonly HttpClient _httpClient;
        protected readonly ILogger _logger;
        protected readonly string _apiEndpoint;

        protected JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public BaseWebRepository(HttpClient httpClient, ILogger logger, string apiEndpoint)
        {
            _httpClient = httpClient;
            _logger = logger;
            _apiEndpoint = apiEndpoint;
        }

        public virtual async Task<List<TModel>> GetAllAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(_apiEndpoint);
                response.EnsureSuccessStatusCode();

                var responseString = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<List<TModel>>>(responseString, _jsonSerializerOptions);

                if (apiResponse != null && apiResponse.success)
                {
                    return apiResponse.Data ?? new List<TModel>();
                }
                else
                {
                    _logger.LogWarning("API returned success false or no data for GetAll. Endpoint: {Endpoint}, Message: {Message}", _apiEndpoint, apiResponse?.Message);
                    return new List<TModel>();
                }
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError(httpEx, "HTTP error during GetAll. Endpoint: {Endpoint}", _apiEndpoint);
                throw new ApplicationException($"Error de comunicación con la API al obtener {_apiEndpoint.ToLowerInvariant().Replace("api/", "")}s.", httpEx);
            }
            catch (JsonException jsonEx)
            {
                _logger.LogError(jsonEx, "JSON deserialization error during GetAll. Endpoint: {Endpoint}", _apiEndpoint);
                throw new ApplicationException($"Error al procesar la respuesta de la API al obtener {_apiEndpoint.ToLowerInvariant().Replace("api/", "")}s.", jsonEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred during GetAll. Endpoint: {Endpoint}", _apiEndpoint);
                throw new ApplicationException($"Ocurrió un error inesperado al obtener {_apiEndpoint.ToLowerInvariant().Replace("api/", "")}s.", ex);
            }
        }

        public virtual async Task<TModel?> GetByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiEndpoint}/{id}");
                response.EnsureSuccessStatusCode();

                var responseString = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<TModel>>(responseString, _jsonSerializerOptions);

                if (apiResponse != null && apiResponse.success)
                {
                    return apiResponse.Data;
                }
                else
                {
                    _logger.LogWarning("API returned success false or no data for GetById {Id}. Endpoint: {Endpoint}, Message: {Message}", id, _apiEndpoint, apiResponse?.Message);
                    return null;
                }
            }
            catch (HttpRequestException httpEx) when (httpEx.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _logger.LogWarning("Item with ID {Id} not found. Endpoint: {Endpoint}", id, _apiEndpoint);
                return null;
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError(httpEx, "HTTP error during GetById {Id}. Endpoint: {Endpoint}", id, _apiEndpoint);
                throw new ApplicationException($"Error de comunicación con la API al obtener {_apiEndpoint.ToLowerInvariant().Replace("api/", "")} con ID {id}.", httpEx);
            }
            catch (JsonException jsonEx)
            {
                _logger.LogError(jsonEx, "JSON deserialization error during GetById {Id}. Endpoint: {Endpoint}", id, _apiEndpoint);
                throw new ApplicationException($"Error al procesar la respuesta de la API al obtener {_apiEndpoint.ToLowerInvariant().Replace("api/", "")} con ID {id}.", jsonEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred during GetById {Id}. Endpoint: {Endpoint}", id, _apiEndpoint);
                throw new ApplicationException($"Ocurrió un error inesperado al obtener {_apiEndpoint.ToLowerInvariant().Replace("api/", "")} con ID {id}.", ex);
            }
        }

        public virtual Task<TEditModel?> GetEditModelByIdAsync(int id)
        {
            throw new NotImplementedException("GetEditModelByIdAsync must be implemented by the derived class.");
        }

        public virtual async Task<bool> CreateAsync(TCreateModel model)
        {
            try
            {
                var content = JsonContent.Create(model, options: _jsonSerializerOptions);
                var response = await _httpClient.PostAsync(_apiEndpoint, content);
                response.EnsureSuccessStatusCode();

                var responseString = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<object>>(responseString, _jsonSerializerOptions);

                if (apiResponse != null && apiResponse.success)
                {
                    return true;
                }
                else
                {
                    _logger.LogWarning("API returned success false for Create. Endpoint: {Endpoint}, Message: {Message}", _apiEndpoint, apiResponse?.Message);
                    return false;
                }
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError(httpEx, "HTTP error during Create. Endpoint: {Endpoint}", _apiEndpoint);
                throw new ApplicationException($"Error de comunicación con la API al crear {_apiEndpoint.ToLowerInvariant().Replace("api/", "")}.", httpEx);
            }
            catch (JsonException jsonEx)
            {
                _logger.LogError(jsonEx, "JSON serialization/deserialization error during Create. Endpoint: {Endpoint}", _apiEndpoint);
                throw new ApplicationException($"Error al procesar la solicitud/respuesta de la API al crear {_apiEndpoint.ToLowerInvariant().Replace("api/", "")}.", jsonEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred during Create. Endpoint: {Endpoint}", _apiEndpoint);
                throw new ApplicationException($"Ocurrió un error inesperado al crear {_apiEndpoint.ToLowerInvariant().Replace("api/", "")}.", ex);
            }
        }

        public virtual async Task<bool> UpdateAsync(int id, TEditModel model)
        {
            try
            {
                var content = JsonContent.Create(model, options: _jsonSerializerOptions);
                var response = await _httpClient.PutAsync($"{_apiEndpoint}/{id}", content);
                response.EnsureSuccessStatusCode();

                var responseString = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<object>>(responseString, _jsonSerializerOptions);

                if (apiResponse != null && apiResponse.success)
                {
                    return true;
                }
                else
                {
                    _logger.LogWarning("API returned success false for Update {Id}. Endpoint: {Endpoint}, Message: {Message}", id, _apiEndpoint, apiResponse?.Message);
                    return false;
                }
            }
            catch (HttpRequestException httpEx) when (httpEx.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _logger.LogWarning("Item with ID {Id} not found for update. Endpoint: {Endpoint}", id, _apiEndpoint);
                return false;
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError(httpEx, "HTTP error during Update {Id}. Endpoint: {Endpoint}", id, _apiEndpoint);
                throw new ApplicationException($"Error de comunicación con la API al actualizar {_apiEndpoint.ToLowerInvariant().Replace("api/", "")} con ID {id}.", httpEx);
            }
            catch (JsonException jsonEx)
            {
                _logger.LogError(jsonEx, "JSON serialization/deserialization error during Update {Id}. Endpoint: {Endpoint}", id, _apiEndpoint);
                throw new ApplicationException($"Error al procesar la solicitud/respuesta de la API al actualizar {_apiEndpoint.ToLowerInvariant().Replace("api/", "")} con ID {id}.", jsonEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred during Update {Id}. Endpoint: {Endpoint}", id, _apiEndpoint);
                throw new ApplicationException($"Ocurrió un error inesperado al actualizar {_apiEndpoint.ToLowerInvariant().Replace("api/", "")} con ID {id}.", ex);
            }
        }

        public virtual async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_apiEndpoint}/{id}");
                response.EnsureSuccessStatusCode();

                return true;
            }
            catch (HttpRequestException httpEx) when (httpEx.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _logger.LogWarning("Item with ID {Id} not found for deletion. Endpoint: {Endpoint}", id, _apiEndpoint);
                return false;
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError(httpEx, "HTTP error during Delete {Id}. Endpoint: {Endpoint}", id, _apiEndpoint);
                throw new ApplicationException($"Error de comunicación con la API al eliminar {_apiEndpoint.ToLowerInvariant().Replace("api/", "")} con ID {id}.", httpEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred during Delete {Id}. Endpoint: {Endpoint}", id, _apiEndpoint);
                throw new ApplicationException($"Ocurrió un error inesperado al eliminar {_apiEndpoint.ToLowerInvariant().Replace("api/", "")} con ID {id}.", ex);
            }
        }
    }
}
