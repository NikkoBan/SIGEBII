using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIGEBI.Domain.Base;

namespace SIGEBI.Domain.Entities
{
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
        public Category? Category { get; set; }
        public Publisher? Publisher { get; set; }
        public ICollection<BookAuthor>? BookAuthors { get; set; }
        public ICollection<Loan>? Loans { get; set; }
        public ICollection<Reservation>? Reservations { get; set; }
    }
}
