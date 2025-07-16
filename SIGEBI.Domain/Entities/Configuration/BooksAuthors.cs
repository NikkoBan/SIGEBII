

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using SIGEBI.Domain.Base;
namespace SIGEBI.Domain.Entities.Configuration
{
    [Table("BookAuthor")]
    [PrimaryKey(nameof(BookId), nameof(AuthorId))]
    public class BookAuthor : AuditableEntity
    {
        //[Key]
        [Column(Order = 0)]
        public int BookId { get; set; }

        //[Key]
        [Column(Order = 1)]
        public int AuthorId { get; set; }

        [ForeignKey("BookId")]
        public Books? Book { get; set; }

        [ForeignKey("AuthorId")]
        public Authors? Author { get; set; }
    }
}
