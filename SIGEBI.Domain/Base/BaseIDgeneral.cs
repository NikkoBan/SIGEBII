
namespace SIGEBI.Domain.Base
{
    public abstract class BaseEntity<Ttype>
    {
        public abstract Ttype ID { get; set; }
    }
}
