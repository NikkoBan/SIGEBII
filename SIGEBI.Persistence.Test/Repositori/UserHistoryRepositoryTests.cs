using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using SIGEBI.Domain.Entities;
using SIGEBI.Persistence.Context;
using SIGEBI.Persistence.Repositori;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace SIGEBI.Persistence.Test.Repositories
{
    [TestClass]
    public class UserHistoryRepositoryTests
    {
        private SIGEBIDbContext _context = null!;
        private UserHistoryRepository _userHistoryRepository = null!;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<SIGEBIDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new SIGEBIDbContext(options);
            _userHistoryRepository = new UserHistoryRepository(_context);

   
            var user = new User
            {
                UserId = 1, 
                FullName = "Test User",
                InstitutionalEmail = "user@test.com",
                PasswordHash = "hashedpass",
                RegistrationDate = DateTime.Now,
                RoleId = 1,
                IsActive = true,
                CreatedAt = DateTime.Now,
                CreatedBy = "Test"
            };
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        [TestCleanup]
        public async Task Cleanup()
        {
            await _context.Database.EnsureDeletedAsync();
            await _context.DisposeAsync();
        }

        [TestMethod]
        public async Task AddAsync_ShouldReturnSuccess_WhenUserHistoryIsValid()
        {
            // Arrange
            var newHistory = new UserHistory
            {
                UserId = 1, 
                EnteredEmail = "newuser@example.com",
                AttemptDate = DateTime.Now,
                IsSuccessful = true,
                CreatedAt = DateTime.Now,
                CreatedBy = "System"
            };

            // Act
            var result = await _userHistoryRepository.AddAsync(newHistory);

            // Assert
            Assert.IsTrue(result.Success, "AddAsync debería retornar éxito para un historial válido.");
            Assert.AreNotEqual(0, newHistory.LogId, "El LogId debería ser generado por la base de datos.");

            var historyInDb = await _context.UserHistories.FindAsync(newHistory.LogId);
            Assert.IsNotNull(historyInDb, "La entrada de historial debería encontrarse en la base de datos.");
            Assert.AreEqual("newuser@example.com", historyInDb.EnteredEmail);
        }

        [TestMethod]
        public async Task GetByIdAsync_ShouldReturnHistory_WhenEntryExists()
        {
            // Arrange
            var existingHistory = new UserHistory
            {
                LogId = 1, 
                UserId = 1,
                EnteredEmail = "existing@example.com",
                AttemptDate = DateTime.Now,
                IsSuccessful = false,
                FailureReason = "Wrong password",
                CreatedAt = DateTime.Now,
                CreatedBy = "System"
            };
            await _context.UserHistories.AddAsync(existingHistory);
            await _context.SaveChangesAsync();

            // Act
            var foundHistory = await _userHistoryRepository.GetByIdAsync(1);

            // Assert
            Assert.IsNotNull(foundHistory, "La entrada de historial debería ser encontrada.");
            Assert.AreEqual("existing@example.com", foundHistory.EnteredEmail);
        }

        [TestMethod]
        public async Task GetByIdAsync_ShouldReturnNull_WhenEntryDoesNotExist()
        {
            // Arrange (no hay historial en la DB)

            // Act
            var foundHistory = await _userHistoryRepository.GetByIdAsync(999);

            // Assert
            Assert.IsNull(foundHistory, "No se debería encontrar ninguna entrada de historial.");
        }

        [TestMethod]
        public async Task GetByUserIdAsync_ShouldReturnHistoryList_WhenEntriesExistForUser()
        {
            // Arrange
            var user1Id = 1; 
            var user2Id = 2; 
            var user2 = new User { UserId = user2Id, FullName = "User Two", InstitutionalEmail = "user2@test.com", PasswordHash = "hash", RegistrationDate = DateTime.Now, RoleId = 1, IsActive = true, CreatedAt = DateTime.Now, CreatedBy = "Test" };
            _context.Users.Add(user2);
            _context.SaveChanges();

            var historyForUser1_1 = new UserHistory { UserId = user1Id, EnteredEmail = "user1@a.com", AttemptDate = DateTime.Now.AddMinutes(-5), IsSuccessful = true, CreatedAt = DateTime.Now, CreatedBy = "System" };
            var historyForUser1_2 = new UserHistory { UserId = user1Id, EnteredEmail = "user1@b.com", AttemptDate = DateTime.Now, IsSuccessful = false, FailureReason = "Incorrect password", CreatedAt = DateTime.Now, CreatedBy = "System" };
            var historyForUser2 = new UserHistory { UserId = user2Id, EnteredEmail = "user2@a.com", AttemptDate = DateTime.Now.AddMinutes(-10), IsSuccessful = true, CreatedAt = DateTime.Now, CreatedBy = "System" };

            await _context.UserHistories.AddRangeAsync(historyForUser1_1, historyForUser1_2, historyForUser2);
            await _context.SaveChangesAsync();

            // Act
            var historyList = await _userHistoryRepository.GetByUserIdAsync(user1Id);

            // Assert
            Assert.IsNotNull(historyList);
            Assert.AreEqual(2, historyList.Count, "Debería retornar 2 entradas de historial para el usuario 1.");
            Assert.IsTrue(historyList.All(h => h.UserId == user1Id), "Todas las entradas deben ser del usuario correcto.");
        }

        [TestMethod]
        public async Task UpdateAsync_ShouldReturnSuccess_WhenUserHistoryExistsAndIsValid()
        {
            // Arrange
            var historyToUpdate = new UserHistory
            {
                UserId = 1,
                EnteredEmail = "original@example.com",
                AttemptDate = DateTime.Now,
                IsSuccessful = true,
                CreatedAt = DateTime.Now,
                CreatedBy = "System"
            };
            await _context.UserHistories.AddAsync(historyToUpdate);
            await _context.SaveChangesAsync();

            historyToUpdate.EnteredEmail = "updated@example.com";
            historyToUpdate.FailureReason = "Updated reason";
            historyToUpdate.UpdatedAt = DateTime.Now;
            historyToUpdate.UpdatedBy = "Test Update";

            // Act
            var result = await _userHistoryRepository.UpdateAsync(historyToUpdate);

            // Assert
            Assert.IsTrue(result.Success, "UpdateAsync debería retornar éxito.");
            var updatedHistory = await _context.UserHistories.FindAsync(historyToUpdate.LogId);
            Assert.IsNotNull(updatedHistory);
            Assert.AreEqual("updated@example.com", updatedHistory.EnteredEmail);
            Assert.AreEqual("Updated reason", updatedHistory.FailureReason);
        }

        [TestMethod]
        public async Task RemoveAsync_ShouldReturnSuccess_WhenUserHistoryExists()
        {
            // Arrange
            var historyToRemove = new UserHistory
            {
                UserId = 1,
                EnteredEmail = "to_remove@example.com",
                AttemptDate = DateTime.Now,
                IsSuccessful = true,
                CreatedAt = DateTime.Now,
                CreatedBy = "System"
            };
            await _context.UserHistories.AddAsync(historyToRemove);
            await _context.SaveChangesAsync();

            // Act
            var result = await _userHistoryRepository.RemoveAsync(historyToRemove.LogId);

            // Assert
            Assert.IsTrue(result.Success, "RemoveAsync debería retornar éxito.");
            var historyInDb = await _context.UserHistories.FindAsync(historyToRemove.LogId);
            Assert.IsNull(historyInDb, "La entrada de historial debería haber sido eliminada.");
        }
    }
}