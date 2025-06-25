using SIGEBI.Domain.Entities;
using SIGEBI.Persistence.Base;
using SIGEBI.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SIGEBI.Persistence.Repositori
{
    public class UserAccountService : IUserAccountService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserHistoryRepository _userHistoryRepository;

        public UserAccountService(IUserRepository userRepository, IUserHistoryRepository userHistoryRepository)
        {
            _userRepository = userRepository;
            _userHistoryRepository = userHistoryRepository;
        }

        public async Task<OperationResult<bool>> LoginAsync(string email, string password, string ipAddress, string userAgent)
        {
            User? user = null;
            bool isValid = false;
            string? failureReason = null;
            string? obtainedRole = null;

            try
            {
                user = await _userRepository.GetByEmailAsync(email);
                isValid = user != null && user.PasswordHash == password && user.IsActive;

                if (!isValid)
                {
                    if (user == null) failureReason = "Credenciales inválidas: Usuario no encontrado.";
                    else if (user.PasswordHash != password) failureReason = "Credenciales inválidas: Contraseña incorrecta.";
                    else if (!user.IsActive) failureReason = "Credenciales inválidas: Usuario inactivo.";
                }
                else
                {
                    obtainedRole = user.RoleId.ToString();
                }
            }
            catch (Exception ex)
            {
                isValid = false;
                failureReason = $"Error interno durante el login: {ex.Message}";
                // Considera loggear la excepción 'ex' aquí para depuración
            }

            // Registrar historial de login
            var history = new UserHistory
            {
                UserId = user?.UserId ?? 0,
                EnteredEmail = email,
                AttemptDate = DateTime.Now,
                IpAddress = ipAddress,
                UserAgent = userAgent,
                IsSuccessful = isValid,
                FailureReason = failureReason,
                ObtainedRole = obtainedRole
            };
            var addHistoryResult = await _userHistoryRepository.AddAsync(history);

            if (!addHistoryResult.Success)
            {
                // Si falla el registro del historial, loggearlo.
                // Podrías devolver un OperationResult.Fail más específico si este fallo es crítico.
                Console.WriteLine($"Advertencia: Falló el registro del historial de login para {email}: {addHistoryResult.Message}");
            }

            return isValid ? OperationResult<bool>.Ok(true, "Login exitoso.") : OperationResult<bool>.Fail(failureReason ?? "Fallo de login desconocido.", false);
        }

        public async Task<OperationResult> LogoutAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user != null)
            {
                var history = new UserHistory
                {
                    UserId = userId,
                    EnteredEmail = user.InstitutionalEmail,
                    AttemptDate = DateTime.Now,
                    IsSuccessful = true,
                    FailureReason = null,
                    ObtainedRole = user.RoleId.ToString(),
                    IpAddress = null,
                    UserAgent = null
                };
                return await _userHistoryRepository.AddAsync(history);
            }
            return OperationResult.Fail("Usuario no encontrado para registrar logout.");
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetByEmailAsync(email);
        }

        public async Task<IEnumerable<UserHistory>> GetUserHistoryAsync(int userId)
        {
            return await _userHistoryRepository.GetByUserIdAsync(userId);
        }

        public async Task<OperationResult> RegisterAsync(User user)
        {
            var existingUser = await _userRepository.GetByEmailAsync(user.InstitutionalEmail);
            if (existingUser != null)
            {
                return OperationResult.Fail($"El correo '{user.InstitutionalEmail}' ya está registrado.");
            }

            user.RegistrationDate = DateTime.Now;
            user.IsActive = true;
            user.IsDeleted = false;

            return await _userRepository.AddAsync(user);
        }

        public async Task<OperationResult> UpdateUserAsync(User user)
        {
            var existing = await _userRepository.GetByIdAsync(user.UserId);
            if (existing == null)
            {
                return OperationResult.Fail("Usuario no encontrado para actualizar.");
            }

            existing.FullName = user.FullName;
            existing.InstitutionalEmail = user.InstitutionalEmail;
            existing.PasswordHash = user.PasswordHash;
            existing.InstitutionalIdentifier = user.InstitutionalIdentifier;
            existing.RoleId = user.RoleId;
            existing.IsActive = user.IsActive;
            existing.UpdatedAt = DateTime.Now;
            existing.UpdatedBy = user.UpdatedBy;

            return await _userRepository.UpdateAsync(existing);
        }

        public async Task<OperationResult> DeleteUserAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return OperationResult.Fail("Usuario no encontrado para eliminar.");
            }

            user.IsDeleted = true;
            user.DeletedAt = DateTime.Now;

            return await _userRepository.UpdateAsync(user);
        }
    }
}