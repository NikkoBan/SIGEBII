

namespace SIGEBI.Domain.Base
{
    public abstract class BaseEntity<Ttype> : AuditableEntity
    {
        public abstract Ttype Id { get; set; }
    }
}
