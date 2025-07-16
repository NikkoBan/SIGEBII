using SIGEBI.Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Domain.Entities.Configuration
{
    public class LoanStatues : BaseEntity<int>
    {
       
        public override int Id { get; set; }

        public required string StatusName { get; set; }
        public required string Description { get; set; }

        public ICollection<Loans> Loans { get; set; } = new List<Loans>();

    }
}
