using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIGEBI.Domain.Base;

namespace SIGEBI.Domain.Entities
{
    public class Loans : AuditEntity<int>
    {

        public override int ID { get; set; }
        public int BookId { get; set; }
        public int UserId { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public required string LoanStatus { get; set; }
        public virtual Books? Book { get; set; }
        public virtual Users? User { get; set; }

        public virtual ICollection<LoanHistory> LoanHistories { get; set; } = new List<LoanHistory>();
    }
}
