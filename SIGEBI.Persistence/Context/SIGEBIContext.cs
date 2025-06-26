using Microsoft.EntityFrameworkCore;
using SIGEBI.Domain.Entities.circulation;

namespace SIGEBI.Persistence.Context
{
    public class SIGEBIContext : DbContext
    {
        public SIGEBIContext(DbContextOptions<SIGEBIContext> options) : base(options)
        {
        }

        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<ReservationStatus> ReservationStatuses { get; set; }
        public DbSet<ReservationHistory> ReservationHistory { get; set; } // kinda confusing, but this is the history of reservations concerning to the 'ReservationHistory' entity.


    }
}
