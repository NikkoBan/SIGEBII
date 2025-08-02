using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Repository;

namespace SIGEBI.Persistence.Interfaces
{
    public interface ILoanHistoryRepository : IBaseRepository<LoanHistory, int>
    {
        Task<List<LoanHistory>> GetHistoryByUser(int userId);
        Task<List<LoanHistory>> GetHistoryByBook(int bookId);
    }
}
