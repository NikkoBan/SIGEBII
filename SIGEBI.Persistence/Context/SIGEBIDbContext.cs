using Microsoft.EntityFrameworkCore;
using SIGEBI.Domain.Entities;

namespace SIGEBI.Persistence.Context
{
    public class SIGEBIDbContext : DbContext
    {
        public SIGEBIDbContext(DbContextOptions<SIGEBIDbContext> options) : base(options)
        {
        }

        // Definición de DbSet para User y UserHistory
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<UserHistory> UserHistories { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuraciones específicas para User y UserHistory
            // Por ejemplo, para asegurar unicidad del email
            modelBuilder.Entity<User>().HasIndex(u => u.InstitutionalEmail).IsUnique();

            /*
                 // Configuraciones de claves primarias si no se siguen las convenciones (ej. 'Id' o 'UserHistoryId')
                  modelBuilder.Entity<User>().HasKey(u => u.UserId);
                  modelBuilder.Entity<UserHistory>().HasKey(uh => uh.UserHistoryId);

              // Configuración de la relación entre User y UserHistory si es necesaria y no se infiere automáticamente
                 modelBuilder.Entity<UserHistory>()
                .HasOne<User>()
                 .WithMany()
                 .HasForeignKey(uh => uh.UserId);
             */
        }
    }
}
