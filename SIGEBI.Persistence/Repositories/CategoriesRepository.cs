

using Microsoft.Extensions.Logging;
using SIGEBI.Domain.Entities.Configuration;

namespace SIGEBI.Persistence.Repositories
{
    public class CategoryRepository : BaseRepository<Categories>
    {
        public CategoryRepository(string connection, ILogger<BaseRepository<Categories>> logger)
            : base(connection, logger) { }
    }
}
