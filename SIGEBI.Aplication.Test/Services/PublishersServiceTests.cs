using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using SIGEBI.Application.DTOs.PublishersDTOs;
using SIGEBI.Application.Services;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities;
using SIGEBI.Persistence.Interfaces;
using Xunit;

namespace SIGEBI.Aplication.Test.Services
{
    public class PublishersServiceTests
    {
        private readonly Mock<IPublishersRepository> _repoMock;
        private readonly PublishersService _service;

        public PublishersServiceTests()
        {
            _repoMock = new Mock<IPublishersRepository>();
            _service = new PublishersService(_repoMock.Object);
        }




        // CreateAsync

        [Fact]
        public async Task CreateAsync_Should_Throw_When_Dto_Is_Null()
        {
            // Arrange y Act
            Func<Task> act = async () => await _service.CreateAsync(null);

            // Assert
            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task CreateAsync_Should_Fail_When_Duplicate_Name_Or_Email()
        {
            var dto = new CreationPublisherDto { PublisherName = "Duplicado", Email = "dup@mail.com" };
            _repoMock.Setup(r => r.ExistsByNameOrEmailAsync(dto.PublisherName, dto.Email)).ReturnsAsync(true);

            var result = await _service.CreateAsync(dto);

            result.Success.Should().BeFalse();
            result.Message.Should().Be("Ya existe una editorial con ese nombre o correo.");
        }

        [Fact]
        public async Task CreateAsync_Should_Return_Success_When_Valid()
        {
            // Arrange
            var dto = new CreationPublisherDto { PublisherName = "Nuevo", Email = "nuevo@mail.com" };
            _repoMock.Setup(r => r.ExistsByNameOrEmailAsync(dto.PublisherName, dto.Email)).ReturnsAsync(false);
            _repoMock.Setup(r => r.SaveEntityAsync(It.IsAny<Publishers>()))
                .ReturnsAsync(OperationResult.Ok("Editorial guardada correctamente."));

            // Act
            var result = await _service.CreateAsync(dto);

            // Assert
            result.Success.Should().BeTrue();
            result.Message.Should().Be("Editorial guardada correctamente.");
        }




        // UpdateAsync

        [Fact]
        public async Task UpdateAsync_Should_Throw_When_Dto_Is_Null()
        {
            Func<Task> act = async () => await _service.UpdateAsync(null);
            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task UpdateAsync_Should_Fail_When_Publisher_Not_Found()
        {
            // Arrange
            var dto = new UpdatePublisherDto { Id = 1, PublisherName = "Update", Email = "update@mail.com" };
            _repoMock.Setup(r => r.GetEntityByIdAsync(dto.Id)).ReturnsAsync((Publishers)null);

            // Act
            var result = await _service.UpdateAsync(dto);

            // Assert
            result.Success.Should().BeFalse();
            result.Message.Should().Be("Editorial no encontrada.");
        }

        [Fact]
        public async Task UpdateAsync_Should_Fail_When_Duplicate_Name_Or_Email()
        {
            // Arrange
            var dto = new UpdatePublisherDto { Id = 1, PublisherName = "Update", Email = "update@mail.com" };
            var publisher = new Publishers { ID = 1, PublisherName = "Update", Email = "update@mail.com" };
            _repoMock.Setup(r => r.GetEntityByIdAsync(dto.Id)).ReturnsAsync(publisher);
            _repoMock.Setup(r => r.ExistsByNameOrEmailExceptIdAsync(dto.PublisherName, dto.Email, dto.Id)).ReturnsAsync(true);

            // Act
            var result = await _service.UpdateAsync(dto);

            // Assert
            result.Success.Should().BeFalse();
            result.Message.Should().Be("Ya existe otra editorial con ese nombre o correo.");
        }

        [Fact]
        public async Task UpdateAsync_Should_Return_Success_When_Valid()
        {
            // Arrange
            var dto = new UpdatePublisherDto { Id = 1, PublisherName = "Update", Email = "update@mail.com" };
            var publisher = new Publishers { ID = 1, PublisherName = "Update", Email = "update@mail.com" };
            _repoMock.Setup(r => r.GetEntityByIdAsync(dto.Id)).ReturnsAsync(publisher);
            _repoMock.Setup(r => r.ExistsByNameOrEmailExceptIdAsync(dto.PublisherName, dto.Email, dto.Id)).ReturnsAsync(false);
            _repoMock.Setup(r => r.UpdateEntityAsync(It.IsAny<Publishers>()))
                .ReturnsAsync(OperationResult.Ok("Editorial actualizada correctamente."));

            // Act
            var result = await _service.UpdateAsync(dto);

            // Assert
            result.Success.Should().BeTrue();
            result.Message.Should().Be("Editorial actualizada correctamente.");
        }




        // DeleteAsync

        [Fact]
        public async Task DeleteAsync_Should_Throw_When_Dto_Is_Null()
        {
            Func<Task> act = async () => await _service.DeleteAsync(null);
            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task DeleteAsync_Should_Fail_When_Publisher_Not_Found()
        {
            // Arrange
            var dto = new RemovePublisherDto { Id = 1, DeletedBy = "admin", Reason = "Duplicado" };
            _repoMock.Setup(r => r.GetEntityByIdAsync(dto.Id)).ReturnsAsync((Publishers)null);

            // Act
            var result = await _service.DeleteAsync(dto);

            // Assert
            result.Success.Should().BeFalse();
            result.Message.Should().Be("Editorial no encontrada.");
        }

        [Fact]
        public async Task DeleteAsync_Should_Return_Success_When_Valid()
        {
            // Arrange
            var dto = new RemovePublisherDto { Id = 1, DeletedBy = "admin", Reason = "Duplicado" };
            var publisher = new Publishers { ID = 1, PublisherName = "ToDelete" };
            _repoMock.Setup(r => r.GetEntityByIdAsync(dto.Id)).ReturnsAsync(publisher);
            _repoMock.Setup(r => r.UpdateEntityAsync(It.IsAny<Publishers>()))
                .ReturnsAsync(OperationResult.Ok("Editorial eliminada correctamente."));

            // Act
            var result = await _service.DeleteAsync(dto);

            // Assert
            result.Success.Should().BeTrue();
            result.Message.Should().Be("Editorial eliminada correctamente.");
        }


        


        // GetAllAsync

        [Fact]
        public async Task GetAllAsync_Should_Return_All_Publishers()
        {
            // Arrange
            var publishers = new List<Publishers>
            {
                new Publishers { ID = 1, PublisherName = "Editorial 1" },
                new Publishers { ID = 2, PublisherName = "Editorial 2" }
            };
            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(publishers);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            result.Should().HaveCount(2);
            result.Should().Contain(p => p.PublisherName == "Editorial 1");
            result.Should().Contain(p => p.PublisherName == "Editorial 2");
        }

        [Fact]
        public async Task GetAllAsync_Should_Return_Empty_When_No_Publishers()
        {
            // Arrange
            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Publishers>());

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            result.Should().BeEmpty();
        }




        // GetByIdAsync

        [Fact]
        public async Task GetByIdAsync_Should_Return_Publisher_When_Exists()
        {
            // Arrange
            var publisher = new Publishers { ID = 1, PublisherName = "Editorial 1" };
            _repoMock.Setup(r => r.GetEntityByIdAsync(1)).ReturnsAsync(publisher);

            // Act
            var result = await _service.GetByIdAsync(1);

            // Assert
            result.Should().NotBeNull();
            result.PublisherName.Should().Be("Editorial 1");
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Null_When_Not_Exists()
        {
            // Arrange
            _repoMock.Setup(r => r.GetEntityByIdAsync(1)).ReturnsAsync((Publishers)null);

            // Act
            var result = await _service.GetByIdAsync(1);

            // Assert
            result.Should().BeNull();
        }





        // SearchByNameAsync

        [Fact]
        public async Task SearchByNameAsync_Should_Return_Matching_Publishers()
        {
            // Arrange
            var publishers = new List<Publishers>
            {
                new Publishers { ID = 1, PublisherName = "Alpha" },
                new Publishers { ID = 2, PublisherName = "Beta" }
            };
            _repoMock.Setup(r => r.SearchByNameAsync("Alpha")).ReturnsAsync(new List<Publishers> { publishers[0] });

            // Act
            var result = await _service.SearchByNameAsync("Alpha");

            // Assert
            result.Should().ContainSingle();
            result[0].PublisherName.Should().Be("Alpha");
        }

        [Fact]
        public async Task SearchByNameAsync_Should_Return_Empty_When_No_Match()
        {
            // Arrange
            _repoMock.Setup(r => r.SearchByNameAsync("NoExiste")).ReturnsAsync(new List<Publishers>());

            // Act
            var result = await _service.SearchByNameAsync("NoExiste");

            // Assert
            result.Should().BeEmpty();
        }




        // GetByEmailAsync

        [Fact]
        public async Task GetByEmailAsync_Should_Return_Publisher_When_Email_Exists()
        {
            // Arrange
            var publisher = new Publishers { ID = 1, PublisherName = "EmailTest", Email = "test@mail.com" };
            _repoMock.Setup(r => r.GetByEmailAsync("test@mail.com")).ReturnsAsync(publisher);

            // Act
            var result = await _service.GetByEmailAsync("test@mail.com");


            result.Should().NotBeNull();
            result.Email.Should().Be("test@mail.com");
        }

        [Fact]
        public async Task GetByEmailAsync_Should_Return_Null_When_Email_Not_Exists()
        {
            // Arrange
            _repoMock.Setup(r => r.GetByEmailAsync("noexiste@mail.com")).ReturnsAsync((Publishers)null);

            // Act
            var result = await _service.GetByEmailAsync("noexiste@mail.com");

            // Assert
            result.Should().BeNull();
        }

        
    }
}