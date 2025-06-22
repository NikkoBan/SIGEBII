using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Repository;

namespace SIGEBI.Persistence.Interfaces
{
    public interface IBookRepository : IBaseRepository<Book, int>
    {
        Task<Book?> GetBookByISBN(string isbn);
        Task<List<Book>> GetBooksByCategory(int categoryId);
        Task<List<Book>> GetAvailableBooks();
    }
}
