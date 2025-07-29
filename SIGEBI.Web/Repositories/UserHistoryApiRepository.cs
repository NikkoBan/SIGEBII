using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using SIGEBI.Application.DTOsAplication.UserHistoryDTOs;
using Microsoft.Extensions.Configuration;

namespace SIGEBI.Web.Repositories
{
    public class UserHistoryApiRepository : IUserHistoryApiRepository
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;

        public UserHistoryApiRepository(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiBaseUrl = configuration.GetValue<string>("ApiSettings:BaseUrl") + "UserHistory/";
        }

        public async Task<IEnumerable<UserHistoryDisplayDto>?> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<UserHistoryDisplayDto>>(_apiBaseUrl);
        }

        public async Task<UserHistoryDisplayDto?> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<UserHistoryDisplayDto>($"{_apiBaseUrl}{id}");
        }

        public async Task<IEnumerable<UserHistoryDisplayDto>?> GetByUserIdAsync(int userId)
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<UserHistoryDisplayDto>>($"{_apiBaseUrl}user/{userId}");
        }

        public async Task<bool> CreateAsync(UserHistoryCreationDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync(_apiBaseUrl, dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_apiBaseUrl}{id}");
            return response.IsSuccessStatusCode;
        }
    }
}