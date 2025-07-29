using System.Net.Http;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using SIGEBI.Application.DTOsAplication.UserHistoryDTOs;

namespace SIGEBI.Web.Services
{
    public class UserHistoryApiClient
    {
        private readonly HttpClient _httpClient;

        public UserHistoryApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<UserHistoryDisplayDto>?> GetAllHistoriesAsync()
        {
            var result = await _httpClient.GetFromJsonAsync<IEnumerable<UserHistoryDisplayDto>>("https://localhost:7276/api/UserHistory");
            return result;
        }

        public async Task<UserHistoryDisplayDto?> GetHistoryByIdAsync(int id)
        {
            var result = await _httpClient.GetFromJsonAsync<UserHistoryDisplayDto>($"https://localhost:7276/api/UserHistory/{id}");
            return result;
        }

        public async Task<IEnumerable<UserHistoryDisplayDto>?> GetHistoryByUserIdAsync(int userId)
        {
            var result = await _httpClient.GetFromJsonAsync<IEnumerable<UserHistoryDisplayDto>>($"https://localhost:7276/api/UserHistory/user/{userId}");
            return result;
        }

        public async Task<bool> CreateHistoryAsync(UserHistoryCreationDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("https://localhost:7276/api/UserHistory", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteHistoryAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:7276/api/UserHistory/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}