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
    public  class LoanDetails : Base.BaseEntity<int>
    {
        
            [Column("LoandDetaild")]
            [Key]
            public override int Id { get; set; }
            
        public int LoandId { get; set; }
        public Loans Loans { get; set; }

        public int BookId { get; set; }
        public Books Books { get; set; }



    }
}
