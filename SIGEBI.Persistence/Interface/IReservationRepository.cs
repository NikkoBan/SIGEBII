using SIGEBI.Domain.Entities;
using System.Collections.Generic;

namespace SIGEBI.Persistence.Interface
{
    public interface IReservationRepository : IBaseRepository<Reservation>
    {
        // Métodos específicos para Reservation si los hay
        IEnumerable<Reservation> GetByUserId(int userId);
        IEnumerable<Reservation> GetByBookId(int bookId);
        IEnumerable<Reservation> GetByStatusId(int statusId);
    }
}