using System.Collections.Generic;
using System.Threading.Tasks;
using SIGEBI.Application.DTOsAplication.UserHistoryDTOs;

namespace SIGEBI.Web.Repositories
{
    public interface IUserHistoryApiRepository
    {
        Task<IEnumerable<UserHistoryDisplayDto>?> GetAllAsync();
        Task<UserHistoryDisplayDto?> GetByIdAsync(int id);
        Task<IEnumerable<UserHistoryDisplayDto>?> GetByUserIdAsync(int userId);
        Task<bool> CreateAsync(UserHistoryCreationDto dto);
        Task<bool> DeleteAsync(int id);
    }
}