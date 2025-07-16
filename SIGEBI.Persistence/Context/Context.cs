using Microsoft.EntityFrameworkCore;
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
            public DbSet<BookAuthor> BookAuthor { get; set; }
    }
}