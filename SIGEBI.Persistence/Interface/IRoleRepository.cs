// SIGEBI.Persistence/Interface/IRoleRepository.cs
using SIGEBI.Domain.Entities;
using System.Collections.Generic;

namespace SIGEBI.Persistence.Interface
{
    public interface IRoleRepository : IBaseRepository<Role>
    {
        // Métodos específicos para Role si los hay
        Role? GetByRoleName(string roleName);
    }
}
