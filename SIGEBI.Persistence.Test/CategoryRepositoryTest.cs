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
    public class CategoryRepositoryTest
    {
        private SIGEBIDbContext _context = null!;
        private CategoryRepository _categoryRepository = null!;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<SIGEBIDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new SIGEBIDbContext(options);
            _categoryRepository = new CategoryRepository(_context);
        }

        [TestCleanup]
        public async Task Cleanup()
        {
            await _context.Database.EnsureDeletedAsync();
            await _context.DisposeAsync();
        }

        [TestMethod]
        public async Task SaveEntityAsync_ShouldReturnSuccess_WhenCategoryIsValid()
        {
            var category = new Category
            {
                CategoryName = "Filosofía"
            };

            var result = await _categoryRepository.SaveEntityAsync(category);

            Assert.IsTrue(result.Success);
            Assert.IsTrue(category.ID > 0);

            var categoryInDb = await _context.Categories.FindAsync(category.ID);
            Assert.IsNotNull(categoryInDb);
            Assert.AreEqual("Filosofía", categoryInDb.CategoryName);
        }

        [TestMethod]
        public async Task GetEntityByIdAsync_ShouldReturnCategory_WhenCategoryExists()
        {
            var category = new Category { CategoryName = "Ciencia" };
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            var result = await _categoryRepository.GetEntityByIdAsync(category.ID);

            Assert.IsNotNull(result);
            Assert.AreEqual("Ciencia", result!.CategoryName);
        }

        [TestMethod]
        public async Task GetEntityByIdAsync_ShouldReturnNull_WhenCategoryDoesNotExist()
        {
            var result = await _categoryRepository.GetEntityByIdAsync(999);
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetCategoryByName_ShouldReturnCategory_WhenNameMatches()
        {
            await _context.Categories.AddAsync(new Category { CategoryName = "Novela" });
            await _context.SaveChangesAsync();

            var result = await _categoryRepository.GetCategoryByName("novela");

            Assert.IsNotNull(result);
            Assert.AreEqual("Novela", result!.CategoryName);
        }

        [TestMethod]
        public async Task GetAllWithBooks_ShouldReturnCategories_WithBooksIncluded()
        {
            var category = new Category
            {
                CategoryName = "Historia",
                Books = new List<Book>
                {
                    new Book { Title = "Historia Universal", ISBN = "123456789", PublisherId = 1, CategoryId = 1, Language = "Español", TotalCopies = 10, AvailableCopies = 5, GeneralStatus = "Disponible" }
                }
            };

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            var result = await _categoryRepository.GetAllWithBooks();

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.IsTrue(result.First().Books!.Any());
        }

        [TestMethod]
        public async Task UpdateEntityAsync_ShouldReturnSuccess_WhenCategoryIsValid()
        {
            var category = new Category { CategoryName = "Psicología" };
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            category.CategoryName = "Psicología Moderna";

            var result = await _categoryRepository.UpdateEntityAsync(category);

            Assert.IsTrue(result.Success);
            var updated = await _context.Categories.FindAsync(category.ID);
            Assert.AreEqual("Psicología Moderna", updated!.CategoryName);
        }

        [TestMethod]
        public async Task RemoveEntityAsync_ShouldReturnSuccess_WhenCategoryExists()
        {
            var category = new Category { CategoryName = "Arte" };
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            var result = await _categoryRepository.RemoveEntityAsync(category);

            Assert.IsTrue(result.Success);
            var deleted = await _context.Categories.FindAsync(category.ID);
            Assert.IsNull(deleted);
        }
    }
}
