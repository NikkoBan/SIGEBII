using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIGEBI.Domain.Base;

namespace SIGEBI.Domain.Entities
{
    public class LoanDetails : AuditEntity<int>
    {
        public override int ID { get; set; }
    
        public int LoanId { get; set; }
        public int BookId { get; set; }
    }
}
