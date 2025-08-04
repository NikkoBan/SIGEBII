using Microsoft.Extensions.Logging;
using SIGEBI.Web.Models.Common;
using SIGEBI.Web.Models.UserHistory;
using SIGEBI.Web.Repositories.interfaces;
using SIGEBI.Web.Services;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SIGEBI.Web.Repositories
{
    public class UserHistoryWebRepository : BaseApiRepository, IUserHistoryWebRepository
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "https://localhost:7276/api/UserHistory";

        public UserHistoryWebRepository(HttpClient httpClient, ILogger<UserHistoryWebRepository> logger)
            : base(logger)
        {
            _httpClient = httpClient;
        }

        public Task<ApiResponse<List<UserHistoryViewModel>>> GetAllAsync() =>
            SafeApiCall(async () =>
                await _httpClient.GetOrDefaultAsync<List<UserHistoryViewModel>>(_baseUrl) ?? new List<UserHistoryViewModel>(),
                nameof(GetAllAsync));

        public Task<ApiResponse<UserHistoryViewModel?>> GetByIdAsync(int id) =>
            SafeApiCall(async () =>
                await _httpClient.GetOrDefaultAsync<UserHistoryViewModel>($"{_baseUrl}/{id}"),
                nameof(GetByIdAsync));

        public Task<ApiResponse<List<UserHistoryViewModel>>> GetByUserIdAsync(int userId) =>
            SafeApiCall(async () =>
                await _httpClient.GetOrDefaultAsync<List<UserHistoryViewModel>>($"{_baseUrl}/user/{userId}") ?? new List<UserHistoryViewModel>(),
                nameof(GetByUserIdAsync));

        public Task<ApiResponse<bool>> CreateAsync(UserHistoryRequest dto) =>
            SafeApiCall(async () =>
                await _httpClient.PostJsonOkAsync(_baseUrl, dto),
                nameof(CreateAsync));

        public Task<ApiResponse<bool>> DeleteAsync(int id) =>
            SafeApiCall(async () =>
                await _httpClient.DeleteOkAsync($"{_baseUrl}/{id}"),
                nameof(DeleteAsync));
    }
}