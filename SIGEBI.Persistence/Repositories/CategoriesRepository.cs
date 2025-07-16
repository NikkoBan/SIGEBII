
namespace SIGEBI.Persistence.Repositories;

//    public class CategoryRepository : BaseRepositoryEf<Categories>, ICategoryRepository
//    {
//        public CategoryRepository(SIGEBIContext context, ILogger<BaseRepositoryEf<Categories>> logger)
//            : base(context, logger) { }

//        public async Task<bool> CheckDuplicateCategoryNameAsync(string categoryName, int? excludeCategoryId = null)
//        {
//            try
//            {
//                return excludeCategoryId.HasValue
//                    ? await _dbSet.AnyAsync(c => c.CategoryName == categoryName && c.Id != excludeCategoryId.Value)
//                    : await _dbSet.AnyAsync(c => c.CategoryName == categoryName);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error verificando nombre duplicado de categoría");
//                return false;
//            }
//        }
//        public override async Task<OperationResult> CreateAsync(Categories entity)
//        {
//            try
//            {
//                await _dbSet.AddAsync(entity);
//                await _context.SaveChangesAsync();

//                return new OperationResult
//                {
//                    Success = true,
//                    Data = entity,
//                    Message = "Categoría creada exitosamente."
//                };
//            }

//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error al crear la categoría.");
//                return new OperationResult
//                {
//                    Success = false,
//                    Message = $"Error al crear Categories: {ex.Message}"
//                };
//            }

//        }
//    }
//}
