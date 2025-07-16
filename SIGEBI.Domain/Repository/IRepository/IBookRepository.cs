using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities.Configuration;


namespace SIGEBI.Domain.IRepository
{
    public interface IBookRepository : IBaseRepository<Books>
    {
        
        Task<bool> CheckDuplicateBookTitleAsync(string title, int? excludeBookId = null);
        Task<OperationResult> GetBooksByCategoryAsync(int categoryId);
        Task<OperationResult> GetBooksByPublisherAsync(int publisherId);
        Task<OperationResult> SearchBooksAsync(string searchTerm);
        Task<OperationResult> GetAvailableBooksAsync();
        Task<bool> CheckDuplicateISBNAsync(string isbn, int? excludeBookId = null);
    }
}
