namespace SIGEBI.Web.Services
{
    public class HttpService : IHttpService
    {
        private readonly HttpClient _httpClient;
        public HttpService(HttpClient httpClient) => _httpClient = httpClient;

        public async Task<T?> GetAsync<T>(string url)
        {
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode) return default;
            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task<TResponse?> PostAsync<TRequest, TResponse>(string url, TRequest data)
        {
            var response = await _httpClient.PostAsJsonAsync(url, data);
            if (!response.IsSuccessStatusCode) return default;
            return await response.Content.ReadFromJsonAsync<TResponse>();
        }

        public async Task<TResponse?> PutAsync<TRequest, TResponse>(string url, TRequest data)
        {
            var response = await _httpClient.PutAsJsonAsync(url, data);
            if (!response.IsSuccessStatusCode) return default;
            return await response.Content.ReadFromJsonAsync<TResponse>();
        }
    }
}
