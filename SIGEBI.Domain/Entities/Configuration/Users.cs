
using SIGEBI.Domain.Base;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SIGEBI.Domain.Entities.Configuration
{
    public class User : BaseEntity<int>
    {
        [Column("UserId")]
        [Key]
        public override int Id { get; set; }

        public string FullName { get; set; }
        public string InstitutionalEmail { get; set; }
        public string PasswordHash { get; set; }
        public string InstitutionalIdentifier { get; set; }
        public DateTime RegistrationDate { get; set; } = DateTime.Now;
        public int RoleId { get; set; }
        public bool IsActive { get; set; } = true;


        public Role Role { get; set; }
        public ICollection<Loans> Loans { get; set; }
        public ICollection<Reservation> Reservations { get; set; }
        public ICollection<Sanction> Sanctions { get; set; }

    }
}
