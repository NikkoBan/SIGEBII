// SIGEBI.Persistence/Interface/ISpecialMaterialRepository.cs
using SIGEBI.Domain.Entities;
using System.Collections.Generic;

namespace SIGEBI.Persistence.Interface
{
    public interface ISpecialMaterialRepository : IBaseRepository<SpecialMaterial>
    {
        // Métodos específicos para SpecialMaterial si los hay
        IEnumerable<SpecialMaterial> GetByRequestedByUserId(int userId);
        IEnumerable<SpecialMaterial> GetByApprovalStatus(string status);
    }
}
