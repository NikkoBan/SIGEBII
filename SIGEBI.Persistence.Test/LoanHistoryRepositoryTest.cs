using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using SIGEBI.Domain.Entities;
using SIGEBI.Persistence.Context;
using SIGEBI.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SIGEBI.Persistence.Test.Repositories
{
    [TestClass]
    public class LoanHistoryRepositoryTest
    {
        private SIGEBIDbContext _context = null!;
        private LoanHistoryRepository _repository = null!;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<SIGEBIDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new SIGEBIDbContext(options);
            _repository = new LoanHistoryRepository(_context);
        }

        [TestCleanup]
        public async Task Cleanup()
        {
            await _context.Database.EnsureDeletedAsync();
            await _context.DisposeAsync();
        }

        [TestMethod]
        public async Task SaveEntityAsync_ShouldReturnSuccess_WhenValid()
        {
            var loanHistory = new LoanHistory
            {
                BookId = 1,
                UserId = 2,
                LoanDate = DateTime.Today,
                ReturnDate = DateTime.Today.AddDays(7)
            };

            var result = await _repository.SaveEntityAsync(loanHistory);

            Assert.IsTrue(result.Success);
            Assert.IsTrue(loanHistory.ID > 0);
        }

        [TestMethod]
        public async Task GetEntityByIdAsync_ShouldReturnLoanHistory_WhenExists()
        {
            var history = new LoanHistory
            {
                BookId = 1,
                UserId = 2,
                LoanDate = DateTime.Today
            };

            await _context.LoanHistories.AddAsync(history);
            await _context.SaveChangesAsync();

            var result = await _repository.GetEntityByIdAsync(history.ID);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result!.BookId);
        }

        [TestMethod]
        public async Task GetEntityByIdAsync_ShouldReturnNull_WhenNotExists()
        {
            var result = await _repository.GetEntityByIdAsync(999);
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetAllAsync_ShouldReturnAllOrderedByDateDescending()
        {
            await _context.LoanHistories.AddRangeAsync(
                new LoanHistory { BookId = 1, UserId = 1, LoanDate = DateTime.Today.AddDays(-2) },
                new LoanHistory { BookId = 2, UserId = 2, LoanDate = DateTime.Today }
            );
            await _context.SaveChangesAsync();

            var results = await _repository.GetAllAsync();

            Assert.AreEqual(2, results.Count);
            Assert.IsTrue(results.First().LoanDate > results.Last().LoanDate);
        }

        [TestMethod]
        public async Task Exists_ShouldReturnTrue_WhenLoanExists()
        {
            var history = new LoanHistory { BookId = 1, UserId = 1, LoanDate = DateTime.Today };
            await _context.LoanHistories.AddAsync(history);
            await _context.SaveChangesAsync();

            var exists = await _repository.Exists(h => h.UserId == 1);
            Assert.IsTrue(exists);
        }

        [TestMethod]
        public async Task Exists_ShouldReturnFalse_WhenLoanDoesNotExist()
        {
            var exists = await _repository.Exists(h => h.UserId == 999);
            Assert.IsFalse(exists);
        }

        [TestMethod]
        public async Task GetHistoryByUser_ShouldReturnOnlyMatchingUser()
        {
            await _context.LoanHistories.AddRangeAsync(
                new LoanHistory { BookId = 1, UserId = 5, LoanDate = DateTime.Today },
                new LoanHistory { BookId = 2, UserId = 6, LoanDate = DateTime.Today }
            );
            await _context.SaveChangesAsync();

            var result = await _repository.GetHistoryByUser(5);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(5, result[0].UserId);
        }

        [TestMethod]
        public async Task GetHistoryByBook_ShouldReturnOnlyMatchingBook()
        {
            await _context.LoanHistories.AddRangeAsync(
                new LoanHistory { BookId = 10, UserId = 1, LoanDate = DateTime.Today },
                new LoanHistory { BookId = 20, UserId = 2, LoanDate = DateTime.Today }
            );
            await _context.SaveChangesAsync();

            var result = await _repository.GetHistoryByBook(10);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(10, result[0].BookId);
        }

        [TestMethod]
        public async Task UpdateEntityAsync_ShouldReturnSuccess_WhenValid()
        {
            var history = new LoanHistory
            {
                BookId = 3,
                UserId = 4,
                LoanDate = DateTime.Today,
                ReturnDate = null
            };

            await _context.LoanHistories.AddAsync(history);
            await _context.SaveChangesAsync();

            history.ReturnDate = DateTime.Today.AddDays(3);
            var result = await _repository.UpdateEntityAsync(history);

            Assert.IsTrue(result.Success);

            var updated = await _context.LoanHistories.FindAsync(history.ID);
            Assert.AreEqual(DateTime.Today.AddDays(3), updated!.ReturnDate);
        }

        [TestMethod]
        public async Task RemoveEntityAsync_ShouldReturnSuccess_WhenExists()
        {
            var history = new LoanHistory
            {
                BookId = 7,
                UserId = 8,
                LoanDate = DateTime.Today
            };

            await _context.LoanHistories.AddAsync(history);
            await _context.SaveChangesAsync();

            var result = await _repository.RemoveEntityAsync(history);

            Assert.IsTrue(result.Success);

            var deleted = await _context.LoanHistories.FindAsync(history.ID);
            Assert.IsNull(deleted);
        }
    }
}
