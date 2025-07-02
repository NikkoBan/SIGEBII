
namespace SIGEBI.Application.DTOs
{
    public class DeleteReservationDto
    {
        public int ReservationId { get; set; } // ID de la reservación a eliminar
        public string DeletedBy { get; set; } = string.Empty; // Usuario que elimina la reservación
    }
}
