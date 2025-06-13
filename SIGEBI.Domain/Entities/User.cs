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
    public class User : BaseEntity<int>
    {
        [Column("UserID")]
        [Key]
        public override int ID { get; set; }
        public string FullName { get; set; }
        public string InstitutionalEmail { get; set; }
        public string PasswordHash { get; set; }
        public string? InstitutionalIdentifier { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int RoleId { get; set; }
        public bool IsActive { get; set; }
        public Role? Role { get; set; }
        public ICollection<UserLogin>? UserLogins { get; set; }
        public ICollection<Loan>? Loans { get; set; }
        public ICollection<Reservation>? Reservations { get; set; }
        public ICollection<Notification>? Notifications { get; set; }
        public ICollection<Sanction>? Sanctions { get; set; }
        public ICollection<SpecialMaterial>? RequestedSpecialMaterials { get; set; }
        public ICollection<SpecialMaterial>? ApprovedSpecialMaterials { get; set; }
    }
}
