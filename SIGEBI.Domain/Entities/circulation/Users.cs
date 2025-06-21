using SIGEBI.Domain.Base;

namespace SIGEBI.Domain.Entities.circulation
{
    public class Users : AuditEntity<int>
    {
        public override int Id { get; set; }
        public required string FullName { get; set; }
        public required string InstitutionalEmail { get; set; }
        public required string PasswordHash { get; set; } 
        public required string InstitutionalIdentifier { get; set; }
        public required string RegistrationDate { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<Loans>? Loans { get; set; } = new List<Loans>();
        public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    }
}
