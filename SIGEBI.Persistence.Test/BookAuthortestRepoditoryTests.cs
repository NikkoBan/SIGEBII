

using Castle.Core.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities.Configuration;
using SIGEBI.Persistence.Context;
using SIGEBI.Persistence.Repositories;
using Xunit;

namespace SIGEBI.Persistence.Test
{
    public class BookAuthorRepositoryTests
    {
        private readonly SIGEBIContext _context;
        private readonly BookAuthorRepository bookAuthorRepository;

        public BookAuthorRepositoryTests()
        {
            _context = InMemoryDbContextFactory.CreateContext();
            var loggerMock = new Mock<ILogger<BookAuthorRepository>>();
            bookAuthorRepository = new BookAuthorRepository(_context, loggerMock.Object);
        }

        [Fact]


      
        public async Task AddBookAuthorAsync_ShouldAddRelation_WhenNotExists()
        {
            // Arrange
            var book = new Books { Title = "Libro Test", ISBN = "0001112223334", CategoryId = 1, PublisherId = 1, TotalCopies = 1, AvailableCopies = 1, GeneralStatus = "Activo" };
            var author = new Authors { FirstName = "Juan", LastName = "Pérez", BirthDate = new DateTime(1990, 1, 1), Nationality = "Dominicano" };

            await _context.Books.AddAsync(book);
            await _context.Authors.AddAsync(author);
            await _context.SaveChangesAsync();

            // Act
            var result = await bookAuthorRepository.AddBookAuthorAsync(book.Id, author.Id);

            // Assert
            Assert.True(result.Success);
            var exists = await _context.BookAuthor.AnyAsync(ba => ba.BookId == book.Id && ba.AuthorId == author.Id);
            Assert.True(exists);
        }
        [Fact]
        public async Task CheckDuplicateBookAuthorCombinationAsync_ShouldReturnTrue_WhenExists()
        {
            // Arrange
            var book = new Books { Title = "Existente", ISBN = "9991112223334", CategoryId = 1, PublisherId = 1, TotalCopies = 1, AvailableCopies = 1, GeneralStatus = "Activo" };
            var author = new Authors { FirstName = "Ana", LastName = "Gómez", BirthDate = new DateTime(1985, 5, 5), Nationality = "Colombiana" };

            await _context.Books.AddAsync(book);
            await _context.Authors.AddAsync(author);
            await _context.SaveChangesAsync();

            var relation = new BookAuthor { BookId = book.Id, AuthorId = author.Id };
            await _context.BookAuthor.AddAsync(relation);
            await _context.SaveChangesAsync();

            // Act
            var exists = await bookAuthorRepository.CheckDuplicateBookAuthorCombinationAsync(book.Id, author.Id);

            // Assert
            Assert.True(exists);
        }

        [Fact]
        public async Task DeleteByBookAndAuthorAsync_ShouldDeleteRelation_WhenExists()
        {
            var book = new Books { Title = "Book A", ISBN = "123", CategoryId = 1, PublisherId = 1, GeneralStatus = "Activo", TotalCopies = 1, AvailableCopies = 1 };
            var author = new Authors { FirstName = "John", LastName = "Doe", BirthDate = new DateTime(1980, 1, 1), Nationality = "US" };

            await _context.Books.AddAsync(book);
            await _context.Authors.AddAsync(author);
            await _context.SaveChangesAsync();

            var relation = new BookAuthor { BookId = book.Id, AuthorId = author.Id };
            await _context.BookAuthor.AddAsync(relation);
            await _context.SaveChangesAsync();

            var result = await bookAuthorRepository.DeleteByBookAndAuthorAsync(book.Id, author.Id);

            Assert.True(result.Success);
            var exists = await _context.BookAuthor.AnyAsync(ba => ba.BookId == book.Id && ba.AuthorId == author.Id);
            Assert.False(exists);
        }
        [Fact]
        public async Task DeleteByBookAndAuthorAsync_ShouldFail_WhenNotExists()
        {
            var result = await bookAuthorRepository.DeleteByBookAndAuthorAsync(-1, -1);

            Assert.False(result.Success);
            Assert.Contains("no existe", result.Message ?? string.Empty);
        }
        [Fact]
        public async Task GetAuthorsByBookAsync_ShouldReturnAuthors()
        {
            //arrge
            var book = new Books { Title = "Book B", ISBN = "456", CategoryId = 1, PublisherId = 1, GeneralStatus = "Activo", TotalCopies = 1, AvailableCopies = 1 };
            var author = new Authors { FirstName = "Jane", LastName = "Smith", BirthDate = new DateTime(1990, 1, 1), Nationality = "US" };
            // Act
            await _context.Books.AddAsync(book);
            await _context.Authors.AddAsync(author);
            await _context.SaveChangesAsync();

            var relation = new BookAuthor { BookId = book.Id, AuthorId = author.Id };
            //act
            await _context.BookAuthor.AddAsync(relation);
            await _context.SaveChangesAsync();

            var result = await bookAuthorRepository.GetAuthorsByBookAsync(book.Id);

            Assert.True(result.Success);
            var authors = result.Data as List<Authors>;
            Assert.NotNull(authors);
            Assert.Single(authors);
            Assert.Equal("Jane", authors[0].FirstName);
        }
        [Fact]
        public async Task GetBooksByAuthorAsync_ShouldReturnBooks()
        { // Arrange
            var book = new Books { Title = "Book C", ISBN = "789", CategoryId = 1, PublisherId = 1, GeneralStatus = "Activo", TotalCopies = 1, AvailableCopies = 1 };
            var author = new Authors { FirstName = "Carlos", LastName = "Lopez", BirthDate = new DateTime(1985, 5, 5), Nationality = "DO" };
            // Act
            await _context.Books.AddAsync(book);
            await _context.Authors.AddAsync(author);
            await _context.SaveChangesAsync();
            //arrange
            var relation = new BookAuthor { BookId = book.Id, AuthorId = author.Id };
            // Act
            await _context.BookAuthor.AddAsync(relation);
            await _context.SaveChangesAsync();
            //arrage
            var result = await bookAuthorRepository.GetBooksByAuthorAsync(author.Id);
            // Assert
            Assert.True(result.Success);
            //arrage
            var books = result.Data as List<Books>;
            Assert.NotNull(books);
            Assert.Single(books);
            Assert.Equal("Book C", books[0].Title);
        }


    }
}
