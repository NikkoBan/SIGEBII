namespace SIGEBI.Web0.Services
{
    public interface IBaseWebService<TModel, TCreateModel, TEditModel>
    {
        Task<IEnumerable<TModel>> GetAllAsync();
        Task<TModel?> GetByIdAsync(int id);
        Task<TEditModel?> GetEditModelByIdAsync(int id);
        Task<bool> CreateAsync(TCreateModel model);
        Task<bool> UpdateAsync(int id, TEditModel model);
        Task<bool> DeleteAsync(int id);
    }

}
