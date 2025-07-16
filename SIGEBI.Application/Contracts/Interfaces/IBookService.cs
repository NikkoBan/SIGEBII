using SIGEBI.Application.Dtos.BooksDtos;
using SIGEBI.Domain.Base;

namespace SIGEBI.Application.Contracts.Service
{
    public interface IBookService : IBaseService<BookDTO, CreateBookDTO, UpdateBookDto>
    {
        Task<OperationResult> CheckDuplicateBookTitleAsync(string title, int? excludeBookId = null);
        Task<OperationResult> CheckDuplicateISBNAsync(string isbn, int? excludeBookId = null);
        Task<OperationResult> GetBooksByCategoryAsync(int categoryId);
        Task<OperationResult> GetBooksByPublisherAsync(int publisherId);
        Task<OperationResult> SearchBooksAsync(string searchTerm);
        Task<OperationResult> GetAvailableBooksAsync();
    }
}
