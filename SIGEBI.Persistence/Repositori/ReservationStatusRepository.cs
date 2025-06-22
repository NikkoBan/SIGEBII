// SIGEBI.Persistence/Repositori/ReservationStatusRepository.cs
using SIGEBI.Domain.Entities;
using SIGEBI.Persistence.Interface;
using System.Linq;

namespace SIGEBI.Persistence.Repositori
{
    public class ReservationStatusRepository : BaseRepository<ReservationStatus>, IReservationStatusRepository
    {
        public ReservationStatus? GetByStatusName(string statusName)
        {
            return _data.FirstOrDefault(rs => rs.StatusName == statusName);
        }
    }
}
