using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Domain.Entities
{
    

    public class User
    {
        public int UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string InstitutionalEmail { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string? InstitutionalIdentifier { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int RoleId { get; set; }
        public bool IsActive { get; set; }
        public Role? Role { get; set; }
        public ICollection<UserLogin>? UserLogins { get; set; }
        public ICollection<LoginHistory>? LoginHistories { get; set; }
        public ICollection<Loan>? Loans { get; set; }
        public ICollection<Reservation>? Reservations { get; set; }
        public ICollection<Notification>? Notifications { get; set; }
        public ICollection<Sanction>? Sanctions { get; set; }
        public ICollection<SpecialMaterial>? RequestedSpecialMaterials { get; set; }
        public ICollection<SpecialMaterial>? ApprovedSpecialMaterials { get; set; }
    }
}
