// SIGEBI.Persistence/Interface/IReservationStatusRepository.cs
using SIGEBI.Domain.Entities;
using System.Collections.Generic;

namespace SIGEBI.Persistence.Interface
{
    public interface IReservationStatusRepository : IBaseRepository<ReservationStatus>
    {
        // Métodos específicos para ReservationStatus si los hay
        ReservationStatus? GetByStatusName(string statusName);
    }
}