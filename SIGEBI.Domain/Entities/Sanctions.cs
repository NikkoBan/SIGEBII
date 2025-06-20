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
    public class Sanctions : AuditEntity<int>
    {
       
        public override int ID { get; set; }
        public int UserId { get; set; }
        public int LoanId { get; set; }
        public required string SanctionType { get; set; }
        public decimal FineAmount { get; set; }
        public DateTime ImpositionDate { get; set; }

        public DateTime? DueDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public required string Reason { get; set; }
        public required string SanctionStatus { get; set; }

        public required virtual Users User { get; set; }
    }
}
