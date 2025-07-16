using SIGEBI.Application.Dtos.CategoryDto;

namespace SIGEBI.Application.Contracts.Service
{
    public interface ICategoryService : IBaseService<CreateCategoryDTO, UpdateCategoryDTO, CategoryDTO>
    {
        Task<bool> CheckDuplicateCategoryNameAsync(string name, int? excludeId = null);
    }
}
