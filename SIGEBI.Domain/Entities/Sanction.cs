using SIGEBI.Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Domain.Entities
{
    public class Sanction : BaseEntity<int>
    {
        [Column("SanctionID")]
        [Key]
        public override int ID { get; set; }
        public int UserId { get; set; }
        public int? LoanId { get; set; }
        public string SanctionType { get; set; } = string.Empty;
        public decimal? FineAmount { get; set; }
        public DateTime ImpositionDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string SanctionStatus { get; set; } = string.Empty;
        public User? User { get; set; }
        public Loan? Loan { get; set; }
    }
}