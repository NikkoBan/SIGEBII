using SIGEBI.Domain.Entities;
using SIGEBI.Persistence.Interface;
using System.Collections.Generic;
using System.Linq;

namespace SIGEBI.Persistence.Repositori
{
    public class UserHistoryRepository : IUserHistoryRepository
    {
        private readonly List<UserHistory> _histories;

        public UserHistoryRepository(List<UserHistory> histories)
        {
            _histories = histories;
        }

        public void Add(UserHistory history) => _histories.Add(history);
        public IEnumerable<UserHistory> GetByUserId(int userId) => _histories.Where(h => h.UserId == userId);
    }
}