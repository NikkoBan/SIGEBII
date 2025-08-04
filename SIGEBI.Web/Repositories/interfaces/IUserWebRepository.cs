using SIGEBI.Web.Models.Users;
using SIGEBI.Web.Models.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIGEBI.Web.Repositories.interfaces
{
    public interface IUserWebRepository
    {
        Task<ApiResponse<List<UserViewModel>>> GetAllAsync();
        Task<ApiResponse<UserViewModel?>> GetByIdAsync(int id);
        Task<ApiResponse<UserViewModel?>> GetByEmailAsync(string email);
        Task<ApiResponse<bool>> RegisterAsync(UserRequest dto);
        Task<ApiResponse<bool>> UpdateAsync(int id, UserUpdateRequest dto);
        Task<ApiResponse<bool>> DeleteAsync(int id);
    }
}