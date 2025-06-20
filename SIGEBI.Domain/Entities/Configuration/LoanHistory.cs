
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;


namespace SIGEBI.Domain.Entities.Configuration
{
    public class LoanHistory: LoanBase
    {
        [Column("HistoryId")]
        [Key]

        public override int Id { get; set; }
        public int? OriginalLoanId { get; set; }
        public int BookId { get; set; }
        public int UserId { get; set; }

        public string FinalStatus { get; set; }
        public string Observations { get; set; }

        public Books Books { get; set; }
        public User User { get; set; }
    }
}
