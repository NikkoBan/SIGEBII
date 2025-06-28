namespace SIGEBI.api.DTOs.Reservations
{
    public class ReservationDto
    {
        int Id { get; set; }
        public string UserId { get; set; }
        public string BookId { get; set; }
        public DateTime ReservationDate { get; set; }
        public ReservationStatusDto ReservationStatus { get; set; }
    }
}
