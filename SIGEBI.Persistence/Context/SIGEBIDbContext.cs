using Microsoft.EntityFrameworkCore;
using SIGEBI.Domain.Entities;

namespace SIGEBI.Persistence.Context
{
    public class SIGEBIDbContext : DbContext
    {
        public SIGEBIDbContext(DbContextOptions<SIGEBIDbContext> options) : base(options)
        {
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<BookAuthor> BookAuthors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<LoanHistory> LoanHistories { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<ReservationHistory> ReservationHistories { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Sanction> Sanctions { get; set; }
        public DbSet<SpecialMaterial> SpecialMaterials { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserLogin> UserLogins { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<BookAuthor>()
                .HasKey(ba => new { ba.ID, ba.AuthorId });

            modelBuilder.Entity<BookAuthor>()
                .HasOne(ba => ba.Book)
                .WithMany(b => b.BookAuthors)
                .HasForeignKey(ba => ba.BookId);

            modelBuilder.Entity<BookAuthor>()
                .HasOne(ba => ba.Author)
                .WithMany()
                .HasForeignKey(ba => ba.AuthorId);


            modelBuilder.Entity<Loan>()
                .Property(l => l.LoanStatus)
                .HasMaxLength(50);

            modelBuilder.Entity<Loan>()
                .HasOne(l => l.Book)
                .WithMany(b => b.Loans)
                .HasForeignKey(l => l.BookId);

            modelBuilder.Entity<Loan>()
                .HasOne(l => l.User)
                .WithMany()
                .HasForeignKey(l => l.UserId);


            modelBuilder.Entity<LoanHistory>()
                .Property(h => h.FinalStatus)
                .HasMaxLength(50);

            modelBuilder.Entity<LoanHistory>()
                .HasOne(lh => lh.Loan)
                .WithMany(l => l.LoanHistories)
                .HasForeignKey(lh => lh.LoanId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SpecialMaterial>()
                .HasOne(sm => sm.RequestedByUser)
                .WithMany(u => u.RequestedSpecialMaterials)
                .HasForeignKey(sm => sm.RequestedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SpecialMaterial>()
                .HasOne(sm => sm.ApprovedByUser)
                .WithMany(u => u.ApprovedSpecialMaterials)
                .HasForeignKey(sm => sm.ApprovedByUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
