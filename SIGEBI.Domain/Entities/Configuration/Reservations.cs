

using SIGEBI.Domain.Entities.Configuration.SIGEBI.Domain.Base;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SIGEBI.Domain.Entities.Configuration
{
    public class Reservation : ReservationBase
    {
        [Column("ReservationId")]
        [Key]
        public override int Id { get; set; }

        public int BookId { get; set; }
        public int UserId { get; set; }
        public int ReservationStatusId { get; set; }

        public Books Book { get; set; }
        public User User { get; set; }
        public ReservationStatuses ReservationStatuses { get; set; }
    }

}
