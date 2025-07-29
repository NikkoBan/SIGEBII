using SIGEBI.Application.DTOsAplication.UserDTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIGEBI.Web.Repositories
{
    public interface IUserApiRepository
    {
        Task<IEnumerable<UserDisplayDto>?> GetAllAsync();
        Task<UserDisplayDto?> GetByIdAsync(int id);
        Task<bool> RegisterAsync(UserCreationDto dto);
        Task<bool> UpdateAsync(int id, UserUpdateDto dto);
        Task<bool> DeleteAsync(int id);
        Task<bool> LoginAsync(string email, string password);
    }
}