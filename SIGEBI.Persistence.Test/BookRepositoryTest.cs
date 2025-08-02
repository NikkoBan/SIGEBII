using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using SIGEBI.Domain.Entities;
using SIGEBI.Persistence.Context;
using SIGEBI.Persistence.Repositories;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace SIGEBI.Persistence.Test.Repositories
{
    [TestClass]
    public class BookRepositoryTest
    {
        private SIGEBIDbContext _context = null!;
        private BookRepository _bookRepository = null!;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<SIGEBIDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new SIGEBIDbContext(options);
            _bookRepository = new BookRepository(_context);
        }

        [TestCleanup]
        public async Task Cleanup()
        {
            await _context.Database.EnsureDeletedAsync();
            await _context.DisposeAsync();
        }

        [TestMethod]
        public async Task SaveEntityAsync_ShouldReturnSuccess_WhenBookIsValid()
        {
            var newBook = new Book
            {
                Title = "Test Book",
                ISBN = "1234567890",
                AvailableCopies = 5,
                TotalCopies = 5,
                CategoryId = 1,
                PublisherId = 1,
                Language = "English",
                GeneralStatus = "New",
                Borrado = false,
                PublicationDate = DateTime.Now
            };

            var result = await _bookRepository.SaveEntityAsync(newBook);

            Assert.IsTrue(result.Success, "SaveEntityAsync debería retornar éxito para un libro válido.");
            Assert.AreNotEqual(0, newBook.ID, "El ID debería ser generado por la base de datos.");

            var bookInDb = await _context.Books.FindAsync(newBook.ID);
            Assert.IsNotNull(bookInDb, "El libro debería encontrarse en la base de datos.");
            Assert.AreEqual("Test Book", bookInDb.Title);
            Assert.AreEqual(5, bookInDb.AvailableCopies);
        }

        [TestMethod]
        public async Task GetEntityByIdAsync_ShouldReturnBook_WhenBookExists()
        {
            var existingBook = new Book
            {
                Title = "Existing Book",
                ISBN = "0987654321",
                AvailableCopies = 3,
                TotalCopies = 3,
                CategoryId = 1,
                PublisherId = 1,
                Language = "Spanish",
                GeneralStatus = "Used",
                Borrado = false,
                PublicationDate = DateTime.Now
            };

            await _context.Books.AddAsync(existingBook);
            await _context.SaveChangesAsync();

            var bookFound = await _bookRepository.GetEntityByIdAsync(existingBook.ID);

            Assert.IsNotNull(bookFound, "El libro debería ser encontrado.");
            Assert.AreEqual("Existing Book", bookFound.Title);
        }

        [TestMethod]
        public async Task UpdateEntityAsync_ShouldReturnSuccess_WhenBookExists()
        {
            var bookToUpdate = new Book
            {
                Title = "Old Title",
                ISBN = "1122334455",
                AvailableCopies = 2,
                TotalCopies = 2,
                CategoryId = 1,
                PublisherId = 1,
                Language = "French",
                GeneralStatus = "Old",
                Borrado = false,
                PublicationDate = DateTime.Now
            };

            await _context.Books.AddAsync(bookToUpdate);
            await _context.SaveChangesAsync();

            var bookId = bookToUpdate.ID;
            bookToUpdate.Title = "Updated Title";
            bookToUpdate.AvailableCopies = 4;

            var result = await _bookRepository.UpdateEntityAsync(bookToUpdate);

            Assert.IsTrue(result.Success, "UpdateEntityAsync debería retornar éxito.");
            var updatedBook = await _context.Books.FindAsync(bookId);
            Assert.IsNotNull(updatedBook);
            Assert.AreEqual("Updated Title", updatedBook.Title);
            Assert.AreEqual(4, updatedBook.AvailableCopies);
        }

        [TestMethod]
        public async Task RemoveEntityAsync_ShouldReturnSuccess_WhenBookExists()
        {
            var bookToRemove = new Book
            {
                Title = "To Remove",
                ISBN = "5566778899",
                AvailableCopies = 1,
                TotalCopies = 1,
                CategoryId = 1,
                PublisherId = 1,
                Language = "German",
                GeneralStatus = "Damaged",
                Borrado = false,
                PublicationDate = DateTime.Now
            };

            await _context.Books.AddAsync(bookToRemove);
            await _context.SaveChangesAsync();

            var result = await _bookRepository.RemoveEntityAsync(bookToRemove);

            Assert.IsTrue(result.Success, "RemoveEntityAsync debería retornar éxito.");
            var bookInDb = await _context.Books.FindAsync(bookToRemove.ID);
            Assert.IsNull(bookInDb, "El libro debería haber sido eliminado.");
        }

        [TestMethod]
        public async Task GetAvailableBooks_ShouldReturnOnlyAvailableBooks()
        {
            var bookAvailable = new Book
            {
                Title = "Available Book",
                ISBN = "111222333",
                AvailableCopies = 2,
                TotalCopies = 2,
                CategoryId = 1,
                PublisherId = 1,
                Language = "English",
                GeneralStatus = "Available",
                Borrado = false,
                PublicationDate = DateTime.Now
            };

            var bookNotAvailable = new Book
            {
                Title = "Unavailable Book",
                ISBN = "444555666",
                AvailableCopies = 0,
                TotalCopies = 2,
                CategoryId = 1,
                PublisherId = 1,
                Language = "English",
                GeneralStatus = "Unavailable",
                Borrado = false,
                PublicationDate = DateTime.Now
            };

            await _context.Books.AddRangeAsync(bookAvailable, bookNotAvailable);
            await _context.SaveChangesAsync();

            var availableBooks = await _bookRepository.GetAvailableBooks();

            Assert.IsTrue(availableBooks.All(b => b.AvailableCopies > 0), "Todos los libros devueltos deben tener copias disponibles.");
            Assert.IsTrue(availableBooks.Any(b => b.Title == "Available Book"), "El libro disponible debe estar en la lista.");
            Assert.IsFalse(availableBooks.Any(b => b.Title == "Unavailable Book"), "El libro sin copias no debe estar en la lista.");
        }
    }
}
