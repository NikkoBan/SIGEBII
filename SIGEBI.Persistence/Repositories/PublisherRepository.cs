

using Microsoft.Extensions.Logging;
using SIGEBI.Domain.Entities.Configuration;

namespace SIGEBI.Persistence.Repositories
{
    public class PublisherRepository : BaseRepository<Publisher>
    {
        public PublisherRepository(string connection, ILogger<BaseRepository<Publisher>> logger)
            : base(connection, logger) { }
    }
}
