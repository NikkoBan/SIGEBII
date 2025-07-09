using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SIGEBI.Persistence.Interfaces
{
    public interface IBookRepository : IBaseRepository<Book, int>
    {
        Task<Book?> GetBookByISBN(string isbn);
        Task<List<Book>> GetBooksByCategory(int categoryId);
        Task<List<Book>> GetAvailableBooks();
        Task<List<Book>> SearchBooksByTitle(string title);
        Task<List<Book>> GetDeletedBooks();
        Task<OperationResult<Book>> RestoreDeletedBook(int bookId);

        new Task<bool> Exists(Expression<Func<Book, bool>> filter);
    }
}
