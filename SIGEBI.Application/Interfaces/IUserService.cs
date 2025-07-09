using SIGEBI.Application.DTOsAplication.UserDTOs;
using SIGEBI.Persistence.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIGEBI.Application.Interfaces
{
    public interface IUserService
    {
        Task<OperationResult<UserDisplayDto>> GetUserByIdAsync(int id);
        Task<OperationResult<IEnumerable<UserDisplayDto>>> GetAllUsersAsync();
        Task<OperationResult<UserDisplayDto>> CreateUserAsync(UserCreationDto userDto);
        Task<OperationResult> UpdateUserAsync(int id, UserUpdateDto userDto);
        Task<OperationResult> DeleteUserAsync(int id);
        Task<OperationResult<UserDisplayDto>> GetUserByEmailAsync(string email);
    }
}