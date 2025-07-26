

using Microsoft.Extensions.Logging;
using Moq;
using SIGEBI.Domain.Entities.Configuration;
using SIGEBI.Persistence.Context;
using SIGEBI.Persistence.Repositories;
using Xunit;

namespace SIGEBI.Persistence.Test
{
    public class AuthorRepositoryTests
    {
        private readonly SIGEBIContext _context;
        private readonly AuthorRepository _authorRepository;

        public AuthorRepositoryTests()
        {
            _context = InMemoryDbContextFactory.CreateContext();
            var loggerMock = new Mock<ILogger<AuthorRepository>>();
            _authorRepository = new AuthorRepository(_context, loggerMock.Object);
        }
    


    [Fact]
        public async Task CreateAsync_ShouldAddAuthor()
        {
            // Arrange
            var author = new Authors
            {
                FirstName = "Gabriel",
                LastName = "García Márquez",
                BirthDate = new DateTime(1927, 3, 6)
            };

            // Act
            var result = await _authorRepository.CreateAsync(author);

            // Assert
            Assert.True(result.Success);
            var added = await _context.Authors.FindAsync(author.Id);
            Assert.NotNull(added);
            Assert.Equal("Gabriel", added.FirstName);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnAuthor_WhenExists()
        {
            // Arrange
            var author = new Authors
            {
                FirstName = "Isabel",
                LastName = "Allende",
                BirthDate = new DateTime(1942, 8, 2)
            };
            await _context.Authors.AddAsync(author);
            await _context.SaveChangesAsync();

            // Act
            var result = await _authorRepository.GetByIdAsync(author.Id);

            // Assert
            Assert.True(result.Success);
            var returned = result.Data as Authors;
            Assert.NotNull(returned);
            Assert.Equal("Isabel", returned.FirstName);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnFailure_WhenNotFound()
        {
            // Act
            var result = await _authorRepository.GetByIdAsync(-1);

            // Assert
            Assert.False(result.Success);
            Assert.Contains("no encontrado", result.Message ?? string.Empty);
        }

        [Fact]
        public async Task UpdateAsync_ShouldModifyAuthor()
        {
            // Arrange
            var author = new Authors
            {
                FirstName = "Mario",
                LastName = "Vargas Llosa",
                BirthDate = new DateTime(1936, 3, 28)
            };
            await _context.Authors.AddAsync(author);
            await _context.SaveChangesAsync();

            author.LastName = "Llosa Vargas";

            // Act
            var result = await _authorRepository.UpdateAsync(author);

            // Assert
            Assert.True(result.Success);
            var updated = await _context.Authors.FindAsync(author.Id);
            Assert.Equal("Llosa Vargas", updated!.LastName);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveAuthor()
        {
            // Arrange
            var author = new Authors
            {
                FirstName = "Julio",
                LastName = "Cortázar",
                BirthDate = new DateTime(1914, 8, 26)
            };
            await _context.Authors.AddAsync(author);
            await _context.SaveChangesAsync();

            // Act
            var result = await _authorRepository.DeleteAsync(author.Id);

            // Assert
            Assert.True(result.Success);
            var deleted = await _context.Authors.FindAsync(author.Id);
            Assert.Null(deleted);
        }

        [Fact]
        public async Task ExistsAsync_ShouldReturnTrue_WhenExists()
        {
            // Arrange
            var author = new Authors
            {
                FirstName = "Borges",
                LastName = "Jorge Luis",
                BirthDate = new DateTime(1899, 8, 24)
            };
            await _context.Authors.AddAsync(author);
            await _context.SaveChangesAsync();

            // Act
            var exists = await _authorRepository.ExistsAsync(author.Id);

            // Assert
            Assert.True(exists);
        }

        [Fact]
        public async Task ExistsAsync_ShouldReturnFalse_WhenNotExists()
        {
            // Act
            var exists = await _authorRepository.ExistsAsync(-100);

            // Assert
            Assert.False(exists);
        }

        [Fact]
        public async Task CheckDuplicateAuthorAsync_ShouldReturnTrue_WhenAuthorExists()
        {
            // Arrange
            var author = new Authors
            {
                FirstName = "John",
                LastName = "Doe",
                BirthDate = new DateTime(1980, 1, 1)
            };
            await _context.Authors.AddAsync(author);
            await _context.SaveChangesAsync();

            // Act
            var result = await _authorRepository.CheckDuplicateAuthorAsync("John", "Doe", new DateTime(1980, 1, 1));

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task CheckDuplicateAuthorAsync_ShouldReturnFalse_WhenAuthorDoesNotExist()
        {
            // Arrange
            // Act
            var result = await _authorRepository.CheckDuplicateAuthorAsync("Jane", "Smith", new DateTime(1990, 5, 10));

            // Assert
            Assert.False(result);
        }
        [Fact]
        public async Task CheckDuplicateAuthorForUpdateAsync_ShouldReturnTrue_WhenAnotherAuthorMatches()
        {
            // Arrange
            var author1 = new Authors
            {
                FirstName = "Ana",
                LastName = "Perez",
                BirthDate = new DateTime(1975, 7, 20)
            };
            var author2 = new Authors
            {
                FirstName = "Ana",
                LastName = "Perez",
                BirthDate = new DateTime(1975, 7, 20)
            };
            await _context.Authors.AddRangeAsync(author1, author2);
            await _context.SaveChangesAsync();

            // Act
            var result = await _authorRepository.CheckDuplicateAuthorForUpdateAsync(author1.Id, "Ana", "Perez", new DateTime(1975, 7, 20));

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task CheckDuplicateAuthorForUpdateAsync_ShouldReturnFalse_WhenNoOtherAuthorMatches()
        {
            // Arrange
            var author = new Authors
            {
                FirstName = "Carlos",
                LastName = "Ruiz",
                BirthDate = new DateTime(1990, 3, 15)
            };
            await _context.Authors.AddAsync(author);
            await _context.SaveChangesAsync();

            // Act
            var result = await _authorRepository.CheckDuplicateAuthorForUpdateAsync(author.Id, "Carlos", "Ruiz", new DateTime(1990, 3, 15));

            // Assert
            Assert.False(result);
        }
        [Fact]
        public async Task GetGenresByAuthorAsync_ShouldReturnGenres_WhenBooksHaveCategories()
        {
            // Arrange
            var category = new Categories { CategoryName = "Fiction" };
            var author = new Authors { FirstName = "Laura", LastName = "Martinez", BirthDate = new DateTime(1985, 2, 10) };
            var book = new Books { Title = "Novel A", ISBN = "9999999999999", Category = category, PublisherId = 1, TotalCopies = 5, AvailableCopies = 5 };
            var bookAuthor = new BookAuthor { Author = author, Book = book };

            await _context.Categories.AddAsync(category);
            await _context.Authors.AddAsync(author);
            await _context.Books.AddAsync(book);
            await _context.BookAuthor.AddAsync(bookAuthor);
            await _context.SaveChangesAsync();

            // Act
            var result = await _authorRepository.GetGenresByAuthorAsync(author.Id);

            // Assert
            Assert.Contains("Fiction", result);
        }

        [Fact]
        public async Task GetGenresByAuthorAsync_ShouldReturnEmpty_WhenAuthorHasNoBooks()
        {
            // Arrange
            var author = new Authors { FirstName = "Mario", LastName = "Lopez", BirthDate = new DateTime(1970, 12, 12) };
            await _context.Authors.AddAsync(author);
            await _context.SaveChangesAsync();

            // Act
            var result = await _authorRepository.GetGenresByAuthorAsync(author.Id);

            // Assert
            Assert.Empty(result);
        }

    }
}

