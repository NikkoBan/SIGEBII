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
    public class BookAuthor : BaseEntity<int>
    {
        [Column("BookAuthorID")]
        [Key]
        public override int ID { get; set; }
        public int AuthorId { get; set; }
        public Book? Book { get; set; }
        public Author? Author { get; set; }
    }
}