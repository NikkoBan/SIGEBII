
using SIGEBI.Domain.Base;

namespace SIGEBI.Domain.Entities.Configuration
{
    namespace SIGEBI.Domain.Base
    {
        public abstract class ReservationBase : BaseEntity<int>
        {
            public DateTime ReservationDate { get; set; } = DateTime.Now;
            public DateTime ExpirationDate { get; set; }
        }
    }

}
