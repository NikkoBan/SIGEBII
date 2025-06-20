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
    public class Authors : AuditEntity<int>
    {
        
        public override int ID { get; set; }
        public required string FirstName { get; set; }
        public required string LastNameId { get; set; }
        public DateOnly BirthDate { get; set; }
        public string? Nationality { get; set; }

        public virtual ICollection<BookAuthor>? BooksAuthors { get; set; } = new List<BookAuthor>();
    }
}