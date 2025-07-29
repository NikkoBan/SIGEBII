using SIGEBI.Application.DTOsAplication.UserDTOs;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace SIGEBI.Web.Repositories
{
    public class UserApiRepository : IUserApiRepository
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;

        public UserApiRepository(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiBaseUrl = configuration.GetValue<string>("ApiSettings:BaseUrl") + "Users/";
        }

        public async Task<IEnumerable<UserDisplayDto>?> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<UserDisplayDto>>(_apiBaseUrl);
        }

        public async Task<UserDisplayDto?> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<UserDisplayDto>($"{_apiBaseUrl}{id}");
        }

        public async Task<bool> RegisterAsync(UserCreationDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_apiBaseUrl}register", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(int id, UserUpdateDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_apiBaseUrl}{id}", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_apiBaseUrl}{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            var loginModel = new { Email = email, Password = password };
            var content = new StringContent(JsonSerializer.Serialize(loginModel), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_apiBaseUrl}login", content);
            return response.IsSuccessStatusCode;
        }
    }
}