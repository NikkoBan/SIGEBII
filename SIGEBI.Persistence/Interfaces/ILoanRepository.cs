using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Repository;

namespace SIGEBI.Persistence.Interfaces
{
    public interface ILoanRepository : IBaseRepository<Loan, int>
    {
        Task<List<Loan>> GetLoansByUser(int userId);
        Task<List<Loan>> GetOverdueLoans();
        Task<Loan?> GetActiveLoanByBook(int bookId);
    }
}
