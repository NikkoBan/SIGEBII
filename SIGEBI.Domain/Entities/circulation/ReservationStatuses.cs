using SIGEBI.Domain.Base;

namespace SIGEBI.Domain.Entities.circulation
{
    public class ReservationStatuses : AuditEntity<int>
    {
        public override int Id {get; set; } 
        public string StatusName { get; set; } 
        public string Description { get; set; }
        
    }
}
