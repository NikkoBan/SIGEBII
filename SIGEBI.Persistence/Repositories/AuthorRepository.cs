using Microsoft.Extensions.Logging;
using SIGEBI.Domain.Entities.Configuration;


namespace SIGEBI.Persistence.Repositories
{
    public class AuthorRepository : BaseRepository<Authors>
    {
        public AuthorRepository(string connection, ILogger<BaseRepository<Authors>> logger)
            : base(connection, logger) { }
    }

}
