using SIGEBI.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Domain.Entities
{
    public class LoanEntity : BaseEntity<int>
    {
        public override int ID { get; set; }
    }
}
