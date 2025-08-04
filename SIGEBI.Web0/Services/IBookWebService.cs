using SIGEBI.Web0.Models.Book;


namespace SIGEBI.Web0.Services.Book
{
    public interface IBookWebService : IBaseWebService<BookModel, CreateBookModel, EditBookModel>
    {
        Task PopulateDropdownsForBookForm(dynamic viewBag);
        Task<BookModel?> GetBookDetailsAsync(int id);
    }
}
