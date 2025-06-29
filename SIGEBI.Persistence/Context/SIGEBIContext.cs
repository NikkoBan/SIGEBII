using Microsoft.EntityFrameworkCore;
using SIGEBI.Domain.Entities.circulation;
using SIGEBI.Application.DTOs;

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
        public DbSet<ReservationDto> ReservationsView { get; set; } // This is a view model, not an entity.
        public DbSet<ReservationStatusDto> ReservationStatusesView { get; set; }
        public DbSet<ReservationHistoryDto> ReservationsHistoryView { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configure entity properties, relationships, etc. here if needed
            modelBuilder.Entity<Reservation>()
                .HasKey(r => r.Id);
            modelBuilder.Entity<ReservationStatus>()
                .HasKey(rs => rs.Id);
            modelBuilder.Entity<ReservationHistory>()
                .HasKey(rh => rh.HistoryId);
            modelBuilder.Entity<ReservationDto>()
                .HasNoKey().ToView(null); // This is a view model, not an entity
            modelBuilder.Entity<ReservationStatusDto>()
                .HasNoKey().ToView(null); // This is a view model, not an entity
            modelBuilder.Entity<ReservationHistoryDto>()
                .HasNoKey().ToView(null); // This is a view model, not an entity
        }
    }
}
