﻿using Microsoft.EntityFrameworkCore;
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
<<<<<<< HEAD
        public DbSet<ReservationHistory> ReservationHistory { get; set; } // kinda confusing, but this is the history of reservations concerning to the 'ReservationHistory' entity.
        public DbSet<ReservationViewDto> ReservationsView { get; set; } // This is a view model, not an entity.
        public DbSet<ReservationStatusViewDto> ReservationStatusesView { get; set; } 
        public DbSet<ReservationHistoryViewDto> ReservationsHistoryView { get; set; } 
=======
        public DbSet<ReservationHistory> ReservationHistory { get; set; }
>>>>>>> c9a5f6 (Fix: cambios en capa de persistencia)

        protected override void OnModelCreating(ModelBuilder modelBuilder) // Aqui tuve que agregarlo ya que mi base de datos tiene los nombres de tablas en plural...
        {
<<<<<<< HEAD
            base.OnModelCreating(modelBuilder);
            // Configure entity properties, relationships, etc. here if needed
            modelBuilder.Entity<Reservation>()
                .HasKey(r => r.Id);
            modelBuilder.Entity<ReservationStatus>()
                .HasKey(rs => rs.Id);
            modelBuilder.Entity<ReservationHistory>()
                .HasKey(rh => rh.HistoryId);
            modelBuilder.Entity<ReservationViewDto>()
                .HasNoKey().ToView(null); // This is a view model, not an entity
            modelBuilder.Entity<ReservationStatusViewDto>()
                .HasNoKey().ToView(null); // This is a view model, not an entity
            modelBuilder.Entity<ReservationHistoryViewDto>()
                .HasNoKey().ToView(null); // This is a view model, not an entity
=======
            modelBuilder.Entity<Reservation>().ToTable("Reservations");
            modelBuilder.Entity<ReservationStatus>().ToTable("ReservationStatuses");

>>>>>>> c9a5f6 (Fix: cambios en capa de persistencia)
        }

    }

}
