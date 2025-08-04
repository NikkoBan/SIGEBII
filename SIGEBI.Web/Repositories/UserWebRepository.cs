using Microsoft.Extensions.Logging;
using SIGEBI.Web.Models.Common;
using SIGEBI.Web.Models.Users;
using SIGEBI.Web.Repositories.interfaces;
using SIGEBI.Web.Services;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SIGEBI.Web.Repositories
{
    public class UserWebRepository : BaseApiRepository, IUserWebRepository
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "https://localhost:7276/api/Users";

        public UserWebRepository(HttpClient httpClient, ILogger<UserWebRepository> logger)
            : base(logger)
        {
            _httpClient = httpClient;
        }

        public Task<ApiResponse<List<UserViewModel>>> GetAllAsync() =>
            SafeApiCall(async () =>
                await _httpClient.GetOrDefaultAsync<List<UserViewModel>>(_baseUrl) ?? new List<UserViewModel>(),
                nameof(GetAllAsync));

        public Task<ApiResponse<UserViewModel?>> GetByIdAsync(int id) =>
            SafeApiCall(async () =>
                await _httpClient.GetOrDefaultAsync<UserViewModel>($"{_baseUrl}/{id}"),
                nameof(GetByIdAsync));

        public Task<ApiResponse<UserViewModel?>> GetByEmailAsync(string email) =>
            SafeApiCall(async () =>
                await _httpClient.GetOrDefaultAsync<UserViewModel>($"{_baseUrl}/email/{email}"),
                nameof(GetByEmailAsync));

        public async Task<ApiResponse<bool>> RegisterAsync(UserRequest dto)
        {
            return await SafeApiCall(
                async () => await _httpClient.PostJsonOkAsync($"{_baseUrl}/register", dto),
                nameof(RegisterAsync)
            );
        }

        public async Task<ApiResponse<bool>> UpdateAsync(int id, UserUpdateRequest dto)
        {
            return await SafeApiCall(
                async () => await _httpClient.PutJsonOkAsync($"{_baseUrl}/{id}", dto),
                nameof(UpdateAsync)
            );
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int id)
        {
            return await SafeApiCall(
                async () => await _httpClient.DeleteOkAsync($"{_baseUrl}/{id}"),
                nameof(DeleteAsync)
            );
        }
    }
}