using SIGEBI.Domain.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SIGEBI.Domain.Entities
{
    public class LoanHistory : BaseEntity<int>
    {
        [Column("HistoryID")]
        [Key]
        public override int ID { get; set; }

        public int LoanId { get; set; }
        public int OriginalLoanId { get; set; }
        public int BookId { get; set; }
        public int UserId { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string FinalStatus { get; set; } = string.Empty;
        public string? Observations { get; set; }

        public Loan? Loan { get; set; }
        [JsonIgnore]

        public virtual Loan? loan { get; set; }
    }
}
