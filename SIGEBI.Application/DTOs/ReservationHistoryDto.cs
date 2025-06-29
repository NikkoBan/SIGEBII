
namespace SIGEBI.Application.DTOs
{
    public class ReservationHistoryDto
    {
        public int HistoryId { get; set; }
        public int ReservationId { get; set; }
        public int StatusId { get; set; }
        public DateTime ReservationDate { get; set; }
        public DateTime ExpirationDate { get; set; }

    }
}
