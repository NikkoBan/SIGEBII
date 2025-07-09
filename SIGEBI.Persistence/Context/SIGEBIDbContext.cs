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

        /* Agrega aquí los DbSet para otras entidades si se relacionan con User
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
                entity.Property(u => u.InstitutionalIdentifier).HasMaxLength(50).IsRequired(false);
                entity.HasIndex(u => u.InstitutionalIdentifier).IsUnique();

                entity.Property(u => u.RegistrationDate).IsRequired();
                entity.Property(u => u.RoleId).IsRequired();
                entity.Property(u => u.IsActive).IsRequired();

                entity.Property(u => u.CreatedAt).IsRequired();
                entity.Property(u => u.CreatedBy).HasMaxLength(100).IsRequired();
                entity.Property(u => u.UpdatedAt).IsRequired(false);
                entity.Property(u => u.UpdatedBy).HasMaxLength(100).IsRequired(false);
                entity.Property(u => u.IsDeleted).IsRequired();
                entity.Property(u => u.DeletedAt).IsRequired(false);
                entity.Property(u => u.DeletedBy).HasMaxLength(100).IsRequired(false);

                entity.HasMany(u => u.UserHistories)
                      .WithOne(uh => uh.User)
                      .HasForeignKey(uh => uh.UserId)
                      .IsRequired()
                      .OnDelete(DeleteBehavior.Cascade);

                /* Otras relaciones de User si existen */
            });

            // Mapeo para la entidad UserHistory (dbo.UserHistory)
            modelBuilder.Entity<UserHistory>(entity =>
            {
                entity.ToTable("UserHistory");
                entity.HasKey(uh => uh.LogId);
                entity.Property(uh => uh.LogId).ValueGeneratedOnAdd();

                entity.Property(uh => uh.UserId).IsRequired();

                entity.Property(uh => uh.EnteredEmail).HasMaxLength(255).IsRequired(false);
                entity.Property(uh => uh.AttemptDate).IsRequired();
                entity.Property(uh => uh.IpAddress).HasMaxLength(45).IsRequired(false);
                entity.Property(uh => uh.UserAgent).HasColumnType("nvarchar(max)").IsRequired(false);
                entity.Property(uh => uh.IsSuccessful).IsRequired();
                entity.Property(uh => uh.FailureReason).HasMaxLength(100).IsRequired(false);
                entity.Property(uh => uh.ObtainedRole).HasMaxLength(50).IsRequired(false);

                entity.Property(uh => uh.CreatedAt).IsRequired();
                entity.Property(uh => uh.CreatedBy).HasMaxLength(100).IsRequired();
                entity.Property(uh => uh.UpdatedAt).IsRequired(false);
                entity.Property(uh => uh.UpdatedBy).HasMaxLength(100).IsRequired(false);
                entity.Property(uh => uh.IsDeleted).IsRequired();
                entity.Property(uh => uh.DeletedAt).IsRequired(false);
                entity.Property(uh => uh.DeletedBy).HasMaxLength(100).IsRequired(false);

                entity.HasOne(uh => uh.User)
                      .WithMany(u => u.UserHistories)
                      .HasForeignKey(uh => uh.UserId)
                      .IsRequired()
                      .OnDelete(DeleteBehavior.Restrict);
            });
            /* Agrega aquí mapeos para otras entidades
             * modelBuilder.Entity<Role>(entity => { ... });
            */
        }
    }
}