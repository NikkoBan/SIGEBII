using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIGEBI.Domain.Base;
using static System.Reflection.Metadata.BlobBuilder;

namespace SIGEBI.Domain.Entities
{
    public class BookAuthor : AuditEntity<int>
    {
        
        public int BookId { get; set; }
        public int AuthorId { get; set; }
        public virtual Books? Book { get; set; }
        public virtual Authors? Author { get; set; }
    }

}
