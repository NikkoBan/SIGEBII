using SIGEBI.Application.DTOsAplication.UserHistoryDTOs;
using SIGEBI.Persistence.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIGEBI.Application.Interfaces
{
    public interface IUserHistoryService
    {
        Task<OperationResult<UserHistoryDisplayDto>> GetUserHistoryByIdAsync(int id);
        Task<OperationResult<IEnumerable<UserHistoryDisplayDto>>> GetAllUserHistoryAsync();
        Task<OperationResult<IEnumerable<UserHistoryDisplayDto>>> GetUserHistoryByUserIdAsync(int userId);
        Task<OperationResult<UserHistoryDisplayDto>> CreateUserHistoryAsync(UserHistoryCreationDto historyDto);
        Task<OperationResult> DeleteUserHistoryAsync(int id);
    }
}