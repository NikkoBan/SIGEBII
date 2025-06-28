namespace SIGEBI.api.DTOs.Reservations
{
    public class ReservationHistoryDto
    {
        public int HistoryId { get; set; }
        public int ReservationId { get; set; }
        public int BookId { get; set; }
        public int UserId { get; set; }
        public ReservationStatusDto Status { get; set; } // DTO completo del estado
        public DateTime ReservationDate { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
