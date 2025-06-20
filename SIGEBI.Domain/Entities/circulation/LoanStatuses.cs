using SIGEBI.Domain.Base;

namespace SIGEBI.Domain.Entities.circulation
{
    public class LoanStatuses : AuditEntity<int>
    {
        public override int Id { get; set; }
        public string Description { get; set; }
    }
}
