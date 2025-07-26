using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

public class Loan : BaseEntity<int>
{
    [Column("LoanID")]
    [Key]
    public override int ID { get; set; }

    public int BookId { get; set; }
    public int UserId { get; set; }

    public DateTime LoanDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public string LoanStatus { get; set; } = string.Empty;
    public bool Borrado { get; set; } = false;

    [ForeignKey("BookId")]
    public virtual Book? Book { get; set; }

    [ForeignKey("UserId")]
    public virtual User? User { get; set; }

    public virtual ICollection<LoanHistory>? LoanHistories { get; set; }
}
