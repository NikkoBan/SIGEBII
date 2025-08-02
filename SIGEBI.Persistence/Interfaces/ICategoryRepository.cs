using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Repository;

namespace SIGEBI.Persistence.Interfaces
{
    public interface ICategoryRepository : IBaseRepository<Category, int>
    {
        Task<List<Category>> GetAllWithBooks();
    }
}
