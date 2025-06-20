using SIGEBI.Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Domain.Entities.Configuration
{
    public class UserHistory : BaseEntity<int>
    {
        [Column("LogId")]
        [Key]
        public override int Id { get; set; }

        public int UserId { get; set; }
        public string EnteredEmail { get; set; }
        public DateTime AttemptDate { get; set; } = DateTime.Now;
        public string IpAddress { get; set; }
        public string UserAgent { get; set; }
        public bool IsSuccessful { get; set; }
        public string FailureReason { get; set; }
        public string ObtainedRole { get; set; }

        public User User { get; set; }
    }
}
