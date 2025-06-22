
using SIGEBI.Domain.Entities;
using SIGEBI.Persistence.Base;
using SIGEBI.Persistence.Interface;
using System.Collections.Generic;
using System.Linq;

namespace SIGEBI.Persistence.Repositori
{
    public class ReservationHistoryRepository : BaseRepository<ReservationHistory>, IReservationHistoryRepository
    {
        public IEnumerable<ReservationHistory> GetByOriginalReservationId(int originalReservationId)
        {
            return _data.Where(rh => rh.OriginalReservationId == originalReservationId);
        }

        public IEnumerable<ReservationHistory> GetByUserId(int userId)
        {
            return _data.Where(rh => rh.UserId == userId);
        }
    }
}
