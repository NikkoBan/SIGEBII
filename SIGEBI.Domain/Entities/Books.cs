using SIGEBI.Domain.Base;

namespace SIGEBI.Domain.Entities
{
    public class Books : AuditEntity<int>
    {
        public override int Id { get; set; }

        public required string Title { get; set; }
        public required string ISBN { get; set; }
        public DateTime PublicationDate { get; set; }
        public int CategoryId { get; set; }
        public int PublisherId { get; set; }
        public string? Language { get; set; }
        public string? Summary { get; set; }
        public int TotalCopies { get; set; }
        public int AvailableCopies { get; set; }
        public string? GeneralStatus { get; set; } 
        public virtual Authors? Author { get; set; }
        public virtual Categories? Category { get; set; }
        public virtual Publishers? Publisher { get; set; }
        public virtual ICollection<BookAuthor> BooksAuthors { get; set; } = new List<BookAuthor>();
        public virtual ICollection<Loans> Loans { get; set; } = new List<Loans>();

    }
}
