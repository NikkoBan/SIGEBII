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
    public class UserLogin : BaseEntity<int>
    {
        [Column("LoginID")]
        [Key]
        public override int ID { get; set; }
        public int UserId { get; set; }
        public string EnteredEmail { get; set; } = string.Empty;
        public DateTime AttemptDate { get; set; }
        public string IpAddress { get; set; } = string.Empty;
        public string UserAgent { get; set; } = string.Empty;
        public bool IsSuccessful { get; set; }
        public string? FailureReason { get; set; }
        public string? ObtainedRole { get; set; }
        public User? User { get; set; }
    }
}
