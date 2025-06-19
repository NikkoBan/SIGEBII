using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Domain.Entities
{
    public class LoanHistory
    {
        public int HistoryId { get; set; }
        public int OriginalLoanId { get; set; }
        public int BookId { get; set; }
        public int UserId { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public string FinalStatus { get; set; } = string.Empty;
        public string? Observations { get; set; }
    }
}

