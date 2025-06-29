
namespace SIGEBI.Application.DTOs
{
    public class CreateReservationRequestDto // crear reservacion
    {
        public int BookId { get; set; }
        public int UserId { get; set; }
        public int StatusId { get; set; } // 1: pendiente, 2: Confirmado, 3: Expirado
        public DateTime ReservationDate { get; set; }
        //public DateTime ExpirationDate { get; set; }
    }
}
