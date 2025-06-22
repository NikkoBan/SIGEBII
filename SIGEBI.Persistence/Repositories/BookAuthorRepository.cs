using Microsoft.Extensions.Logging;
using SIGEBI.Domain.Entities.Configuration;

namespace SIGEBI.Persistence.Repositories
{
    public class BookAuthorRepository : BaseRepository<BooksAuthors>
    {
        public BookAuthorRepository(string connection, ILogger<BaseRepository<BooksAuthors>> logger)
            : base(connection, logger) { }
    }
}
