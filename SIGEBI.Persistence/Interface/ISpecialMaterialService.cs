// SIGEBI.Persistence/Interface/ISpecialMaterialService.cs
using SIGEBI.Domain.Entities;
using System.Collections.Generic;

namespace SIGEBI.Persistence.Interface
{
    public interface ISpecialMaterialService
    {
        SpecialMaterial? GetSpecialMaterialById(int materialId);
        IEnumerable<SpecialMaterial> GetAllSpecialMaterials();
        void RequestSpecialMaterial(SpecialMaterial material);
        void ApproveSpecialMaterial(int materialId, int approvedByUserId);
        void RejectSpecialMaterial(int materialId, string reason);
        IEnumerable<SpecialMaterial> GetSpecialMaterialsByRequestedUser(int userId);
        IEnumerable<SpecialMaterial> GetSpecialMaterialsByStatus(string status);
    }
}
