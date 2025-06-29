using SIGEBI.Domain.Entities.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Application.Contracts.Repository
{
    public interface IBookAuthorRepository : IBaseRepository<BooksAuthors>
    {
        Task<bool> CheckDuplicateBookAuthorCombinationAsync(int bookId, int authorId);

    }
}
