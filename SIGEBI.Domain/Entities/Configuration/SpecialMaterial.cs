
using SIGEBI.Domain.Base;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SIGEBI.Domain.Entities.Configuration
{
    public class SpecialMaterial : BaseEntity<int>
    {
        [Column("MatetialId")]
        [Key]
        public override int Id { get; set; }

        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public DateTime RequestDate { get; set; } = DateTime.Now;
        public int RequestedByUserId { get; set; }
        public string ApprovalStatus { get; set; } 
        public int? ApprovedByUserId { get; set; }
        public DateTime? AcquisitionDate { get; set; }

        
        public User RequestedByUser { get; set; }
        public User ApprovedByUser { get; set; }
    }
}
