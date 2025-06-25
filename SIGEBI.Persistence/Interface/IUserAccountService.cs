using SIGEBI.Domain.Entities;
using SIGEBI.Persistence.Base;

namespace SIGEBI.Persistence.Interfaces
{
    public interface IUserAccountService
    {
        Task<OperationResult<bool>> LoginAsync(string email, string password, string ipAddress, string userAgent);
        Task<OperationResult> LogoutAsync(int userId);
        Task<User?> GetUserByEmailAsync(string email);
        Task<IEnumerable<UserHistory>> GetUserHistoryAsync(int userId);
        Task<OperationResult> RegisterAsync(User user);
        Task<OperationResult> UpdateUserAsync(User user);
        Task<OperationResult> DeleteUserAsync(int userId);
    }
}