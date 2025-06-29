using Microsoft.EntityFrameworkCore;
using SIGEBI.Domain.Entities;
using System.Threading.Tasks;

namespace SIGEBI.Persistence.Context
{
    public class SIGEBIDbContext : DbContext
    {
        public SIGEBIDbContext(DbContextOptions<SIGEBIDbContext> options) : base(options) { }

        public DbSet<Publishers> Publishers { get; set; }
        public DbSet<Books> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Publishers>(entity =>
            {
                entity.HasKey(p => p.ID);
                entity.Property(p => p.PublisherName).IsRequired().HasMaxLength(255);
                entity.Property(p => p.Address).HasMaxLength(255);
                entity.Property(p => p.PhoneNumber).HasMaxLength(20);
                entity.Property(p => p.Email).HasMaxLength(255);
                entity.Property(p => p.Website).HasMaxLength(255);
                entity.Ignore(p => p.Notes); 
            });

            modelBuilder.Entity<Books>(entity =>
            {
                entity.HasKey(b => b.ID);
                entity.Property(b => b.Title).IsRequired().HasMaxLength(255);
                entity.Property(b => b.ISBN).IsRequired().HasMaxLength(13);
                entity.Property(b => b.Language).HasMaxLength(50);
                entity.Property(b => b.GeneralStatus).IsRequired().HasMaxLength(50);
                entity.HasOne(b => b.Publisher)
                      .WithMany(p => p.Books)
                      .HasForeignKey(b => b.PublisherId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.Ignore(b => b.Notes); 
            });
        }
    }

}
