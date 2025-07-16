using SIGEBI.Domain.Entities.Configuration;

namespace SIGEBI.Domain.IRepository
{
     public interface ICategoryRepository : IBaseRepository<Categories>
    {
          /// <summary>
          /// Verifica si ya existe una categoría con el mismo nombre (para crear).
          /// </summary>
          /// <param name="categoryName">Nombre de la categoría.</param>
          /// <param name="excludeCategoryId">Id a excluir en caso de actualización (opcional).</param>
          /// <returns>True si hay duplicado, false si es único.</returns>
        Task<bool> CheckDuplicateCategoryNameAsync(string categoryName, int? excludeCategoryId = null);
}
    
    }