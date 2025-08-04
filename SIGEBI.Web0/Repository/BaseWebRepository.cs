using SIGEBI.Web0.Interfaz;
using System.Text.Json;
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

        protected JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

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
                var res = await _httpClient.GetAsync(_apiEndpoint);
                res.EnsureSuccessStatusCode();

                var json = await res.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<ApiResponse<List<TModel>>>(json, _jsonOptions);

                if (result?.success == true)
                    return result.Data ?? new();

                _logger.LogWarning("No se pudo obtener la lista. Endpoint: {0}", _apiEndpoint);
                return new();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos. Endpoint: {0}", _apiEndpoint);
                throw new ApplicationException($"No se pudo obtener la lista desde {_apiEndpoint}.", ex);
            }
        }

        public virtual async Task<TModel?> GetByIdAsync(int id)
        {
            try
            {
                var res = await _httpClient.GetAsync($"{_apiEndpoint}/{id}");
                res.EnsureSuccessStatusCode();

                var json = await res.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<ApiResponse<TModel>>(json, _jsonOptions);

                if (result?.success == true)
                    return result.Data;

                _logger.LogWarning("No se encontró el item con ID {0}.", id);
                return null;
            }
            catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _logger.LogWarning("No se encontró el ID {0} en {1}", id, _apiEndpoint);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en GetById con ID {0}", id);
                throw new ApplicationException($"No se pudo obtener el elemento con ID {id}.", ex);
            }
        }

        public virtual Task<TEditModel?> GetEditModelByIdAsync(int id)
            => throw new NotImplementedException("Debes implementar esto en tu repo.");

        public virtual async Task<bool> CreateAsync(TCreateModel model)
        {
            try
            {
                var content = JsonContent.Create(model, options: _jsonOptions);
                var res = await _httpClient.PostAsync(_apiEndpoint, content);
                res.EnsureSuccessStatusCode();

                var json = await res.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<ApiResponse<object>>(json, _jsonOptions);

                if (result?.success == true) return true;

                _logger.LogWarning("Falló el Create en {0}", _apiEndpoint);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando en {0}", _apiEndpoint);
                throw new ApplicationException($"No se pudo crear en {_apiEndpoint}.", ex);
            }
        }

        public virtual async Task<bool> UpdateAsync(int id, TEditModel model)
        {
            try
            {
                var content = JsonContent.Create(model, options: _jsonOptions);
                var res = await _httpClient.PutAsync($"{_apiEndpoint}/{id}", content);
                res.EnsureSuccessStatusCode();

                var json = await res.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<ApiResponse<object>>(json, _jsonOptions);

                if (result?.success == true) return true;

                _logger.LogWarning("Falló el Update con ID {0}", id);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando ID {0}", id);
                throw new ApplicationException($"No se pudo actualizar el ID {id}.", ex);
            }
        }

        public virtual async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var res = await _httpClient.DeleteAsync($"{_apiEndpoint}/{id}");
                res.EnsureSuccessStatusCode();
                return true;
            }
            catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _logger.LogWarning("No se encontró para eliminar el ID {0}", id);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando ID {0}", id);
                throw new ApplicationException($"No se pudo eliminar el ID {id}.", ex);
            }
        }
    }
}
