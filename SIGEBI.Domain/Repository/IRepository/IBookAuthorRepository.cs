using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities.Configuration;

namespace SIGEBI.Domain.IRepository
{
    public interface IBookAuthorRepository : IBaseRepository<BookAuthor>
    {
        Task<bool> CheckDuplicateBookAuthorCombinationAsync(int bookId, int authorId);
        Task<OperationResult> DeleteByBookAndAuthorAsync(int bookId, int authorId);
        Task<OperationResult> GetAuthorsByBookAsync(int bookId);
        Task<OperationResult> GetBooksByAuthorAsync(int authorId);
        Task<OperationResult> AddBookAuthorAsync(int bookId, int authorId);
        Task<BookAuthor?> GetByCompositeKeyAsync(int bookId, int authorId);
        Task<OperationResult> GetAllBookAuthorsAsync();



    }
}
