using SIGEBI.Domain.Entities.Configuration.SIGEBI.Domain.Base;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SIGEBI.Domain.Entities.Configuration
{
    public class ReservationHistory : ReservationBase
    {
        [Column("ReservationHistoryId")]
        [Key]
        public override int Id { get; set; }

        public int? OriginalReservationId { get; set; } 
        public string FinalStatus { get; set; } 
        public DateTime UpdateDate { get; set; } = DateTime.Now;

        public Books Book { get; set; }
        public User User { get; set; }
    }
}
