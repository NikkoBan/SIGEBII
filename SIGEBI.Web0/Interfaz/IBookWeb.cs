using SIGEBI.Web0.Interfaz; 
using SIGEBI.Web0.Models.Book;
using Microsoft.AspNetCore.Mvc.Rendering;
using SIGEBI.Application.Dtos.BooksDtos;


namespace SIGEBI.Web0.Interfaces.Book
{
    public interface IBookWeb : IBaseWebRepository<BookModel, CreateBookModel, EditBookModel>
    {
       

        Task<IEnumerable<SelectListItem>> GetCategoriesAsSelectListAsync();
        Task<IEnumerable<SelectListItem>> GetPublishersAsSelectListAsync();
        Task<IEnumerable<SelectListItem>> GetAuthorsAsSelectListAsync();
        Task<BookModel?> GetBookWithRelationshipsByIdAsync(int id);

        Task<BookDTO?> GetBookDtoByIdAsync(int id);

    }
}