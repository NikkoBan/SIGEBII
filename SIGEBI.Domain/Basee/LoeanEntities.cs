
using SIGEBI.Persistence.Base;

namespace SIGEBI.Persistence
{
    public abstract class LoanEntity : BaseEntity <int>
    {
       public override int ID { get; set; }
    }
}
