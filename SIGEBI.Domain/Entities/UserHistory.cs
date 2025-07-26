using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIGEBI.Domain.Base;

namespace SIGEBI.Domain.Entities
{
    public class UserHistory : AuditEntity<int>
    {
        public override int ID { get; set; }
        public int UserId { get; set; }
        public string? EnteredEmail { get; set; }
        public DateTime AttempDate { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public bool IsSuccessful { get; set; }
        public string? FailureReason { get; set; }
        public string? ObteinedRole { get; set; }
    }
}
