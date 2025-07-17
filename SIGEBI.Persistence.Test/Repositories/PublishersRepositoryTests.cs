using Microsoft.EntityFrameworkCore;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities;
using SIGEBI.Persistence.Context;
using SIGEBI.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Assert = Xunit.Assert;

namespace SIGEBI.Persistence.Test.Repositories
{
    public class PublishersRepositoryTests
    {
        private SIGEBIDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<SIGEBIDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new SIGEBIDbContext(options);
        }



        //  Pruebas para SaveEntityAsync



        [Fact]
        public async Task SaveEntityAsync_Should_Fail_When_Name_Is_Empty()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new PublishersRepository(context);
            var publisher = new Publishers { PublisherName = "", CreatedBy = "TestUser" };

            // Act
            var result = await repo.SaveEntityAsync(publisher);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("El nombre de la editorial es obligatorio.", result.Message);
        }

        [Fact]
        public async Task SaveEntityAsync_Should_Fail_When_Email_Invalid()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new PublishersRepository(context);
            var publisher = new Publishers { PublisherName = "Test", Email = "correo-invalido", CreatedBy = "TestUser" };

            // Act
            var result = await repo.SaveEntityAsync(publisher);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("El correo electrónico no es válido.", result.Message);
        }

        [Fact]
        public async Task SaveEntityAsync_Should_Fail_When_Duplicate_Name_Or_Email()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new PublishersRepository(context);
            var publisher1 = new Publishers { PublisherName = "Duplicado", Email = "dup@mail.com", CreatedBy = "TestUser" };
            var publisher2 = new Publishers { PublisherName = "Duplicado", Email = "otro@mail.com", CreatedBy = "TestUser" };
            await repo.SaveEntityAsync(publisher1);

            // Act
            var result = await repo.SaveEntityAsync(publisher2);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Ya existe una editorial con ese nombre o correo electrónico.", result.Message);
        }

        // Pruebas para UpdateEntityAsync

        [Fact]
        public async Task UpdateEntityAsync_Should_Update_Valid_Publisher()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new PublishersRepository(context);
            var publisher = new Publishers { PublisherName = "Original", CreatedBy = "User" };
            await repo.SaveEntityAsync(publisher);

            // Act
            publisher.PublisherName = "Updated";
            var result = await repo.UpdateEntityAsync(publisher);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Updated", ((Publishers)result.Data).PublisherName);
        }

        [Fact]
        public async Task UpdateEntityAsync_Should_Fail_When_Publisher_Not_Found()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new PublishersRepository(context);
            var publisher = new Publishers { ID = 999, PublisherName = "NoExiste", CreatedBy = "User" };

            // Act
            var result = await repo.UpdateEntityAsync(publisher);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Editorial no encontrada.", result.Message);
        }

        [Fact]
        public async Task UpdateEntityAsync_Should_Fail_When_Name_Is_Empty()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new PublishersRepository(context);
            var publisher = new Publishers { PublisherName = "Original", CreatedBy = "User" };
            await repo.SaveEntityAsync(publisher);

            // Act
            publisher.PublisherName = "";
            var result = await repo.UpdateEntityAsync(publisher);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("El nombre de la editorial es obligatorio.", result.Message);
        }

        [Fact]
        public async Task UpdateEntityAsync_Should_Fail_When_Name_Too_Long()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new PublishersRepository(context);
            var publisher = new Publishers { PublisherName = "Original", CreatedBy = "User" };
            await repo.SaveEntityAsync(publisher);

            // Act
            publisher.PublisherName = new string('A', 101);
            var result = await repo.UpdateEntityAsync(publisher);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("El nombre de la editorial no puede superar los 100 caracteres.", result.Message);
        }

        [Fact]
        public async Task UpdateEntityAsync_Should_Fail_When_Email_Invalid()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new PublishersRepository(context);
            var publisher = new Publishers { PublisherName = "Original", CreatedBy = "User" };
            await repo.SaveEntityAsync(publisher);

            // Act
            publisher.Email = "correo-invalido";
            var result = await repo.UpdateEntityAsync(publisher);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("El correo electrónico no es válido.", result.Message);
        }

        [Fact]
        public async Task UpdateEntityAsync_Should_Fail_When_CreatedBy_Empty()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new PublishersRepository(context);
            var publisher = new Publishers { PublisherName = "Original", CreatedBy = "User" };
            await repo.SaveEntityAsync(publisher);

            // Act
            publisher.CreatedBy = "";
            var result = await repo.UpdateEntityAsync(publisher);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("El campo 'Creado por' es obligatorio.", result.Message);
        }

        [Fact]
        public async Task UpdateEntityAsync_Should_Fail_When_IsDeleted_True()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new PublishersRepository(context);
            var publisher = new Publishers { PublisherName = "Original", CreatedBy = "User" };
            await repo.SaveEntityAsync(publisher);

            // Act
            publisher.IsDeleted = true;
            var result = await repo.UpdateEntityAsync(publisher);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("No se puede modificar una editorial eliminada.", result.Message);
        }


        // Pruebas para RemoveEntityAsync

        [Fact]
        public async Task RemoveEntityAsync_Should_Remove_Publisher_Without_Books()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new PublishersRepository(context);
            var publisher = new Publishers { PublisherName = "ToRemove", CreatedBy = "User" };
            await repo.SaveEntityAsync(publisher);

            // Act
            var result = await repo.RemoveEntityAsync(publisher);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Editorial eliminada correctamente.", result.Message);
        }

        [Fact]
        public async Task RemoveEntityAsync_Should_Fail_When_Publisher_Not_Found()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new PublishersRepository(context);
            var publisher = new Publishers { ID = 999, PublisherName = "NoExiste", CreatedBy = "User" };

            // Act
            var result = await repo.RemoveEntityAsync(publisher);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Editorial no encontrada.", result.Message);
        }

        [Fact]
        public async Task RemoveEntityAsync_Should_Fail_When_Already_Deleted()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new PublishersRepository(context);
            var publisher = new Publishers { PublisherName = "ToDelete", CreatedBy = "User" };
            await repo.SaveEntityAsync(publisher);

            // Simula que la editorial ya está eliminada
            publisher.IsDeleted = true;
            context.Publishers.Update(publisher);
            await context.SaveChangesAsync();

            // Act
            var result = await repo.RemoveEntityAsync(publisher);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("La editorial ya está eliminada.", result.Message);
        }

        [Fact]
        public async Task RemoveEntityAsync_Should_Fail_When_Publisher_Has_Books()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new PublishersRepository(context);
            var publisher = new Publishers
            {
                PublisherName = "WithBooks",
                CreatedBy = "User",
                Books = new List<Books>
        {
            new Books
            {
                Title = "Libro1",
                ISBN = "1234567890",
                PublicationDate = DateTime.Now,
                CategoryId = 1,
                PublisherId = 1,
                TotalCopies = 1,
                AvailableCopies = 1,
                GeneralStatus = "Activo"
            }
        }
            };
            await repo.SaveEntityAsync(publisher);

            // Act
            var result = await repo.RemoveEntityAsync(publisher);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("No se puede eliminar una editorial con libros asociados.", result.Message);
        }

        [Fact]
        public async Task GetEntityByIdAsync_Should_Return_Publisher_When_Exists_And_Not_Deleted()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new PublishersRepository(context);
            var publisher = new Publishers { PublisherName = "Test", CreatedBy = "User" };
            await repo.SaveEntityAsync(publisher);

            // Act
            var found = await repo.GetEntityByIdAsync(publisher.ID);

            // Assert
            Assert.NotNull(found);
            Assert.Equal("Test", found.PublisherName);
        }

        [Fact]
        public async Task GetEntityByIdAsync_Should_Return_Null_When_Deleted()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new PublishersRepository(context);
            var publisher = new Publishers { PublisherName = "Test", CreatedBy = "User", IsDeleted = true };
            await context.Publishers.AddAsync(publisher);
            await context.SaveChangesAsync();

            // Act
            var found = await repo.GetEntityByIdAsync(publisher.ID);

            // Assert
            Assert.Null(found);
        }




        // Pruebas para GetAllAsync


        [Fact]
        public async Task GetAllAsync_Returns_Only_NotDeleted_Publishers()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new PublishersRepository(context);

            //  se agrega publishers dos activos y uno eliminado
            var publisher1 = new Publishers { PublisherName = "Activa1", CreatedBy = "User" };
            var publisher2 = new Publishers { PublisherName = "Activa2", CreatedBy = "User" };
            var publisher3 = new Publishers { PublisherName = "Eliminada", CreatedBy = "User", IsDeleted = true };

            await context.Publishers.AddRangeAsync(publisher1, publisher2, publisher3);
            await context.SaveChangesAsync();

            // Act
            var result = await repo.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.DoesNotContain(result, p => p.IsDeleted);
        }


        // Pruebas para Exists


        [Fact]
        public async Task Exists_Should_Return_True_If_Exists()
        {
            /// Arrange
            var context = GetDbContext();
            var repo = new PublishersRepository(context);
            await repo.SaveEntityAsync(new Publishers { PublisherName = "Exists", CreatedBy = "User" });

            // Act
            var exists = await repo.Exists(p => p.PublisherName == "Exists");

            // Assert
            Assert.True(exists);
        }

        [Fact]
        public async Task Exists_Should_Return_False_If_Not_Exists()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new PublishersRepository(context);

            // Act
            var exists = await repo.Exists(p => p.PublisherName == "NoExiste");

            // Assert
            Assert.False(exists);
        }



        
        // Pruebas para SearchByNameAsync
        

        [Fact]
        public async Task SearchByNameAsync_Should_Return_Matching_Publishers()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new PublishersRepository(context);
            await repo.SaveEntityAsync(new Publishers { PublisherName = "Alpha", CreatedBy = "User" });
            await repo.SaveEntityAsync(new Publishers { PublisherName = "Beta", CreatedBy = "User" });

            // Act
            var result = await repo.SearchByNameAsync("Alpha");

            // Assert
            Assert.Single(result);
            Assert.Equal("Alpha", result[0].PublisherName);
        }

        [Fact]
        public async Task SearchByNameAsync_Should_Return_Empty_When_No_Match()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new PublishersRepository(context);

            // Act
            var result = await repo.SearchByNameAsync("NoExiste");

            // Assert
            Assert.Empty(result);
        }


       
        // Pruebas para GetByEmailAsync
      

        [Fact]
        public async Task GetByEmailAsync_Should_Return_Publisher_When_Email_Exists()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new PublishersRepository(context);
            await repo.SaveEntityAsync(new Publishers { PublisherName = "EmailTest", Email = "test@mail.com", CreatedBy = "User" });

            // Act
            var found = await repo.GetByEmailAsync("test@mail.com");

            // Assert
            Assert.NotNull(found);
            Assert.Equal("test@mail.com", found.Email);
        }

        [Fact]
        public async Task GetByEmailAsync_Should_Return_Null_When_Email_Not_Exists()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new PublishersRepository(context);

            // Act
            var found = await repo.GetByEmailAsync("noexiste@mail.com");

            // Assert
            Assert.Null(found);
        }




        // Pruebas para ExistsByNameOrEmailAsync
        
        [Fact]
        public async Task ExistsByNameOrEmailAsync_Should_Return_True_If_Exists()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new PublishersRepository(context);
            await repo.SaveEntityAsync(new Publishers { PublisherName = "UniqueName", Email = "unique@mail.com", CreatedBy = "User" });

            // Act
            var exists = await repo.ExistsByNameOrEmailAsync("UniqueName", "unique@mail.com");

            // Assert
            Assert.True(exists);
        }

        [Fact]
        public async Task ExistsByNameOrEmailAsync_Should_Return_False_If_Not_Exists()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new PublishersRepository(context);

            // Act
            var exists = await repo.ExistsByNameOrEmailAsync("NoExiste", "noexiste@mail.com");

            // Assert
            Assert.False(exists);
        }



        // Pruebas para ExistsByNameOrEmailExceptIdAsync
     
        [Fact]
        public async Task ExistsByNameOrEmailExceptIdAsync_Should_Exclude_Id()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new PublishersRepository(context);
            var publisher = new Publishers { PublisherName = "ExceptName", Email = "except@mail.com", CreatedBy = "User" };
            await repo.SaveEntityAsync(publisher);

            // Act
            var exists = await repo.ExistsByNameOrEmailExceptIdAsync("ExceptName", "except@mail.com", publisher.ID);

            // Assert
            Assert.False(exists);
        }

       

    }
}
