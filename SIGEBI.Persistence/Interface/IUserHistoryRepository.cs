using SIGEBI.Domain.Entities;
using SIGEBI.Persistence.Interfaces;

public interface IUserHistoryRepository : IBaseRepository<UserHistory, int>
{
    Task<List<UserHistory>> GetByUserIdAsync(int userId); 
}