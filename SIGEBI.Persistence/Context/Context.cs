using Microsoft.EntityFrameworkCore;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Entities.Configuration;

namespace SIGEBI.Persistence.Context
{
    public class SIGEBIContext : DbContext
    {
        public SIGEBIContext(DbContextOptions<SIGEBIContext> options) : base(options)
        {
        }

        public DbSet<Authors> Authors { get; set; }
        public DbSet<Books> Books { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<BooksAuthors> BookAuthors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BooksAuthors>()
                .HasKey(ba => new { ba.BookId, ba.AuthorId }); // Asumiendo que BookId y AuthorId son las propiedades de la clave compuesta

            modelBuilder.Entity<BooksAuthors>()
                .HasOne(ba => ba.Books) 
                .WithMany(b => b.BooksAuthors) 
                .HasForeignKey(ba => ba.BookId);

            modelBuilder.Entity<BooksAuthors>()
                .HasOne(ba => ba.Authors) // Asumiendo que BooksAuthors tiene una propiedad de navegación 'Author'
                .WithMany(a => a.BooksAuthors) // Asumiendo que Authors tiene una colección 'BooksAuthors'
                .HasForeignKey(ba => ba.AuthorId);

            base.OnModelCreating(modelBuilder);


        }

    }
}