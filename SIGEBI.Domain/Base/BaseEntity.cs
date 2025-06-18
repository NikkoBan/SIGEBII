using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Domain.Base
{
   public abstract class BaseEntity<Ttype>
    {
        public abstract Ttype ID { get; set; }
    }
}
