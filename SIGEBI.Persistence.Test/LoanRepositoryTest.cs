using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using SIGEBI.Domain.Entities;
using SIGEBI.Persistence.Context;
using SIGEBI.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace SIGEBI.Persistence.Test.Repositories
{
    [TestClass]
    public class LoanRepositoryTest
    {
        private SIGEBIDbContext _context = null!;
        private LoanRepository _loanRepository = null!;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<SIGEBIDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new SIGEBIDbContext(options);
            _loanRepository = new LoanRepository(_context);
        }

        [TestCleanup]
        public async Task Cleanup()
        {
            await _context.Database.EnsureDeletedAsync();
            await _context.DisposeAsync();
        }

        [TestMethod]
        public async Task SaveEntityAsync_ShouldReturnSuccess_WhenLoanIsValid()
        {
            var loan = new Loan
            {
                UserId = 1,
                BookId = 1,
                LoanDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(7),
                ReturnDate = null,
                Borrado = false
            };

            var result = await _loanRepository.SaveEntityAsync(loan);

            Assert.IsTrue(result.Success);
            Assert.IsTrue(loan.ID > 0);

            var loanInDb = await _context.Loans.FindAsync(loan.ID);
            Assert.IsNotNull(loanInDb);
            Assert.AreEqual(1, loanInDb.UserId);
        }

        [TestMethod]
        public async Task GetEntityByIdAsync_ShouldReturnLoan_WhenLoanExists()
        {
            var loan = new Loan
            {
                UserId = 2,
                BookId = 3,
                LoanDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(10),
                Borrado = false
            };
            await _context.Loans.AddAsync(loan);
            await _context.SaveChangesAsync();

            var loanFromRepo = await _loanRepository.GetEntityByIdAsync(loan.ID);

            Assert.IsNotNull(loanFromRepo);
            Assert.AreEqual(2, loanFromRepo!.UserId);
        }

        [TestMethod]
        public async Task GetEntityByIdAsync_ShouldReturnNull_WhenLoanDoesNotExist()
        {
            var loanFromRepo = await _loanRepository.GetEntityByIdAsync(999);

            Assert.IsNull(loanFromRepo);
        }

        [TestMethod]
        public async Task GetAllAsync_ShouldReturnAllLoans()
        {
            var loan1 = new Loan { UserId = 1, BookId = 1, LoanDate = DateTime.Now, DueDate = DateTime.Now.AddDays(7), Borrado = false };
            var loan2 = new Loan { UserId = 2, BookId = 2, LoanDate = DateTime.Now, DueDate = DateTime.Now.AddDays(5), Borrado = false };
            await _context.Loans.AddRangeAsync(loan1, loan2);
            await _context.SaveChangesAsync();

            var loans = await _loanRepository.GetAllAsync();

            Assert.IsNotNull(loans);
            Assert.AreEqual(2, loans.Count);
        }

        [TestMethod]
        public async Task UpdateEntityAsync_ShouldReturnSuccess_WhenLoanExists()
        {
            var loan = new Loan
            {
                UserId = 3,
                BookId = 4,
                LoanDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(7),
                Borrado = false
            };
            await _context.Loans.AddAsync(loan);
            await _context.SaveChangesAsync();

            loan.DueDate = loan.DueDate.AddDays(3);

            var result = await _loanRepository.UpdateEntityAsync(loan);

            Assert.IsTrue(result.Success);

            var updatedLoan = await _context.Loans.FindAsync(loan.ID);
            Assert.IsNotNull(updatedLoan);
            Assert.AreEqual(loan.DueDate, updatedLoan!.DueDate);
        }

        [TestMethod]
        public async Task RemoveEntityAsync_ShouldReturnSuccess_WhenLoanExists()
        {
            var loan = new Loan
            {
                UserId = 5,
                BookId = 6,
                LoanDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(7),

            };
            await _context.Loans.AddAsync(loan);
            await _context.SaveChangesAsync();

            var result = await _loanRepository.RemoveEntityAsync(loan);

            Assert.IsTrue(result.Success);

            var deletedLoan = await _context.Loans.FindAsync(loan.ID);
            Assert.IsNull(deletedLoan);
        }

        [TestMethod]
        public async Task GetLoansByUser_ShouldReturnLoans_ForGivenUser()
        {
            var loan1 = new Loan { UserId = 10, BookId = 1, LoanDate = DateTime.Now, DueDate = DateTime.Now.AddDays(7) };
            var loan2 = new Loan { UserId = 10, BookId = 2, LoanDate = DateTime.Now, DueDate = DateTime.Now.AddDays(7) };
            var loan3 = new Loan { UserId = 11, BookId = 3, LoanDate = DateTime.Now, DueDate = DateTime.Now.AddDays(7) };
            await _context.Loans.AddRangeAsync(loan1, loan2, loan3);
            await _context.SaveChangesAsync();

            var userLoans = await _loanRepository.GetLoansByUser(10);

            Assert.AreEqual(2, userLoans.Count);
            Assert.IsTrue(userLoans.All(l => l.UserId == 10));
        }

        [TestMethod]
        public async Task GetOverdueLoans_ShouldReturnOnlyOverdueLoans()
        {
            var overdueLoan = new Loan
            {
                UserId = 20,
                BookId = 1,
                LoanDate = DateTime.Now.AddDays(-15),
                DueDate = DateTime.Now.AddDays(-5),
                ReturnDate = null,

            };
            var activeLoan = new Loan
            {
                UserId = 21,
                BookId = 2,
                LoanDate = DateTime.Now.AddDays(-3),
                DueDate = DateTime.Now.AddDays(4),
                ReturnDate = null,

            };
            await _context.Loans.AddRangeAsync(overdueLoan, activeLoan);
            await _context.SaveChangesAsync();

            var overdueLoans = await _loanRepository.GetOverdueLoans();

            Assert.AreEqual(1, overdueLoans.Count);
            Assert.IsTrue(overdueLoans[0].ID == overdueLoan.ID);
        }

        [TestMethod]
        public async Task GetActiveLoanByBook_ShouldReturnLoan_WhenActiveLoanExists()
        {
            var loan = new Loan
            {
                UserId = 30,
                BookId = 5,
                LoanDate = DateTime.Now.AddDays(-2),
                DueDate = DateTime.Now.AddDays(5),
                ReturnDate = null,

            };
            await _context.Loans.AddAsync(loan);
            await _context.SaveChangesAsync();

            var activeLoan = await _loanRepository.GetActiveLoanByBook(5);

            Assert.IsNotNull(activeLoan);
            Assert.AreEqual(loan.ID, activeLoan!.ID);
        }

        [TestMethod]
        public async Task GetActiveLoanByBook_ShouldReturnNull_WhenNoActiveLoan()
        {
            var loan = new Loan
            {
                UserId = 31,
                BookId = 6,
                LoanDate = DateTime.Now.AddDays(-10),
                DueDate = DateTime.Now.AddDays(-3),
                ReturnDate = DateTime.Now.AddDays(-2),

            };
            await _context.Loans.AddAsync(loan);
            await _context.SaveChangesAsync();

            var activeLoan = await _loanRepository.GetActiveLoanByBook(6);

            Assert.IsNull(activeLoan);
        }
    }
}
