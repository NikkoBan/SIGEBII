using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace SIGEBI.Domain.Entities
{
    public class LoanHistory
    {
        public int HistoryId { get; set; }  
        public int LoanId { get; set; }
        public Loan Loan { get; set; } = null!;
        public int StatusId { get; set; }
        public DateTime ChangedAt { get; set; }
        public string ChangedBy { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public int BookId { get; set; }
        public int UserId { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public string? FinalStatus { get; set; }
        public string? Observations { get; set; }
    }
}

