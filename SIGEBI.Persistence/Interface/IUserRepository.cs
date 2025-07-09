using SIGEBI.Domain.Entities;
using SIGEBI.Persistence.Base;
using System.Threading.Tasks;

namespace SIGEBI.Persistence.Interfaces
{
    public interface IUserRepository : IBaseRepository<User, int>
    {
        Task<User?> GetByEmailAsync(string email);
    }
}