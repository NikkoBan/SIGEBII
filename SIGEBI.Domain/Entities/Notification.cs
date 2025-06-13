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
    public class Notification : BaseEntity<int>
    {
        [Column("UserID")]
        [Key]
        public override int ID { get; set; }
        public string NotificationType { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime SentDate { get; set; }
        public bool IsRead { get; set; }
        public DateTime? ReadDate { get; set; }
        public User? User { get; set; }
    }

}