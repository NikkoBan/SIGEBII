using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Domain.Entities
{
    public class SpecialMaterial
    {
        public int MaterialId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Author { get; set; }
        public string? Description { get; set; }
        public DateTime RequestDate { get; set; }
        public int RequestedByUserId { get; set; }
        public string ApprovalStatus { get; set; } = string.Empty;
        public int? ApprovedByUserId { get; set; }
        public DateTime? AcquisitionDate { get; set; }
        public User? RequestedByUser { get; set; }
        public User? ApprovedByUser { get; set; }
    }
}
