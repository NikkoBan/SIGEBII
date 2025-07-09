using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using SIGEBI.Domain.Entities;
using SIGEBI.Persistence.Context;
using SIGEBI.Persistence.Repositori;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic; // Aunque no se use explícitamente, para consistencia

namespace SIGEBI.Persistence.Test.Repositories
{
    [TestClass]
    public class UserRepositoryTests
    {
        private SIGEBIDbContext _context = null!;
        private UserRepository _userRepository = null!;

        [TestInitialize] 
        public void Setup()
        {
            
            var options = new DbContextOptionsBuilder<SIGEBIDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
                .Options;

            _context = new SIGEBIDbContext(options);
            _userRepository = new UserRepository(_context);
        }

        [TestCleanup] 
        public async Task Cleanup()
        {
           
            await _context.Database.EnsureDeletedAsync();
            // Libera los recursos del contexto
            await _context.DisposeAsync();
        }

        [TestMethod]
        public async Task AddAsync_ShouldReturnSuccess_WhenUserIsValid()
        {
            // Arrange (Preparar los datos de entrada para la prueba)
            var newUser = new User
            {
                FullName = "Jane Doe",
                InstitutionalEmail = "jane.doe@example.com",
                PasswordHash = "passwordhash123",
                RegistrationDate = DateTime.Now,
                RoleId = 2,
                IsActive = true,
                CreatedAt = DateTime.Now,
                CreatedBy = "Test"
            };

            // Act (Ejecutar la acción que se está probando)
            var result = await _userRepository.AddAsync(newUser);

            // Assert (Verificar el resultado)
            Assert.IsTrue(result.Success, "AddAsync debería retornar éxito para un usuario válido.");
            Assert.AreNotEqual(0, newUser.UserId, "El UserId debería ser generado por la base de datos.");

            var userInDb = await _context.Users.FindAsync(newUser.UserId);
            Assert.IsNotNull(userInDb, "El usuario debería encontrarse en la base de datos.");
            Assert.AreEqual("Jane Doe", userInDb.FullName);
        }

        [TestMethod]
        public async Task GetByIdAsync_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var existingUser = new User
            {
                UserId = 1, 
                FullName = "Test User",
                InstitutionalEmail = "test@example.com",
                PasswordHash = "passhash",
                RegistrationDate = DateTime.Now,
                RoleId = 1,
                IsActive = true,
                CreatedAt = DateTime.Now,
                CreatedBy = "Setup"
            };
            await _context.Users.AddAsync(existingUser);
            await _context.SaveChangesAsync();

            // Act
            var foundUser = await _userRepository.GetByIdAsync(1);

            // Assert
            Assert.IsNotNull(foundUser, "El usuario debería ser encontrado.");
            Assert.AreEqual("Test User", foundUser.FullName);
        }

        [TestMethod]
        public async Task GetByIdAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange (la base de datos en memoria está vacía para esta prueba)

            // Act
            var foundUser = await _userRepository.GetByIdAsync(999); 

            // Assert
            Assert.IsNull(foundUser, "No se debería encontrar ningún usuario.");
        }

        [TestMethod]
        public async Task GetAllAsync_ShouldReturnAllUsers()
        {
            // Arrange
            await _context.Users.AddAsync(new User { FullName = "User1", InstitutionalEmail = "user1@example.com", PasswordHash = "hash1", RegistrationDate = DateTime.Now, RoleId = 1, IsActive = true, CreatedAt = DateTime.Now, CreatedBy = "Test" });
            await _context.Users.AddAsync(new User { FullName = "User2", InstitutionalEmail = "user2@example.com", PasswordHash = "hash2", RegistrationDate = DateTime.Now, RoleId = 2, IsActive = true, CreatedAt = DateTime.Now, CreatedBy = "Test" });
            await _context.SaveChangesAsync();

            // Act
            var users = await _userRepository.GetAllAsync();

            // Assert
            Assert.IsNotNull(users);
            Assert.AreEqual(2, users.Count, "Debería retornar todos los 2 usuarios.");
        }

        [TestMethod]
        public async Task UpdateAsync_ShouldReturnSuccess_WhenUserExistsAndIsValid()
        {
            // Arrange
            var userToUpdate = new User
            {
                FullName = "Original Name",
                InstitutionalEmail = "original@example.com",
                PasswordHash = "passhash",
                RegistrationDate = DateTime.Now,
                RoleId = 1,
                IsActive = true,
                CreatedAt = DateTime.Now,
                CreatedBy = "Setup"
            };
            await _context.Users.AddAsync(userToUpdate);
            await _context.SaveChangesAsync();

            var userId = userToUpdate.UserId; 

            userToUpdate.FullName = "Updated Name";
            userToUpdate.InstitutionalEmail = "updated@example.com";
            userToUpdate.UpdatedAt = DateTime.Now;
            userToUpdate.UpdatedBy = "Updater";

            // Act
            var result = await _userRepository.UpdateAsync(userToUpdate);

            // Assert
            Assert.IsTrue(result.Success, "UpdateAsync debería retornar éxito.");
            var updatedUser = await _context.Users.FindAsync(userId);
            Assert.IsNotNull(updatedUser, "El usuario actualizado debería encontrarse.");
            Assert.AreEqual("Updated Name", updatedUser.FullName, "El nombre del usuario debería haberse actualizado.");
            Assert.AreEqual("updated@example.com", updatedUser.InstitutionalEmail, "El email del usuario debería haberse actualizado.");
        }

        [TestMethod]
        public async Task RemoveAsync_ShouldReturnSuccess_WhenUserExists()
        {
            // Arrange
            var userToRemove = new User
            {
                FullName = "To Remove",
                InstitutionalEmail = "remove@example.com",
                PasswordHash = "passhash",
                RegistrationDate = DateTime.Now,
                RoleId = 1,
                IsActive = true,
                CreatedAt = DateTime.Now,
                CreatedBy = "Setup"
            };
            await _context.Users.AddAsync(userToRemove);
            await _context.SaveChangesAsync();

            // Act
            var result = await _userRepository.RemoveAsync(userToRemove.UserId);

            // Assert
            Assert.IsTrue(result.Success, "RemoveAsync debería retornar éxito.");
            var userInDb = await _context.Users.FindAsync(userToRemove.UserId);
            Assert.IsNull(userInDb, "El usuario debería haber sido eliminado de la base de datos.");
        }

        [TestMethod]
        public async Task GetByEmailAsync_ShouldReturnUser_WhenEmailExists()
        {
            // Arrange
            var user = new User
            {
                FullName = "Email User",
                InstitutionalEmail = "emailtest@example.com",
                PasswordHash = "hash",
                RegistrationDate = DateTime.Now,
                RoleId = 1,
                IsActive = true,
                CreatedAt = DateTime.Now,
                CreatedBy = "Setup"
            };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // Act
            var foundUser = await _userRepository.GetByEmailAsync("emailtest@example.com");

            // Assert
            Assert.IsNotNull(foundUser, "El usuario debería ser encontrado por email.");
            Assert.AreEqual("Email User", foundUser.FullName);
        }

        [TestMethod]
        public async Task GetByEmailAsync_ShouldReturnNull_WhenEmailDoesNotExist()
        {
            // Arrange (DB vacía)

            // Act
            var foundUser = await _userRepository.GetByEmailAsync("nonexistent@example.com");

            // Assert
            Assert.IsNull(foundUser, "No se debería encontrar un usuario por un email inexistente.");
        }
    }
}