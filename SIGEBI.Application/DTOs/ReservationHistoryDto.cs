
namespace SIGEBI.Application.DTOs
{
    public class ReservationHistoryDto
    {
        public int HistoryId { get; set; }
        public int ReservationId { get; set; }
        public string StatusName { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }

    }
}
