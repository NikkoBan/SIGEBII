using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class BookAuthor : BaseEntity<int>
{
    [Column("BookAuthorID")]
    [Key]
    public override int ID { get; set; }

    public int AuthorId { get; set; }
    public int? BookId { get; set; }

    [ForeignKey("AuthorId")]
    public virtual Author? Author { get; set; }

    [ForeignKey("BookId")]
    public virtual Book? Book { get; set; }
}
