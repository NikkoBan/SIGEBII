using SIGEBI.Web0.Models.BookAuthor;

namespace SIGEBI.Web0.Services.BookAuthor
{
    public interface IBookAuthorWebService
    {
        Task<List<BookAuthorModel>> GetAllRelationshipsAsync();
        Task<BookAuthorModel?> GetRelationshipByIdsAsync(int bookId, int authorId);
        Task<bool> CreateRelationshipAsync(CreateBookAuthorModel model);
        Task<bool> DeleteRelationshipAsync(int bookId, int authorId);
        Task<DeleteBookAuthorModel?> GetDeleteModelAsync(int bookId, int authorId);
        Task PopulateDropdownsForBookAuthorForm(dynamic viewBag);
    }
}
