using SIGEBI.Domain.Entities;
using SIGEBI.Persistence.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIGEBI.Persistence.Interfaces
{
    public interface IUserHistoryRepository : IBaseRepository<UserHistory, int>
    {
        Task<IEnumerable<UserHistory>> GetByUserIdAsync(int userId);
    }
}