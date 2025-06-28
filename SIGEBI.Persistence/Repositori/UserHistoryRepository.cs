using Microsoft.EntityFrameworkCore;
using SIGEBI.Domain.Entities;
using SIGEBI.Persistence.Base;
using SIGEBI.Persistence.Context;
using SIGEBI.Persistence.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SIGEBI.Persistence.Repositori
{
    public class UserHistoryRepository : BaseRepository<UserHistory, int>, IUserHistoryRepository
    {
        public UserHistoryRepository(SIGEBIDbContext context) : base(context)
        {
        }

        public async Task<List<UserHistory>> GetByUserIdAsync(int userId)
        {
            return await Entity.Where(h => h.UserId == userId).ToListAsync();
        }
    }
}