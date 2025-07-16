using SIGEBI.Domain.Base;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace SIGEBI.Domain.Entities.Configuration
{
    public class ReservationStatuses : BaseEntity<int>

    {
        
        public override int Id { get ; set ; }
        public required string StatusName { get; set; } 
        public  required string Description { get; set; } 

    }
}
