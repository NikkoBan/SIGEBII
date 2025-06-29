using SIGEBI.Domain.Entities.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Application.Contracts.Repository
{
   public interface ICategoryRepository : IBaseRepository<Categories>
    {
        Task<bool> CheckDuplicateCategoryNameAsync(string categoryName, int? excludeCategoryId = null);

    }
}
