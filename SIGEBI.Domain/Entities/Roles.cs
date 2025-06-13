using SIGEBI.Domain.Base;

namespace SIGEBI.Domain.Entities
{
    public class Roles : AuditEntity<int>
    {
        public override int Id { get; set; }
        public required string RoleName { get; set; } 
        public required string Description { get; set; } 

    }
}
