using Microsoft.Extensions.Logging;
using SIGEBI.Domain.Entities.Configuration;

namespace SIGEBI.Persistence.Repositories
{
    public class BookRepository : BaseRepository<Books>
    {
        public BookRepository(string connection, ILogger<BaseRepository<Books>> logger)
            : base(connection, logger) { }
    }
}
