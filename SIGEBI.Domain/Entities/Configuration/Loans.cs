using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Domain.Entities.Configuration
{
    public  class Loans : LoanBase
    {
        [Column("LoandId")]
        [Key]
        public override int Id { get; set; }

        public int BookId { get; set; }
        public int UserId { get; set; }
        public int LoanStatus { get; set; }

        public Books Books { get; set; }
        public User User { get; set; }
        public LoanStatues LoanStatues { get; set; }
    }
}
