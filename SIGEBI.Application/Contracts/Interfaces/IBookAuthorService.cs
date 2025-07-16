using SIGEBI.Application.Dtos.BookAuthorDTO;
using SIGEBI.Domain.Base;

namespace SIGEBI.Application.Contracts.Service
{
    public interface IAuthorBookService : IBaseService<BookAuthorDTO, CreateBookAuthorDTO, UpdateBookAuthorDTO>
    {
        Task<OperationResult> CheckDuplicateBookAuthorCombinationAsync(int bookId, int authorId);
        Task<OperationResult> DeleteByBookAndAuthorAsync(int bookId, int authorId);
        Task<OperationResult> GetAuthorsByBookAsync(int bookId);
        Task<OperationResult> GetBooksByAuthorAsync(int authorId);
        Task<OperationResult> AddBookAuthorAsync(int bookId, int authorId);
    }
}
