using System;

namespace SIGEBI.Domain.Entities
{
    public class Loan
    {
        public int LoanId { get; set; }

        // Clave foránea a User
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        // Clave foránea a Book
        public int BookId { get; set; }
        public Book Book { get; set; } = null!;

        // Clave foránea a LoanStatus
        public int LoanStatusId { get; set; }
        public LoanStatus LoanStatus { get; set; } = null!;

        public DateTime LoanDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = null!;
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }
        public ICollection<LoanDetail> LoanDetails { get; set; } = new List<LoanDetail>();
        public ICollection<LoanHistory> History { get; set; } = new List<LoanHistory>();

    }
}
