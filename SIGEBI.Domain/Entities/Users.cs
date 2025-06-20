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
    public class Users : AuditEntity<int>
    {
        
        public override int ID { get; set; }
        public required string FullName { get; set; }
        public required string InstitutionalEmail { get; set; }
        public required string PasswordHash { get; set; }
        public required string InstitutionalIdentifier { get; set; }
        public required string RegistrationDate { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<Loans>? Loans { get; set; } = new List<Loans>();
        public virtual ICollection<Reservations> Reservations { get; set; } = new List<Reservations>();
    }
}
