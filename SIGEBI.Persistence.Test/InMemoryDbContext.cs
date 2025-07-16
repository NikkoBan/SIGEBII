

using Microsoft.EntityFrameworkCore;
using SIGEBI.Persistence.Context;

namespace SIGEBI.Persistence.Test
{

    public static class InMemoryDbContextFactory
    {
        public static SIGEBIContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<SIGEBIContext>()
                .UseInMemoryDatabase(databaseName: "TestDB_" + System.Guid.NewGuid().ToString())
                .Options;

            return new SIGEBIContext(options);
        }
    }
}

