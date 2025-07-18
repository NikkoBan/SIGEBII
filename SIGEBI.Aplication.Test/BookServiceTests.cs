
using Moq;
using Xunit;
using SIGEBI.Application.Services;
using SIGEBI.Domain.IRepository;
using SIGEBI.Application.Dtos.AuthorDTO;
using Microsoft.Extensions.Logging;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities.Configuration;
using Assert = Xunit.Assert;
using SIGEBI.Application.Dtos.BooksDtos;

namespace SIGEBI.Aplication.Test
{
    public class BookServiceTests
    {
        private readonly Mock<IBookRepository> _bookRepoMock;
        private readonly Mock<ILogger<BookService>> _loggerMock;
        private readonly BookService _bookService;

        public BookServiceTests()
        {
            _bookRepoMock = new Mock<IBookRepository>();
            _loggerMock = new Mock<ILogger<BookService>>();
            _bookService = new BookService(_bookRepoMock.Object, _loggerMock.Object);
        }


        [Fact]
        public async Task CreateAsync_ShouldReturnSuccessResult()
        {
            // Arrange
            var dto = new CreateBookDTO
            {
                Title = "El rpincipe Cruel",
                ISBN = "1234567890",
                PublicationDate = DateTime.Now,
                CategoryId = 1,
                PublisherId = 1,
                Language = "EN",
                Summary = "Un mundo de Hadas y una mortal se encuentra ahí",
                TotalCopies = 5,
                AuthorIds = new List<int> { 1, 2 }
            };

            var createdBook = new Books
            {
                Id = 1,
                Title = dto.Title,
                ISBN = dto.ISBN,
                PublicationDate = dto.PublicationDate,
                CategoryId = dto.CategoryId,
                PublisherId = dto.PublisherId,
                Language = dto.Language,
                Summary = dto.Summary,
                TotalCopies = dto.TotalCopies
            };

            _bookRepoMock.Setup(r => r.CreateAsync(It.IsAny<Books>()))
                .ReturnsAsync(OperationResult.SuccessResult(createdBook));

            // Act
            var result = await _bookService.CreateAsync(dto);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            var resultDto = Assert.IsType<BookDTO>(result.Data);
            Assert.Equal(dto.Title, resultDto.Title);
            Assert.Equal(dto.ISBN, resultDto.ISBN);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnSuccessResult()
        {
            // Arrange
            int bookId = 1;

            var dto = new UpdateBookDto
            {
                Title = "Updated Los sietes maridos de Evelyn",
                ISBN = "9876543210",
                PublicationDate = DateTime.Now,
                CategoryId = 2,
                PublisherId = 3,
                Language = "FR",
                Summary = "Updated Summary",
                TotalCopies = 10,
                AvailableCopies = 8,
                GeneralStatus = "Disponible"
            };

            var existingBook = new Books
            {
                Id = bookId,
                Title = "Los sietes maridos de Evelyn",
                ISBN = "1234567890",
                PublicationDate = DateTime.Now.AddYears(-1),
                CategoryId = 1,
                PublisherId = 1,
                Language = "EN",
                Summary = "Old Summary",
                TotalCopies = 5,
                AvailableCopies = 5,
                GeneralStatus = "Disponible"
            };

            _bookRepoMock.Setup(r => r.GetByIdAsync(bookId))
                .ReturnsAsync(OperationResult.SuccessResult(existingBook));

            _bookRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Books>()))
                .ReturnsAsync(OperationResult.SuccessResult(existingBook));

            // Act
            var result = await _bookService.UpdateAsync(bookId, dto);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);

            var resultEntity = Assert.IsType<Books>(result.Data);

            Assert.Equal(dto.Title, resultEntity.Title);
            Assert.Equal(dto.ISBN, resultEntity.ISBN);
        }
        [Fact]
        public async Task DeleteAsync_ShouldReturnSuccessResult()
        {
            // Arrange
            int bookId = 1;

            _bookRepoMock.Setup(r => r.DeleteAsync(bookId))
                .ReturnsAsync(OperationResult.SuccessResult(null, "Deleted"));

            // Act
            var result = await _bookService.DeleteAsync(bookId);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Deleted", result.Message);
        }




        [Fact]
        public async Task GetByIdAsync_ShouldReturnBook_WhenBookExists()
        {
            // Arrange
            int bookId = 1;
            var book = new Books
            {
                Id = bookId,
                Title = "Test Book",
                ISBN = "1234567890",
                PublicationDate = DateTime.Now,
                CategoryId = 1,
                PublisherId = 1,
                Language = "EN",
                Summary = "Test",
                TotalCopies = 5
            };

            _bookRepoMock.Setup(r => r.GetByIdAsync(bookId))
                .ReturnsAsync(OperationResult.SuccessResult(book));

            // Act
            var result = await _bookService.GetByIdAsync(bookId);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            var resultDto = Assert.IsType<BookDTO>(result.Data);
            Assert.Equal(book.Title, resultDto.Title);
        }
        [Fact]
        public async Task ExistsAsync_ShouldReturnTrue_WhenBookExists()
        {
            // Arrange
            int bookId = 1;

            _bookRepoMock.Setup(r => r.ExistsAsync(bookId)).ReturnsAsync(true);

            // Act
            var exists = await _bookRepoMock.Object.ExistsAsync(bookId);

            // Assert
            Assert.True(exists);
        }

        [Fact]
        public async Task CheckDuplicateBookTitleAsync_ShouldReturnTrue_WhenTitleExists()
        {
            // Arrange
            string title = "Existing Book";
            _bookRepoMock.Setup(r => r.CheckDuplicateBookTitleAsync(title, null))
                         .ReturnsAsync(true);

            // Act
            var result = await _bookService.CheckDuplicateBookTitleAsync(title);

            // Assert
            Assert.True(result.Success);
            Assert.True(result.Data is bool && (bool)result.Data);
        }
        [Fact]
        public async Task CheckDuplicateISBNAsync_ShouldReturnFalse_WhenISBNDontExist()
        {
            // Arrange
            string isbn = "9999999999999";
            _bookRepoMock.Setup(r => r.CheckDuplicateISBNAsync(isbn, null))
                         .ReturnsAsync(false);

            // Act
            var result = await _bookService.CheckDuplicateISBNAsync(isbn);

            // Assert
            Assert.True(result.Success);
            Assert.True(result.Data is bool && !(bool)result.Data);
        }

        [Fact]
        public async Task GetBooksByCategoryAsync_ShouldReturnBooks_WhenCategoryExists()
        {
            // Arrange
            int categoryId = 1;
            var books = new List<Domain.Entities.Configuration.Books>
            {
                new Domain.Entities.Configuration.Books { Id = 1, Title = "Book 1" },
                new Domain.Entities.Configuration.Books { Id = 2, Title = "Book 2" }
            };

            _bookRepoMock.Setup(r => r.GetBooksByCategoryAsync(categoryId))
                         .ReturnsAsync(OperationResult.SuccessResult(books));

            // Act
            var result = await _bookService.GetBooksByCategoryAsync(categoryId);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
        }
        [Fact]
        public async Task GetBooksByPublisherAsync_ShouldReturnFailure_WhenExceptionThrown()
        {
            // Arrange
            int publisherId = 1;
            _bookRepoMock.Setup(r => r.GetBooksByPublisherAsync(publisherId))
                         .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _bookService.GetBooksByPublisherAsync(publisherId);

            // Assert
            Assert.False(result.Success);
            Assert.Contains("Database error", result.Message);
        }
        [Fact]
        public async Task SearchBooksAsync_ShouldReturnSuccessResult_WithBooks()
        {
            // Arrange
            string searchTerm = "Test";
            var books = new List<Domain.Entities.Configuration.Books>
            {
                new Domain.Entities.Configuration.Books { Id = 1, Title = "Test Book" }
            };

            _bookRepoMock.Setup(r => r.SearchBooksAsync(searchTerm))
                         .ReturnsAsync(OperationResult.SuccessResult(books));

            // Act
            var result = await _bookService.SearchBooksAsync(searchTerm);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
        }

        [Fact]
        public async Task GetAvailableBooksAsync_ShouldReturnFailure_WhenExceptionOccurs()
        {
            // Arrange
            _bookRepoMock.Setup(r => r.GetAvailableBooksAsync())
                         .ThrowsAsync(new Exception("Error retrieving available books"));

            // Act
            var result = await _bookService.GetAvailableBooksAsync();

            // Assert
            Assert.False(result.Success);
            Assert.Contains("Error retrieving available books", result.Message);
        }
    }
}