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
       
        public override int Id { get; set; }

        public int UserId { get; set; }
        public required string EnteredEmail { get; set; }
        public DateTime AttemptDate { get; set; } = DateTime.Now;
        public required string IpAddress { get; set; }
        public required string UserAgent { get; set; }
        public  bool IsSuccessful { get; set; }
        public required string FailureReason { get; set; }
        public required string ObtainedRole { get; set; }

        public User? User { get; set; }
    }
}
