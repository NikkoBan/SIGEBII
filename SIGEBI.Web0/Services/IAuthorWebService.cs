using SIGEBI.Web0.Models.Author;

namespace SIGEBI.Web0.Services.Author
{
    public interface IAuthorWebService
    {
        Task<List<Authormodel>> GetAllAuthorsAsync();
        Task<Authormodel?> GetAuthorByIdAsync(int id);
        Task<bool> CreateAuthorAsync(CreateAuthorModel model);
        Task<bool> UpdateAuthorAsync(int id, EditAuthorModel model);
        Task<EditAuthorModel?> GetEditAuthorModelByIdAsync(int id);
        Task<bool> DeleteAuthorAsync(int id);
    }
}
