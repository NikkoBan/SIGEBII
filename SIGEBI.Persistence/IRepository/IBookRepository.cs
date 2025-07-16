//using SIGEBI.Domain.Base;
//using SIGEBI.Domain.Entities.Configuration;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace SIGEBI.Persistence.IRepository
//{
//    public interface IBookRepository : IBaseRepository<Books>
//    {
        
//        Task<bool> CheckDuplicateBookTitleAsync(string title, int? excludeBookId = null);
//        Task GetBooksByAuthorAsync(int authorId);
//        Task<OperationResult> GetBooksByAuthorIdAsync(int authorId);
//    }
//}
