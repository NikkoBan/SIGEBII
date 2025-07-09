using AutoMapper;
using SIGEBI.Application.DTOsAplication.UserDTOs;
using SIGEBI.Application.Interfaces;
using SIGEBI.Application.Validation;
using SIGEBI.Domain.Entities;
using SIGEBI.Persistence.Base;
using SIGEBI.Persistence.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace SIGEBI.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<OperationResult<UserDisplayDto>> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null || user.IsDeleted)
            {
                return OperationResult<UserDisplayDto>.Fail("Usuario no encontrado o eliminado.", null, 404);
            }
            var userDto = _mapper.Map<UserDisplayDto>(user);
            return OperationResult<UserDisplayDto>.Ok(userDto);
        }

        public async Task<OperationResult<IEnumerable<UserDisplayDto>>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync(u => !u.IsDeleted && u.IsActive);
            var userDtos = _mapper.Map<IEnumerable<UserDisplayDto>>(users);
            return OperationResult<IEnumerable<UserDisplayDto>>.Ok(userDtos);
        }

        public async Task<OperationResult<UserDisplayDto>> CreateUserAsync(UserCreationDto userDto)
        {
            var user = _mapper.Map<User>(userDto);

            if (!UserValidation.IsValid(user))
            {
                return OperationResult<UserDisplayDto>.Fail("Datos de usuario inválidos.", null, 400);
            }

            var existingUserByEmail = await _userRepository.GetByEmailAsync(user.InstitutionalEmail);
            if (existingUserByEmail != null)
            {
                return OperationResult<UserDisplayDto>.Fail($"El correo '{user.InstitutionalEmail}' ya está registrado.", null, 409);
            }

            // NOTA DE SEGURIDAD: Aquí se DEBE hashear la contraseña
            // user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
            user.PasswordHash = userDto.Password; // Temporal, reemplazar con hashing real.

            user.RegistrationDate = DateTime.Now;
            user.CreatedAt = DateTime.Now;
            user.CreatedBy = "API_System";
            user.IsActive = true;
            user.IsDeleted = false;

            var result = await _userRepository.AddAsync(user);

            if (!result.Success)
            {
                return OperationResult<UserDisplayDto>.Fail(result.Message ?? "Error al crear el usuario.", null, result.StatusCode);
            }

            var createdUserDto = _mapper.Map<UserDisplayDto>(user);
            return OperationResult<UserDisplayDto>.Ok(createdUserDto, "Usuario creado exitosamente.");
        }

        public async Task<OperationResult> UpdateUserAsync(int id, UserUpdateDto userDto)
        {
            var existingUser = await _userRepository.GetByIdAsync(id);
            if (existingUser == null || existingUser.IsDeleted)
            {
                return OperationResult.Fail("Usuario no encontrado o eliminado.", null, 404);
            }

            _mapper.Map(userDto, existingUser);

            if (!UserValidation.IsValid(existingUser))
            {
                return OperationResult.Fail("Datos de usuario inválidos para la actualización.", null, 400);
            }

            if (existingUser.InstitutionalEmail != userDto.InstitutionalEmail)
            {
                var existingUserByEmail = await _userRepository.GetByEmailAsync(userDto.InstitutionalEmail);
                if (existingUserByEmail != null && existingUserByEmail.UserId != id)
                {
                    return OperationResult.Fail($"El correo '{userDto.InstitutionalEmail}' ya está registrado por otro usuario.", null, 409);
                }
            }

            existingUser.UpdatedAt = DateTime.Now;
            existingUser.UpdatedBy = userDto.UpdatedBy ?? "API_System";

            var result = await _userRepository.UpdateAsync(existingUser);

            if (!result.Success)
            {
                return OperationResult.Fail(result.Message ?? "Error al actualizar el usuario.", null, result.StatusCode);
            }
            return OperationResult.Ok("Usuario actualizado exitosamente.");
        }

        public async Task<OperationResult> DeleteUserAsync(int id)
        {
            var userToDelete = await _userRepository.GetByIdAsync(id);
            if (userToDelete == null || userToDelete.IsDeleted)
            {
                return OperationResult.Fail("Usuario no encontrado o ya eliminado.", null, 404);
            }

            userToDelete.IsDeleted = true;
            userToDelete.DeletedAt = DateTime.Now;
            userToDelete.DeletedBy = "API_System";

            var result = await _userRepository.UpdateAsync(userToDelete);

            if (!result.Success)
            {
                return OperationResult.Fail(result.Message ?? "Error al eliminar el usuario.", null, result.StatusCode);
            }
            return OperationResult.Ok("Usuario eliminado lógicamente exitosamente.");
        }

        public async Task<OperationResult<UserDisplayDto>> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null || user.IsDeleted)
            {
                return OperationResult<UserDisplayDto>.Fail("Usuario no encontrado o eliminado.", null, 404);
            }
            var userDto = _mapper.Map<UserDisplayDto>(user);
            return OperationResult<UserDisplayDto>.Ok(userDto);
        }
    }
}