using SIGEBI.Domain.Entities;
using SIGEBI.Persistence.Base;
using SIGEBI.Persistence.Interface;
using System.Collections.Generic;
using System.Linq;

namespace SIGEBI.Persistence.Repositori
{
    public class ReservationRepository : BaseRepository<Reservation>, IReservationRepository
    {
        public IEnumerable<Reservation> GetByUserId(int userId)
        {
            return _data.Where(r => r.UserId == userId);
        }

        public IEnumerable<Reservation> GetByBookId(int bookId)
        {
            return _data.Where(r => r.BookId == bookId);
        }

        public IEnumerable<Reservation> GetByStatusId(int statusId)
        {
            return _data.Where(r => r.StatusId == statusId);
        }
    }
}
