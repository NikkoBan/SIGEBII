using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Repository;

namespace SIGEBI.Persistence.Interfaces
{
    public interface IBookAuthorRepository : IBaseRepository<BookAuthor, int>
    {
        Task<List<BookAuthor>> GetByAuthorId(int authorId);
        Task<List<BookAuthor>> GetByBookId(int bookId);
    }
}
