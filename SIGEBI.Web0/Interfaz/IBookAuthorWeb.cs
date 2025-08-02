using SIGEBI.Web0.Models.BookAuthor;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIGEBI.Web0.Interfaz.BookAuthor
{
    public interface IBookAuthorWeb
    {
        Task<List<BookAuthorModel>?> GetAllAsync();
        Task<bool> CreateAsync(CreateBookAuthorModel model);
        Task<bool> DeleteAsync(int bookId, int authorId);
        Task<List<BookAuthorModel>?> GetAuthorsByBookAsync(int bookId);
        Task<List<BookAuthorModel>?> GetBooksByAuthorAsync(int authorId);
        Task<DeleteBookAuthorModel?> GetDeleteModelAsync(int bookId, int authorId);
        Task<BookAuthorModel?> GetByIdsAsync(int bookId, int authorId);

    }
}