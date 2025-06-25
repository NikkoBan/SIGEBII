using Microsoft.EntityFrameworkCore;
using SIGEBI.Domain.Entities;
using SIGEBI.Persistence.Base;
using SIGEBI.Persistence.Context;
using SIGEBI.Persistence.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace SIGEBI.Persistence.Repositori
{
    public class UserRepository : BaseRepository<User, int>, IUserRepository
    {
        public UserRepository(SIGEBIDbContext context) : base(context)
        {
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await Entity.FirstOrDefaultAsync(u => u.InstitutionalEmail != null && u.InstitutionalEmail.ToLower() == email.ToLower());
        }
    }
}