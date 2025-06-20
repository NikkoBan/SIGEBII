using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Domain.Entities
{
    public class Sanction
    {
        public int SanctionId { get; set; }
        public int UserId { get; set; }
        public int LoanId { get; set; }
        public string SanctionType { get; set; } = null!;
        public decimal? FineAmount { get; set; }
        public DateTime ImpositionDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string? Reason { get; set; }
        public string SanctionStatus { get; set; } = null!;
    }
}
