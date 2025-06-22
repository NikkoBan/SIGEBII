using SIGEBI.Domain.Entities;
using SIGEBI.Persistence.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIGEBI.Persistence.Repositori
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IReservationHistoryRepository _reservationHistoryRepository;
        private readonly IReservationStatusRepository _reservationStatusRepository; // Para obtener el ID del estado

        public ReservationService(
            IReservationRepository reservationRepository,
            IReservationHistoryRepository reservationHistoryRepository,
            IReservationStatusRepository reservationStatusRepository) // Inyectar el repositorio de estados
        {
            _reservationRepository = reservationRepository;
            _reservationHistoryRepository = reservationHistoryRepository;
            _reservationStatusRepository = reservationStatusRepository;
        }

        public Reservation? GetReservationById(int reservationId)
        {
            return _reservationRepository.GetById(reservationId);
        }

        public IEnumerable<Reservation> GetAllReservations()
        {
            return _reservationRepository.GetAll();
        }

        public void CreateReservation(Reservation reservation)
        {
            // Asigna valores por defecto si no están ya definidos
            if (reservation.CreatedAt == default)
            {
                reservation.CreatedAt = DateTime.Now;
            }
            if (string.IsNullOrEmpty(reservation.CreatedBy))
            {
                reservation.CreatedBy = "System"; // O un usuario logueado
            }
            // Suponiendo que el ID de estado "Pending" es 1
            var pendingStatus = _reservationStatusRepository.GetByStatusName("Pending");
            reservation.StatusId = pendingStatus?.StatusId ?? 1; // Asigna el ID del estado "Pending"

            _reservationRepository.Add(reservation);

            // Registrar historial de la reserva
            var history = new ReservationHistory
            {
                OriginalReservationId = reservation.ReservationId,
                BookId = reservation.BookId,
                UserId = reservation.UserId,
                ReservationDate = reservation.ReservationDate,
                ExpirationDate = reservation.ExpirationDate,
                FinalStatus = _reservationStatusRepository.GetById(reservation.StatusId)?.StatusName ?? "Pending",
                UpdateDate = DateTime.Now
            };
            _reservationHistoryRepository.Add(history);
        }

        public void UpdateReservation(Reservation reservation)
        {
            var existingReservation = _reservationRepository.GetById(reservation.ReservationId);
            if (existingReservation != null)
            {
                existingReservation.BookId = reservation.BookId;
                existingReservation.UserId = reservation.UserId;
                existingReservation.ReservationDate = reservation.ReservationDate;
                existingReservation.ExpirationDate = reservation.ExpirationDate;
                existingReservation.StatusId = reservation.StatusId;
                existingReservation.UpdatedAt = DateTime.Now;
                existingReservation.UpdatedBy = reservation.UpdatedBy;

                _reservationRepository.Update(existingReservation);

                // Registrar historial de la actualización
                var history = new ReservationHistory
                {
                    OriginalReservationId = reservation.ReservationId,
                    BookId = reservation.BookId,
                    UserId = reservation.UserId,
                    ReservationDate = reservation.ReservationDate,
                    ExpirationDate = reservation.ExpirationDate,
                    FinalStatus = _reservationStatusRepository.GetById(reservation.StatusId)?.StatusName ?? "Unknown",
                    UpdateDate = DateTime.Now
                };
                _reservationHistoryRepository.Add(history);
            }
        }

        public void CancelReservation(int reservationId, string cancelledBy)
        {
            var reservation = _reservationRepository.GetById(reservationId);
            if (reservation != null)
            {
                var cancelledStatus = _reservationStatusRepository.GetByStatusName("Cancelled");
                reservation.StatusId = cancelledStatus?.StatusId ?? 0; // Asigna el ID del estado "Cancelled"
                reservation.UpdatedAt = DateTime.Now;
                reservation.UpdatedBy = cancelledBy;
                _reservationRepository.Update(reservation);

                // Registrar historial de la cancelación
                var history = new ReservationHistory
                {
                    OriginalReservationId = reservation.ReservationId,
                    BookId = reservation.BookId,
                    UserId = reservation.UserId,
                    ReservationDate = reservation.ReservationDate,
                    ExpirationDate = reservation.ExpirationDate,
                    FinalStatus = "Cancelled",
                    UpdateDate = DateTime.Now
                };
                _reservationHistoryRepository.Add(history);
            }
        }

        public IEnumerable<Reservation> GetReservationsByUserId(int userId)
        {
            return _reservationRepository.GetByUserId(userId);
        }

        public IEnumerable<Reservation> GetReservationsByStatus(int statusId)
        {
            return _reservationRepository.GetByStatusId(statusId);
        }
    }
}
