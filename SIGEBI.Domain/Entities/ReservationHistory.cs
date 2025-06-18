using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Domain.Entities
{
    public class ReservationHistory
    {
        public int ReservationHistoryId { get; set; }
        public int OriginalReservationId { get; set; }
        public int BookId { get; set; }
        public int UserId { get; set; }
        public DateTime ReservationDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string FinalStatus { get; set; } = string.Empty;
        public DateTime UpdateDate { get; set; }
    }
}
