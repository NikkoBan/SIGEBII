
using Microsoft.Extensions.Logging;
using SIGEBI.Persistence.Base;
using SIGEBI.Persistence.Context;
using Moq;
using SIGEBI.Persistence.Repositories;
using SIGEBI.Domain.Entities.Configuration;
using Xunit;
using SIGEBI.Domain.Base;
using Assert = Xunit.Assert;
using SIGEBI.Application.Dtos.BooksDtos;




namespace SIGEBI.Persistence.Test
{
    public class BookRepositoryTests
    {

        private readonly SIGEBIContext _context;
        private readonly BookRepository _repository;


        public BookRepositoryTests()
        {
            _context = InMemoryDbContextFactory.CreateContext();
            var loggerMock = new Mock<ILogger<BookRepository>>();
            _repository = new BookRepository(_context, loggerMock.Object);

        }
        [Fact]
        public async Task TaskAsyncShoulAddBook()
        {
            //Arrange

            var book = new Books
            {
                Title = "Clean Code",
                ISBN = "1234567890123",
                CategoryId = 1,
                PublisherId = 1,
                GeneralStatus = "Activo",
                TotalCopies = 10,
                AvailableCopies = 10
            };

            //Act
            var result = await _repository.CreateAsync(book);

            //assert
            Assert.True(result.Success);                    
            var addedBook = await _context.Books.FindAsync(book.Id);
            Assert.NotNull(addedBook);                      
            Assert.Equal("Clean Code", addedBook.Title);
        }
        [Fact]
        public async Task GetByIdAsync_ShouldReturnBook_WhenExists()
        {
            // Arrange
            var book = new Books
            {
                Title = "DDD",
                ISBN = "9876543210123",
                CategoryId = 1,
                PublisherId = 1,
                GeneralStatus = "Activo",
                TotalCopies = 5,
                AvailableCopies = 5
            };
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByIdAsync(book.Id);

            // Assert
            Assert.True(result.Success);
            var returnedBook = result.Data as Books;
            Assert.NotNull(returnedBook);
            Assert.Equal("DDD", returnedBook.Title);
        }
        [Fact]
        public async Task GetByIdAsyncShouldFail_WhenNotFound()
        {
            // Act
            var result = await _repository.GetByIdAsync(-1);

            // Assert
            Assert.False(result.Success);
            Assert.Contains("no encontrado", result.Message ?? string.Empty);
        }

        [Fact]
        public async Task UpdateAsync_ShouldModifyBook()
        {
            // Arrange
            var book = new Books
            {
                Title = "Old Title",
                ISBN = "1231231231234",
                CategoryId = 1,
                PublisherId = 1,
                GeneralStatus = "Activo",
                TotalCopies = 10,
                AvailableCopies = 10
            };
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();

            book.Title = "New Title";

            // Act
            var result = await _repository.UpdateAsync(book);

            // Assert
            Assert.True(result.Success);
            var updatedBook = await _context.Books.FindAsync(book.Id);
            Assert.Equal("New Title", updatedBook.Title);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveBook()
        {
            // Arrange
            var book = new Books
            {
                Title = "To Delete",
                ISBN = "0000000000000",
                CategoryId = 1,
                PublisherId = 1,
                GeneralStatus = "Activo",
                TotalCopies = 1,
                AvailableCopies = 1
            };
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.DeleteAsync(book.Id);

            // Assert
            Assert.True(result.Success);
            var deletedBook = await _context.Books.FindAsync(book.Id);
            Assert.Null(deletedBook);
        }

        [Fact]
        public async Task ExistsAsync_ShouldReturnTrue_WhenExists()
        {
            // Arrange
            var book = new Books
            {
                Title = "Exists",
                ISBN = "1111111111111",
                CategoryId = 1,
                PublisherId = 1,
                GeneralStatus = "Activo",
                TotalCopies = 2,
                AvailableCopies = 2
            };
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();

            // Act
            var exists = await _repository.ExistsAsync(book.Id);

            // Assert
            Assert.True(exists);
        }

        [Fact]
        public async Task ExistsAsync_ShouldReturnFalse_WhenNotExists()
        {
            // Act
            var exists = await _repository.ExistsAsync(-100);

            // Assert
            Assert.False(exists);
        }


        [Fact]
        //arrange
        public async Task CheckDuplicateBookTitleAsync_ShouldReturnTrue_WhenDuplicateExists()
        {
            //act
            var book = new Books { Title = "Unique Title", ISBN = "1234567890123", CategoryId = 1, PublisherId = 1 };
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();

            var result = await _repository.CheckDuplicateBookTitleAsync("Unique Title");
            //assert
            Assert.True(result);
        }

        [Fact]
        //arrange
        public async Task CheckDuplicateBookTitleAsync_ShouldReturnFalse_WhenNoDuplicateExists()
        {
            //act
            var result = await _repository.CheckDuplicateBookTitleAsync("Nonexistent Title");
            //assert
            Assert.False(result);
        }

        [Fact]

        //arrange
        public async Task CheckDuplicateISBNAsync_ShouldReturnTrue_WhenDuplicateExists()
        {
            //act
            var book = new Books { Title = "Book", ISBN = "9876543210123", CategoryId = 1, PublisherId = 1 };
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();

            var result = await _repository.CheckDuplicateISBNAsync("9876543210123");
            //assert
            Assert.True(result);
        }

        [Fact]
        //arrange
        public async Task CheckDuplicateISBNAsync_ShouldReturnFalse_WhenNoDuplicateExists()
        {
            //act
            var result = await _repository.CheckDuplicateISBNAsync("non-existing-isbn");
            //assert
            Assert.False(result);
        }
        [Fact]
        //arrange
        public async Task GetBooksByCategoryAsync_ShouldReturnBooks_WhenCategoryExists()
        {
            //act
            var book = new Books { Title = "By Category", ISBN = "1234567891111", CategoryId = 10, PublisherId = 1 };
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();

            var result = await _repository.GetBooksByCategoryAsync(10);
            //assert
            Assert.True(result.Success);
            Assert.NotEmpty((List<BookDTO>)result.Data!);
        }

        [Fact]
        //arrange
        public async Task GetBooksByPublisherAsync_ShouldReturnBooks_WhenPublisherExists()
        {
            //act
            var book = new Books { Title = "By Publisher", ISBN = "1234567892222", CategoryId = 1, PublisherId = 20 };
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();

            var result = await _repository.GetBooksByPublisherAsync(20);
            //assert
            Assert.True(result.Success);
            Assert.NotEmpty((List<BookDTO>)result.Data!);
        }

        [Fact]
        //arrange
        public async Task SearchBooksAsync_ShouldReturnBooks_WhenTitleMatches()
        {
            //act
            var book = new Books { Title = "Search Match", ISBN = "1234567893333", Summary = "Resumen", CategoryId = 1, PublisherId = 1 };
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();

            var result = await _repository.SearchBooksAsync("Search");
            //assert
            Assert.True(result.Success);
            Assert.NotEmpty((List<BookDTO>)result.Data!);
        }

        [Fact]
        //arrange
        public async Task GetAvailableBooksAsync_ShouldReturnBooks_ThatAreNotDeleted()
        {
            //act
            var book = new Books { Title = "Disponible", ISBN = "1234567894444", CategoryId = 1, PublisherId = 1, IsDeleted = false };
            var deletedBook = new Books { Title = "Eliminado", ISBN = "0000000000000", CategoryId = 1, PublisherId = 1, IsDeleted = true };

            await _context.Books.AddRangeAsync(book, deletedBook);
            await _context.SaveChangesAsync();

            var result = await _repository.GetAvailableBooksAsync();
            //assert
            Assert.True(result.Success);
            var list = (List<BookDTO>)result.Data!;
            Assert.Single(list);
            Assert.Equal("Disponible", list.First().Title);
        }

    }


}

