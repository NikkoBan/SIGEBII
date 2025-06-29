using SIGEBI.Domain.Entities;


namespace SIGEBI.Domain.Services
{
    public interface ILoanService
    {
        Loan? GetLoanById(int id);
        IEnumerable<Loan> GetLoansByUser(int userId);
        void RegisterLoan(Loan loan);
        void RegisterReturn(int loanId, DateTime returnDate, string observations);
        IEnumerable<LoanHistory> GetLoanHistoryByUser(int userId);
    }

    public class LoanService : ILoanService
    {
        private readonly List<Loan> _loans;
        private readonly List<LoanHistory> _loanHistories;

        public LoanService(List<Loan> loans, List<LoanHistory> loanHistories)
        {
            _loans = loans;
            _loanHistories = loanHistories;
        }

        public Loan? GetLoanById(int id)
        {
            return _loans.FirstOrDefault(l => l.ID == id && l.ReturnDate == null);
        }

        public IEnumerable<Loan> GetLoansByUser(int userId)
        {
            return _loans.Where(l => l.UserId == userId && l.ReturnDate == null);
        }

        public void RegisterLoan(Loan loan)
        {
            loan.ID = _loans.Count > 0 ? _loans.Max(l => l.ID) + 1 : 1;
            loan.LoanDate = DateTime.Now;
            loan.LoanStatus = "En curso";
            _loans.Add(loan);
        }

        public void RegisterReturn(int loanId, DateTime returnDate, string observations)
        {
            var loan = _loans.FirstOrDefault(l => l.ID == loanId);
            if (loan == null || loan.ReturnDate != null) return;

            loan.ReturnDate = returnDate;
            loan.LoanStatus = "Devuelto";

            _loanHistories.Add(new LoanHistory
            {
                ID = _loanHistories.Count > 0 ? _loanHistories.Max(h => h.ID) + 1 : 1,
                OriginalLoanId = loan.ID,
                BookId = loan.BookId,
                UserId = loan.UserId,
                LoanDate = loan.LoanDate,
                DueDate = loan.DueDate,
                ReturnDate = returnDate,
                FinalStatus = "Devuelto",
                Observations = observations
            });
        }

        public IEnumerable<LoanHistory> GetLoanHistoryByUser(int userId)
        {
            return _loanHistories.Where(h => h.UserId == userId);
        }
    }
}
