using Microsoft.EntityFrameworkCore;
using SIGEBI.Domain.Entities;
using System;

namespace SIGEBI.Persistence.Context
{
    public class SIGEBIDbContext : DbContext
    {
        public SIGEBIDbContext(DbContextOptions<SIGEBIDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<UserHistory> UserHistories { get; set; } = null!;

        /* Agrega aquí los DbSet para otras entidades si se relacionan con User y están en tu base de datos, por ejemplo:
           public DbSet<Role> Roles { get; set; } = null!;
           public DbSet<Loan> Loans { get; set; } = null!;
           public DbSet<Reservation> Reservations { get; set; } = null!;
           public DbSet<Sanction> Sanctions { get; set; } = null!;
        */

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Mapeo para la entidad User (dbo.Users)
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(u => u.UserId);
                entity.Property(u => u.UserId).ValueGeneratedOnAdd();

                entity.Property(u => u.FullName).HasMaxLength(200).IsRequired();
                entity.Property(u => u.InstitutionalEmail).HasMaxLength(255).IsRequired(); 
                entity.HasIndex(u => u.InstitutionalEmail).IsUnique();

                entity.Property(u => u.PasswordHash).HasMaxLength(255).IsRequired();
                entity.Property(u => u.InstitutionalIdentifier).HasMaxLength(50).IsRequired(false); // Anulable
                entity.HasIndex(u => u.InstitutionalIdentifier).IsUnique();

                entity.Property(u => u.RegistrationDate).IsRequired();
                entity.Property(u => u.RoleId).IsRequired();
                entity.Property(u => u.IsActive).IsRequired();

                entity.Property(u => u.CreatedAt).IsRequired();
                entity.Property(u => u.CreatedBy).HasMaxLength(100).IsRequired();
                entity.Property(u => u.UpdatedAt).IsRequired(false); // Anulable
                entity.Property(u => u.UpdatedBy).HasMaxLength(100).IsRequired(false); // Anulable
                entity.Property(u => u.IsDeleted).IsRequired();
                entity.Property(u => u.DeletedAt).IsRequired(false); // Anulable
                entity.Property(u => u.DeletedBy).HasMaxLength(100).IsRequired(false); // Anulable

                // Configuración de la relación One-to-Many con UserHistory
                // Si borras un usuario, borra su historial.
                // Si la FK en DB es NOT NULL, esto funciona si UserHistory.UserId es NOT NULL.
                entity.HasMany(u => u.UserHistories)
                      .WithOne(uh => uh.User) // La propiedad de navegación inversa en UserHistory
                      .HasForeignKey(uh => uh.UserId)
                      .IsRequired() // UserId en UserHistory es NOT NULL
                      .OnDelete(DeleteBehavior.Cascade); // Si borras User, se borran UserHistories asociados
            });

            // Mapeo para la entidad UserHistory (dbo.UserHistory)
            modelBuilder.Entity<UserHistory>(entity =>
            {
                entity.ToTable("UserHistory");
                entity.HasKey(uh => uh.LogId);
                entity.Property(uh => uh.LogId).ValueGeneratedOnAdd();

                entity.Property(uh => uh.UserId).IsRequired(); // FK, NOT NULL en DB

                entity.Property(uh => uh.EnteredEmail).HasMaxLength(255).IsRequired(false); // Anulable
                entity.Property(uh => uh.AttemptDate).IsRequired();
                entity.Property(uh => uh.IpAddress).HasMaxLength(45).IsRequired(false); // Anulable
                entity.Property(uh => uh.UserAgent).HasColumnType("nvarchar(max)").IsRequired(false); // Anulable, nvarchar(max)
                entity.Property(uh => uh.IsSuccessful).IsRequired();
                entity.Property(uh => uh.FailureReason).HasMaxLength(100).IsRequired(false); // Anulable
                entity.Property(uh => uh.ObtainedRole).HasMaxLength(50).IsRequired(false); // Anulable

                entity.Property(uh => uh.CreatedAt).IsRequired();
                entity.Property(uh => uh.CreatedBy).HasMaxLength(100).IsRequired();
                entity.Property(uh => uh.UpdatedAt).IsRequired(false);
                entity.Property(uh => uh.UpdatedBy).HasMaxLength(100).IsRequired(false);
                entity.Property(uh => uh.IsDeleted).IsRequired();
                entity.Property(uh => uh.DeletedAt).IsRequired(false);
                entity.Property(uh => uh.DeletedBy).HasMaxLength(100).IsRequired(false);

                // Configuración de la relación con User (vista desde UserHistory)
                entity.HasOne(uh => uh.User)
                      .WithMany(u => u.UserHistories)
                      .HasForeignKey(uh => uh.UserId)
                      .IsRequired() // Coincide con NOT NULL en DB
                      .OnDelete(DeleteBehavior.Restrict); // Evita borrado en cascada desde UserHistory si borras User.
            });
            /* Agrega aquí mapeos para otras entidades si las necesitas en este contexto, por ejemplo:
             * modelBuilder.Entity<Role>(entity => { ... });
            */
        }
    }
}