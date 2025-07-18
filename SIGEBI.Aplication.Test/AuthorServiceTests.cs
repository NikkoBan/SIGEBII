using Moq;
using Xunit;
using SIGEBI.Application.Services;
using SIGEBI.Domain.IRepository;
using SIGEBI.Application.Dtos.AuthorDTO;
using Microsoft.Extensions.Logging;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities.Configuration;
using Assert = Xunit.Assert;
using SIGEBI.Persistence.Repositories;



public class AuthorServiceTests
{
    private readonly Mock<IAuthorRepository> _authorRepoMock;
    private readonly Mock<ILogger<AuthorService>> _loggerMock;
    private readonly AuthorService _authorService;

    public AuthorServiceTests()
    {
        _authorRepoMock = new Mock<IAuthorRepository>();
        _loggerMock = new Mock<ILogger<AuthorService>>();
        _authorService = new AuthorService(_authorRepoMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnSuccess_WhenAuthorIsCreated()
    {
        // Arrange
        var createDto = new CreateAuthorDTO
        {
            FirstName = "Diomar",
            LastName = "García Márquez",
            BirthDate = new DateTime(1927, 3, 6),
            Nationality = "Domi"
        };

        _authorRepoMock
            .Setup(repo => repo.CreateAsync(It.IsAny<Authors>()))
            .ReturnsAsync(OperationResult.SuccessResult(new Authors(), "creado"));

        // Act
        var result = await _authorService.CreateAsync(createDto);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("creado", result.Message);
        _authorRepoMock.Verify(r => r.CreateAsync(It.IsAny<Authors>()), Times.Once);
    }

    
    [Fact]
    public async Task UpdateAsyncShouldReturnSuccessWhenAuthorUpdated()
    {
        // Arrange
        int authorId = 2;
        var updateDto = new UpdateAuthorDTO
        {
            AuthorId = authorId,
            FirstName = "Mario",
            LastName = "Vargas Llosa",
            BirthDate = new DateTime(1936, 3, 28),
            Nationality = "Peruano"
        };

        var existingAuthor = new Authors
        {
            Id = authorId,
            FirstName = "Mario",
            LastName = "Vargas",
            BirthDate = new DateTime(1936, 3, 28),
            Nationality = "Peruano"
        };

        _authorRepoMock.Setup(r => r.GetByIdAsync(authorId))
            .ReturnsAsync(OperationResult.SuccessResult(existingAuthor));

        _authorRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Authors>()))
            .ReturnsAsync(OperationResult.SuccessResult(existingAuthor, "actualizado"));

        // Act
        var result = await _authorService.UpdateAsync(authorId, updateDto);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("actualizado", result.Message);
    }
    [Fact]
    public async Task GetByIdAsync_ShouldReturnAuthor_WhenAuthorExists()
    {
        // Arrange
        int authorId = 1;
        var author = new Authors
        {
            Id = authorId,
            FirstName = "Isabel",
            LastName = "Allende",
            BirthDate = new DateTime(1942, 8, 2),
            Nationality = "Chilena"
        };

        _authorRepoMock
            .Setup(r => r.GetByIdAsync(authorId))
            .ReturnsAsync(OperationResult.SuccessResult(author));

        // Act
        var result = await _authorService.GetByIdAsync(authorId);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Equal(author.FirstName, ((AuthorDTO)result.Data).FirstName);
    }
    [Fact]
    public async Task DeleteAsync_ShouldReturnSuccess_WhenAuthorDeleted()
    {
        // Arrange
        int authorId = 3;
        var author = new Authors { Id = authorId, FirstName = "Laura", LastName = "Esquivel" };

        _authorRepoMock.Setup(r => r.GetByIdAsync(authorId))
            .ReturnsAsync(OperationResult.SuccessResult(author));

        _authorRepoMock.Setup(r => r.DeleteAsync(authorId))
            .ReturnsAsync(OperationResult.SuccessResult(null, "eliminado"));

        // Act
        var result = await _authorService.DeleteAsync(authorId);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("eliminado", result.Message);
    }
    [Fact]
    public async Task GetAllAsync_ShouldReturnAllAuthors()
    {
        // Arrange
        var authors = new List<Authors>
    {
        new Authors { Id = 1, FirstName = "Autor1", LastName = "A" },
        new Authors { Id = 2, FirstName = "Autor2", LastName = "B" }
    };

        _authorRepoMock.Setup(r => r.GetAllAsync())
            .ReturnsAsync(OperationResult.SuccessResult(authors));

        // Act
        var result = await _authorService.GetAllAsync();

        // Assert
        Assert.True(result.Success);
        var authorList = Assert.IsAssignableFrom<IEnumerable<AuthorDTO>>(result.Data);
        Assert.Equal(2, authorList.Count());
    }
    [Fact]
    
    public async Task CheckDuplicateAsync_ShouldReturnTrue_WhenDuplicateExists()
    {
        // Arrange
        string firstName = "Gabriel";
        string lastName = "García Márquez";
        DateTime birthDate = new(1927, 3, 6);

        _authorRepoMock.Setup(repo =>
            repo.CheckDuplicateAuthorAsync(firstName, lastName, birthDate))
            .ReturnsAsync(true);

        // Act
        var result = await _authorService.CheckDuplicateAsync(firstName, lastName, birthDate);

        // Assert
        Assert.True(result.Success);
    }
    [Fact]
    
    public async Task CheckDuplicateForUpdateAsync_ShouldReturnFalse_WhenNoDuplicateExists()
    {
        // Arrange
        int authorId = 1;
        string firstName = "Isabel";
        string lastName = "Allende";
        DateTime birthDate = new DateTime(1942, 8, 2);

        _authorRepoMock.Setup(repo =>
            repo.CheckDuplicateAuthorForUpdateAsync(authorId, firstName, lastName, birthDate))
            .ReturnsAsync(false);

        // Act
        var result = await _authorService.CheckDuplicateForUpdateAsync(authorId, firstName, lastName, birthDate);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.IsType<bool>(result.Data);
        Assert.False((bool)result.Data);
    }

    [Fact]

   
    public async Task GetGenresByAuthorAsync_ShouldReturnGenres_WhenAuthorExists()
    {
        // Arrange
        int authorId = 1;
        var expectedGenres = new List<string> { "Realismo Mágico", "Narrativa Histórica", "Ensayo" };

        _authorRepoMock.Setup(r => r.ExistsAsync(It.IsAny<int>())).ReturnsAsync(true);
        _authorRepoMock.Setup(r => r.GetGenresByAuthorAsync(It.IsAny<int>())).ReturnsAsync(expectedGenres);

        // Act
        var result = await _authorService.GetGenresByAuthorAsync(authorId);

        // Assert
       

        Assert.NotNull(result);
        Assert.True(result.Success, $"Expected Success=true but got Success={result.Success} with message: {result.Message}");
        Assert.NotNull(result.Data);

        var genres = Assert.IsAssignableFrom<IEnumerable<string>>(result.Data);
        Assert.Equal(3, genres.Count());
        Assert.Contains("Realismo Mágico", genres);
        Assert.Contains("Narrativa Histórica", genres);
        Assert.Contains("Ensayo", genres);
    }





    [Fact]
    public async Task ExistsAsync_ShouldReturnTrue_WhenAuthorExists()
    {
        // Arrange
        int authorId = 5;

        _authorRepoMock.Setup(repo => repo.ExistsAsync(authorId))
            .ReturnsAsync(true);

        // Act
        var result = await _authorService.ExistsAsync(authorId);

        // Assert
        Assert.True(result.Success);
    }

}





