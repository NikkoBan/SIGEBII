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
    public class BookAuthor : BaseEntity<int>
    {
        [Column("BookID")]
        [Key]
        public override int ID { get; set; }
        public int AuthorId { get; set; }
        public Book? Book { get; set; }
        public Author? Author { get; set; }
    }

}
