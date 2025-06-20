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
    public class SpecialMaterials : AuditEntity<int>
    {
        
        public override int ID { get; set; }
        public required string Tittle { get; set; }
        public required string Author { get; set; }
        public string? Description { get; set; }
        public DateTime RequestDate { get; set; }
        public string? ApprovalStatus { get; set; }
        public DateTime AdquisitionDate { get; set; }
        public int? ApprovedByUserId { get; set; }
        public int RequestedByUserId { get; set; }
    }
}
