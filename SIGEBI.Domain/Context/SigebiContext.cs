using Microsoft.EntityFrameworkCore;
using System;
using SIGEBI.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Domain.Context
{
    public class SigebiContext : DbContext
    {
        public SigebiContext(DbContextOptions<SigebiContext> options) : base(options)
        {
        }

        public DbSet<InsuranceProvider> InsuraceProviders { get; set; }

    }

}
