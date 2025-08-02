using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIGEBI.Web0.Interfaz
{
    
    public interface IBaseWebRepository<TModel, TCreateModel, TEditModel>
        where TModel : class
        where TCreateModel : class
        where TEditModel : class
    {
        Task<List<TModel>> GetAllAsync();
        Task<TModel?> GetByIdAsync(int id);
        Task<TEditModel?> GetEditModelByIdAsync(int id); 
        Task<bool> CreateAsync(TCreateModel model);
        Task<bool> UpdateAsync(int id, TEditModel model);
        Task<bool> DeleteAsync(int id);
    }
}