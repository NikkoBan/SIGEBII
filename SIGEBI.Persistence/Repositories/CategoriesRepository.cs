

using Microsoft.Extensions.Logging;
using SIGEBI.Application.Contracts.Repository;
using SIGEBI.Application.Contracts.Service;
using SIGEBI.Domain.Entities.Configuration;
using SIGEBI.Persistence.Base.SIGEBI.Persistence.Repositories;
using SIGEBI.Persistence.Context;
using Microsoft.EntityFrameworkCore;
namespace SIGEBI.Persistence.Repositories
{
    public class CategoryRepository : BaseRepositoryEf<Categories>, ICategoryRepository
    {
        public CategoryRepository(SIGEBIContext context, ILogger<CategoryRepository> logger)
             : base(context, logger) { }

        public async Task<bool> CheckDuplicateCategoryNameAsync(string categoryName, int? excludeCategoryId = null)
        {
            try
            {
                if (excludeCategoryId.HasValue)
                {
                    
                    return await _dbSet.AnyAsync(c => c.CategoryName == categoryName && c.Id != excludeCategoryId.Value);
                }
               
                return await _dbSet.AnyAsync(c => c.CategoryName == categoryName);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"Error al verificar el nombre de categoría duplicado: {categoryName}.");
                return false;
            }

        }
    }
}
