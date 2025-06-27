
namespace SIGEBI.Application.DTOs
{
    public class ReservationHistoryViewDto
    {
        public int HistoryId { get; set; }
        public int ReservationId { get; set; }
        public string StatusName { get; set; }
        public DateTime ChangeDate { get; set; }
        public string ChangedBy { get; set; } // Assuming this is a string, adjust as necessary
 
    }
}
