using SIGEBI.Web.Models.UserHistory;
using SIGEBI.Web.Models.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIGEBI.Web.Repositories.interfaces
{
    public interface IUserHistoryWebRepository
    {
        Task<ApiResponse<List<UserHistoryViewModel>>> GetAllAsync();
        Task<ApiResponse<UserHistoryViewModel?>> GetByIdAsync(int id);
        Task<ApiResponse<List<UserHistoryViewModel>>> GetByUserIdAsync(int userId);
        Task<ApiResponse<bool>> CreateAsync(UserHistoryRequest dto);
        Task<ApiResponse<bool>> DeleteAsync(int id);
    }
}