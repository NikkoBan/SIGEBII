using System;

namespace SIGEBI.Domain.Entities
{
    public class LoanDetail
    {
        public int LoanDetailId { get; set; }

        // Relación con Loan
        public int LoanId { get; set; }
        public Loan Loan { get; set; } = null!;

        // Relación con Book
        public int BookId { get; set; }
        public Book Book { get; set; } = null!;
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = null!;
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }
    }
}
