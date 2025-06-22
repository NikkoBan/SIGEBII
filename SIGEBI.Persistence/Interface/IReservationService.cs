using SIGEBI.Domain.Entities;
using System.Collections.Generic;

namespace SIGEBI.Persistence.Interface
{
    public interface IReservationService
    {
        Reservation? GetReservationById(int reservationId);
        IEnumerable<Reservation> GetAllReservations();
        void CreateReservation(Reservation reservation);
        void UpdateReservation(Reservation reservation);
        void CancelReservation(int reservationId, string cancelledBy);
        IEnumerable<Reservation> GetReservationsByUserId(int userId);
        IEnumerable<Reservation> GetReservationsByStatus(int statusId);
    }
}
