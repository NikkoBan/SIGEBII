using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Application.DTOs
{
    public class ReservationHistoryByIdDto : Base.BaseAuditDTo
    {
        public int HistoryId { get; set; }
        public int ReservationId { get; set; }
        public string StatusName { get; set; } = string.Empty;
    }
}
