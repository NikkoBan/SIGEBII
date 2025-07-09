
namespace SIGEBI.Application.DTOs
{
    public class ReservationHistoryDto : Base.BaseAuditDTo
    {
        public int HistoryId { get; set; }
        public int ReservationId { get; set; }
        public string StatusName { get; set; }

    }
}
