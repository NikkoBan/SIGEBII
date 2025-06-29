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
    public class Publishers : AuditEntity<int>
    {

        [Key]
        [Column("PublisherId")]
        public override int ID { get; set; }
        public string PublisherName { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Website { get; set; }
        public ICollection<Books>? Books { get; set; } = new List<Books>();
    }
}

