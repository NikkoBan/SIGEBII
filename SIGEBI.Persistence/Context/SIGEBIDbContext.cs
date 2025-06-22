using Microsoft.EntityFrameworkCore;
using SIGEBI.Domain.Entities;
using System.Threading.Tasks;

namespace SIGEBI.Persistence.Context
{
    public class SIGEBIDbContext : DbContext
    {
        public SIGEBIDbContext(DbContextOptions<SIGEBIDbContext> options) : base(options)
        {

        }
        public DbSet<Authors> Authors { get; set; }
        public DbSet<BookAuthor> BookAuthors { get; set; }
        public DbSet<Books> Books { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<Loans> Loans { get; set; }
        public DbSet<LoanEntity> LoanEntities { get; set; }
        public DbSet<LoanHistory> LoanHistories { get; set; }
        public DbSet<Publishers> Publishers { get; set; }
        public DbSet<Reservations> Reservations { get; set; }
        public DbSet<ReservationHistory> ReservationHistories { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Sanctions> Sanctions { get; set; }
        public DbSet<SpecialMaterials> SpecialMaterials { get; set; }
        public DbSet<Users> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BookAuthor>()
                .HasKey(ba => new { ba.ID, ba.AuthorId });

            modelBuilder.Entity<Loans>()
                .Property(l => l.LoanStatus)
                .HasMaxLength(50);

            modelBuilder.Entity<LoanHistory>()
                .Property(h => h.FinalStatus)
                .HasMaxLength(50);
        }
    }
}
