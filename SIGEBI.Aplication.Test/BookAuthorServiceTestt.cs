using Xunit;
using Moq;

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SIGEBI.Application.Services;
using SIGEBI.Domain.Entities.Configuration;
using SIGEBI.Domain.IRepository;
using SIGEBI.Application.Dtos.BookAuthorDTO;
using SIGEBI.Domain.Base;
using Assert = Xunit.Assert;

namespace SIGEBI.Tests.Application.Services
{
    public class BookAuthorServiceCrudTests
    {
        private readonly Mock<IBookAuthorRepository> _bookAuthorRepositoryMock;
        private readonly Mock<ILogger<BookAuthorService>> _loggerMock;
        private readonly BookAuthorService _service;

        public BookAuthorServiceCrudTests()
        {
            _bookAuthorRepositoryMock = new Mock<IBookAuthorRepository>();
            _loggerMock = new Mock<ILogger<BookAuthorService>>();
            _service = new BookAuthorService(_bookAuthorRepositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnSuccess()
        {
            // Arrange
            var createDto = new CreateBookAuthorDTO { BookId = 1, AuthorId = 2 };
            _bookAuthorRepositoryMock
                .Setup(repo => repo.CreateAsync(It.IsAny<BookAuthor>()))
                .ReturnsAsync(OperationResult.SuccessResult());

            // Act
            var result = await _service.CreateAsync(createDto);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Success);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnSuccess()
        {
            // Arrange
            var entity = new BookAuthor { BookId = 1, AuthorId = 2 };
            _bookAuthorRepositoryMock
                .Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(OperationResult.SuccessResult(entity));

            // Act
            var result = await _service.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnSuccess()
        {
            // Arrange
            var entities = new List<BookAuthor>
    {
        new BookAuthor { BookId = 1, AuthorId = 2 },
        new BookAuthor { BookId = 3, AuthorId = 4 }
    };

            _bookAuthorRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(OperationResult.SuccessResult(entities));

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.NotNull(result.Data);

            var typeName = result.Data?.GetType().ToString();
            Console.WriteLine($"Tipo de result.Data: {typeName}");

            var list = result.Data as IEnumerable<object>;
            Assert.NotNull(list);
            Assert.Equal(2, list.Count());
        }




        [Fact]
        public async Task UpdateAsync_ShouldReturnSuccess()
        {
            // Arrange
            var existingEntity = new BookAuthor { BookId = 1, AuthorId = 2 };

            _bookAuthorRepositoryMock
                .Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(OperationResult.SuccessResult(existingEntity));

            _bookAuthorRepositoryMock
                .Setup(repo => repo.UpdateAsync(It.IsAny<BookAuthor>()))
                .ReturnsAsync(OperationResult.SuccessResult());

            var updateDto = new UpdateBookAuthorDTO { BookId = 10, AuthorId = 20 };

            // Act
            var result = await _service.UpdateAsync(1, updateDto);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Success);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnSuccess()
        {
            // Arrange
            _bookAuthorRepositoryMock
                .Setup(repo => repo.DeleteAsync(It.IsAny<int>()))
                .ReturnsAsync(OperationResult.SuccessResult());

            // Act
            var result = await _service.DeleteAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Success);
        }
        [Fact]
        public async Task CheckDuplicateBookAuthorCombinationAsync_ReturnsTrue_WhenExists()
        {
            // Arrange
            _bookAuthorRepositoryMock.Setup(r => r.CheckDuplicateBookAuthorCombinationAsync(1, 1))
                     .ReturnsAsync(true);

            // Act
            var result = await _service.CheckDuplicateBookAuthorCombinationAsync(1, 1);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(true, result.Data);
        }

        [Fact]
        public async Task CheckDuplicateBookAuthorCombinationAsync_ReturnsFalse_WhenNotExists()
        {
            _bookAuthorRepositoryMock.Setup(r => r.CheckDuplicateBookAuthorCombinationAsync(1, 2))
                     .ReturnsAsync(false);

            var result = await _service.CheckDuplicateBookAuthorCombinationAsync(1, 2);

            Assert.True(result.Success);
            Assert.Equal(false, result.Data);
        }

        [Fact]
        public async Task DeleteByBookAndAuthorAsync_ReturnsSuccessResult()
        {
            var expected = OperationResult.SuccessResult();
            _bookAuthorRepositoryMock.Setup(r => r.DeleteByBookAndAuthorAsync(1, 1))
                     .ReturnsAsync(expected);

            var result = await _service.DeleteByBookAndAuthorAsync(1, 1);

            Assert.True(result.Success);
        }

        [Fact]
        public async Task GetAuthorsByBookAsync_ReturnsSuccessResult()
        {
            var expected = OperationResult.SuccessResult(new[] { "Author 1", "Author 2" });
            _bookAuthorRepositoryMock.Setup(r => r.GetAuthorsByBookAsync(1))
                     .ReturnsAsync(expected);

            var result = await _service.GetAuthorsByBookAsync(1);

            Assert.True(result.Success);
            Assert.NotNull(result.Data);
        }

        [Fact]
        public async Task GetBooksByAuthorAsync_ReturnsSuccessResult()
        {
            var expected = OperationResult.SuccessResult(new[] { "Book 1", "Book 2" });
            _bookAuthorRepositoryMock.Setup(r => r.GetBooksByAuthorAsync(1))
                     .ReturnsAsync(expected);

            var result = await _service.GetBooksByAuthorAsync(1);

            Assert.True(result.Success);
            Assert.NotNull(result.Data);
        }

        [Fact]
        public async Task AddBookAuthorAsync_ReturnsSuccessResult()
        {
            var expected = OperationResult.SuccessResult();
            _bookAuthorRepositoryMock.Setup(r => r.AddBookAuthorAsync(1, 1))
                     .ReturnsAsync(expected);

            var result = await _service.AddBookAuthorAsync(1, 1);

            Assert.True(result.Success);
        }
    }
}

