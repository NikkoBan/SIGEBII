using System.Net.Http;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using SIGEBI.Application.DTOsAplication.UserDTOs;

namespace SIGEBI.Web.Services
{
    public class UserApiClient
    {
        private readonly HttpClient _httpClient;

        public UserApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<UserDisplayDto>?> GetUsersAsync()
        {
            // Ajusta la URL si tu API está en otro host/puerto
            var result = await _httpClient.GetFromJsonAsync<IEnumerable<UserDisplayDto>>("https://localhost:7276/api/Users");
            return result;
        }

        public async Task<UserDisplayDto?> GetUserByIdAsync(int id)
        {
            var result = await _httpClient.GetFromJsonAsync<UserDisplayDto>($"https://localhost:7276/api/Users/{id}");
            return result;
        }

        public async Task<UserDisplayDto?> GetUserByEmailAsync(string email)
        {
            var url = $"https://localhost:7276/api/Users/email/{email}";
            var result = await _httpClient.GetFromJsonAsync<UserDisplayDto>(url);
            return result;
        }

        public async Task<bool> RegisterUserAsync(UserCreationDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("https://localhost:7276/api/Users/register", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateUserAsync(int id, UserUpdateDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"https://localhost:7276/api/Users/{id}", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:7276/api/Users/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}