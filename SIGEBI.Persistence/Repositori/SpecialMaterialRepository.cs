using SIGEBI.Persistence.Interface;
using SIGEBI.Persistence.Repositori;
using SIGEBI.Domain.Entities;

namespace SIGEBI.Persistence.Repositori
{
    public class SpecialMaterialRepository : BaseRepository<SpecialMaterial>, ISpecialMaterialRepository
    {
        public IEnumerable<SpecialMaterial> GetByRequestedByUserId(int userId)
        {
            return _data.Where(sm => sm.RequestedByUserId == userId);
        }

        public IEnumerable<SpecialMaterial> GetByApprovalStatus(string status)
        {
            return _data.Where(sm => sm.ApprovalStatus == status);
        }
    }
}
