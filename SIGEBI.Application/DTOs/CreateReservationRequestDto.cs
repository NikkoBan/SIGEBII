
namespace SIGEBI.Application.DTOs
{
    public class CreateReservationRequestDto // crear reservacion
    {
        public int BookId { get; set; }
        public int UserId { get; set; }
        public string CreatedBy { get; set; } = string.Empty;

    }
}
