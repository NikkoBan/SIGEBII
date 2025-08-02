using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

public class Book : BaseEntity<int>
{
    [Column("BookID")]
    [Key]
    public override int ID { get; set; }

    public string Title { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public DateTime? PublicationDate { get; set; }
    public int CategoryId { get; set; }
    public int PublisherId { get; set; }
    public string Language { get; set; } = string.Empty;
    public string? Summary { get; set; }
    public int TotalCopies { get; set; }
    public int AvailableCopies { get; set; }
    public string GeneralStatus { get; set; } = string.Empty;
    public bool Borrado { get; set; } = false;

    [ForeignKey("CategoryId")]
    public virtual Category? Category { get; set; }

    [ForeignKey("PublisherId")]
    public virtual Publisher? Publisher { get; set; }

    public virtual ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
    public virtual ICollection<Loan> Loans { get; set; } = new List<Loan>();
    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    [JsonIgnore]
    public virtual ICollection<Loan>? Loan { get; set; }
    [NotMapped]
    public int Stock => AvailableCopies;

}
