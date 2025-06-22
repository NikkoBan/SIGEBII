// SIGEBI.Persistence/Repositori/RoleRepository.cs
using SIGEBI.Domain.Entities;
using SIGEBI.Persistence.Interface;
using System.Linq;

namespace SIGEBI.Persistence.Repositori
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public Role? GetByRoleName(string roleName)
        {
            return _data.FirstOrDefault(r => r.RoleName == roleName);
        }
    }
}