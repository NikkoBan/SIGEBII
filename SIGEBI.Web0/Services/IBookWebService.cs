using SIGEBI.Web0.Models.Book;

namespace SIGEBI.Web0.Services.Book
{
    public interface IBookWebService
    {
        Task<IEnumerable<BookModel>> GetAllBooksAsync();
        Task<BookModel?> GetBookDetailsAsync(int id);
        Task<EditBookModel?> GetEditBookModelByIdAsync(int id);
        Task<bool> CreateBookAsync(CreateBookModel model);
        Task<bool> UpdateBookAsync(int id, EditBookModel model);
        Task<bool> DeleteBookAsync(int id);
        Task PopulateDropdownsForBookForm(dynamic viewBag);
    }
}
