
namespace SIGEBI.Application.DTOs
{
    public class UpdateReservationDto
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int UserId { get; set; }
        public int StatusId { get; set; }
        public DateTime ReservationDate { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
