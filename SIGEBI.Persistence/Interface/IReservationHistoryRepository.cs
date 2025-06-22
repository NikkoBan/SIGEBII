using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Persistence.Interface
{
    public interface IReservationHistoryRepository : IBaseRepository<ReservationHistory>
    {
        IEnumerable<ReservationHistory> GetByOriginalReservationId(int originalReservationId);
        IEnumerable<ReservationHistory> GetByUserId(int userId);
    }
}

