using Microsoft.VisualStudio.TestTools.UnitTesting;
using SIGEBI.Domain.Entities;
using SIGEBI.Application.Validation;
using System;

namespace SIGEBI.Test.ValidationTests
{
    [TestClass]
    public class UserValidationTests
    {
        [TestMethod]
        public void IsValid_ShouldReturnTrue_WhenUserIsValid()
        {
            var user = new User
            {
                FullName = "John Doe",
                InstitutionalEmail = "john.doe@itla.edu.do",
                PasswordHash = "hashedpassword123",
                RoleId = 1,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "TestSystem",
                RegistrationDate = DateTime.UtcNow,
                InstitutionalIdentifier = "ID12345"
            };
            bool result = UserValidation.IsValid(user);
            Assert.IsTrue(result, "El usuario válido debería pasar la validación.");
        }

        [TestMethod]
        public void IsValid_ShouldReturnFalse_WhenFullNameIsInvalid()
        {
            var user = new User
            {
                FullName = "",
                InstitutionalEmail = "test@example.com",
                PasswordHash = "validhash123",
                RoleId = 1,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "TestSystem",
                RegistrationDate = DateTime.UtcNow
            };
            bool result = UserValidation.IsValid(user);
            Assert.IsFalse(result, "El usuario con nombre completo vacío no debería pasar la validación.");
        }

        [TestMethod]
        public void IsValid_ShouldReturnFalse_WhenEmailIsInvalid()
        {
            var user = new User
            {
                FullName = "Valid Name",
                InstitutionalEmail = "invalid-email",
                PasswordHash = "validhash123",
                RoleId = 1,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "TestSystem",
                RegistrationDate = DateTime.UtcNow
            };
            bool result = UserValidation.IsValid(user);
            Assert.IsFalse(result, "El usuario con email inválido no debería pasar la validación.");
        }

        [TestMethod]
        public void IsValid_ShouldReturnFalse_WhenPasswordHashIsTooShort()
        {
            var user = new User
            {
                FullName = "Valid Name",
                InstitutionalEmail = "test@example.com",
                PasswordHash = "short",
                RoleId = 1,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "TestSystem",
                RegistrationDate = DateTime.UtcNow
            };
            bool result = UserValidation.IsValid(user);
            Assert.IsFalse(result, "El usuario con password hash demasiado corto no debería pasar la validación.");
        }

        [TestMethod]
        public void IsValid_ShouldReturnFalse_WhenRoleIdIsInvalid()
        {
            var user = new User
            {
                FullName = "Valid Name",
                InstitutionalEmail = "test@example.com",
                PasswordHash = "validhash123",
                RoleId = 0,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "TestSystem",
                RegistrationDate = DateTime.UtcNow
            };
            bool result = UserValidation.IsValid(user);
            Assert.IsFalse(result, "El usuario con RoleId inválido no debería pasar la validación.");
        }

        [TestMethod]
        public void IsValid_ShouldReturnFalse_WhenCreatedByIsInvalid()
        {
            var user = new User
            {
                FullName = "Valid Name",
                InstitutionalEmail = "test@example.com",
                PasswordHash = "validhash123",
                RoleId = 1,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "",
                RegistrationDate = DateTime.UtcNow
            };
            bool result = UserValidation.IsValid(user);
            Assert.IsFalse(result, "El usuario con CreatedBy inválido no debería pasar la validación.");
        }
    }
}