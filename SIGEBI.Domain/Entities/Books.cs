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
    public class Books : AuditEntity<int>
    {
        [Key]
        [Column("BookId")]
        public override int ID { get; set; }

        public required string Title { get; set; }
        public required string ISBN { get; set; }
        public DateTime PublicationDate { get; set; }
        public int CategoryId { get; set; }


        [ForeignKey("Publisher")]
        public int PublisherId { get; set; }

        public string? Language { get; set; }
        public string? Summary { get; set; }
        public int TotalCopies { get; set; }
        public int AvailableCopies { get; set; }
        public string? GeneralStatus { get; set; }
        
        public virtual Publishers? Publisher { get; set; }
       
    }
}
