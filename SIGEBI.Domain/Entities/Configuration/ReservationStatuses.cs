using SIGEBI.Domain.Base;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace SIGEBI.Domain.Entities.Configuration
{
    public class ReservationStatuses : BaseEntity<int>

    {
        [Column("StatusId")]
        [Key]
        public override int Id { get ; set ; }
        public string StatusName { get; set; } 
        public string Description { get; set; } 

    }
}
