
using SIGEBI.Domain.Base;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SIGEBI.Domain.Entities.Configuration
{
    public class Sanction : BaseEntity<int>
    {
        [Column("SanctionId")]
        [Key]
        public override int Id { get; set; }

        public int UserId { get; set; }
        public int? LoanId { get; set; } 
        public required string SanctionType { get; set; }
        public decimal? FineAmount { get; set; }
        public DateTime ImpositionDate { get; set; } = DateTime.Now;
        public DateTime? DueDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public required string Reason { get; set; }
        public required string SanctionStatus { get; set; }

       
        public User? Users { get; set; }
        public Loans ? Loans { get; set; }
    }
}
