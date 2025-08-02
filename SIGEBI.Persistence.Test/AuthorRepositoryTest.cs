using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using SIGEBI.Domain.Entities;
using SIGEBI.Persistence.Context;
using SIGEBI.Persistence.Repositories;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace SIGEBI.Persistence.Test.Repositories
{
    [TestClass]
    public class AuthorRepositoryTest
    {
        private SIGEBIDbContext _context = null!;
        private AuthorRepository _authorRepository = null!;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<SIGEBIDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new SIGEBIDbContext(options);
            _authorRepository = new AuthorRepository(_context);
        }

        [TestCleanup]
        public async Task Cleanup()
        {
            await _context.Database.EnsureDeletedAsync();
            await _context.DisposeAsync();
        }

        [TestMethod]
        public async Task SaveEntityAsync_ShouldReturnSuccess_WhenAuthorIsValid()
        {
            var author = new Author
            {
                FirstName = "Gabriel",
                LastName = "García Márquez",
                Nationality = "Colombian",
                BirthDate = new DateTime(1927, 3, 6)
            };

            var result = await _authorRepository.SaveEntityAsync(author);

            Assert.IsTrue(result.Success);
            Assert.IsTrue(author.ID > 0);

            var authorInDb = await _context.Authors.FindAsync(author.ID);
            Assert.IsNotNull(authorInDb);
            Assert.AreEqual("Gabriel", authorInDb.FirstName);
        }

        [TestMethod]
        public async Task GetEntityByIdAsync_ShouldReturnAuthor_WhenAuthorExists()
        {
            var author = new Author
            {
                FirstName = "Isabel",
                LastName = "Allende",
                Nationality = "Chilean",
                BirthDate = new DateTime(1942, 8, 2)

            };
            await _context.Authors.AddAsync(author);
            await _context.SaveChangesAsync();

            var authorFromRepo = await _authorRepository.GetEntityByIdAsync(author.ID);

            Assert.IsNotNull(authorFromRepo);
            Assert.AreEqual("Isabel", authorFromRepo!.FirstName);
        }

        [TestMethod]
        public async Task GetEntityByIdAsync_ShouldReturnNull_WhenAuthorDoesNotExist()
        {
            var authorFromRepo = await _authorRepository.GetEntityByIdAsync(999);

            Assert.IsNull(authorFromRepo);
        }

        [TestMethod]
        public async Task GetAuthorsByNationality_ShouldReturnAuthors_WhenNationalityMatches()
        {
            var author1 = new Author { FirstName = "Mario", LastName = "Vargas Llosa", Nationality = "Peruvian" };
            var author2 = new Author { FirstName = "Julio", LastName = "Cortázar", Nationality = "Argentinian" };
            var author3 = new Author { FirstName = "Laura", LastName = "Esquivel", Nationality = "Mexican" };
            await _context.Authors.AddRangeAsync(author1, author2, author3);
            await _context.SaveChangesAsync();

            var peruvianAuthors = await _authorRepository.GetAuthorsByNationality("Peruvian");

            Assert.AreEqual(1, peruvianAuthors.Count);
            Assert.AreEqual("Mario", peruvianAuthors[0].FirstName);
        }

        [TestMethod]
        public async Task UpdateNationality_ShouldReturnSuccess_WhenUpdateIsValid()
        {
            var author = new Author
            {
                FirstName = "Carlos",
                LastName = "Fuentes",
                Nationality = "Mexican"
            };
            await _context.Authors.AddAsync(author);
            await _context.SaveChangesAsync();

            var result = await _authorRepository.UpdateNationality(author, "Spanish");

            Assert.IsTrue(result.Success);

            var updatedAuthor = await _context.Authors.FindAsync(author.ID);
            Assert.AreEqual("Spanish", updatedAuthor!.Nationality);
        }

        [TestMethod]
        public async Task RemoveEntityAsync_ShouldReturnSuccess_WhenAuthorExists()
        {
            var author = new Author
            {
                FirstName = "Jorge",
                LastName = "Luis Borges",
                Nationality = "Argentinian"
            };
            await _context.Authors.AddAsync(author);
            await _context.SaveChangesAsync();

            var result = await _authorRepository.RemoveEntityAsync(author);

            Assert.IsTrue(result.Success);

            var deletedAuthor = await _context.Authors.FindAsync(author.ID);
            Assert.IsNull(deletedAuthor);
        }
    }
}
