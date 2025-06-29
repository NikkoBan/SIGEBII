using SIGEBI.Domain.Entities;
using SIGEBI.Persistence.Base;
using SIGEBI.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SIGEBI.Application.Validation; 

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
            }

            if (user != null)
            {
                var history = new UserHistory
                {
                    UserId = user.UserId,
                    EnteredEmail = email,
                    AttemptDate = DateTime.Now,
                    IpAddress = ipAddress,
                    UserAgent = userAgent,
                    IsSuccessful = isValid,
                    FailureReason = failureReason,
                    ObtainedRole = obtainedRole,
                    CreatedAt = DateTime.Now,
                    CreatedBy = "System"
                };
                // Validar historial antes de añadir
                // if (!UserHistoryValidation.IsValid(history)) { /* Manejar error de validación de historial */ }
                var addHistoryResult = await _userHistoryRepository.AddAsync(history);

                if (!addHistoryResult.Success)
                {
                    Console.WriteLine($"Advertencia: Falló el registro del historial de login para {email}: {addHistoryResult.Message}");
                }
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
                    UserAgent = null,
                    CreatedAt = DateTime.Now,
                    CreatedBy = "System"
                };
                //  Validar historial antes de añadir
                // if (!UserHistoryValidation.IsValid(history)) { /* Manejar error de validación de historial */ }
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
            //  INICIA LA VALIDACIÓN
            if (!UserValidation.IsValid(user))
            {
                // Aquí podrías ser más específico sobre qué campo falló si lo deseas
                return OperationResult.Fail("Datos de usuario inválidos para el registro.");
            }
            // TERMINA LA VALIDACIÓN 

            var existingUser = await _userRepository.GetByEmailAsync(user.InstitutionalEmail);
            if (existingUser != null)
            {
                return OperationResult.Fail($"El correo '{user.InstitutionalEmail}' ya está registrado.");
            }

            user.RegistrationDate = DateTime.Now;
            user.IsActive = true;
            user.IsDeleted = false;
            user.CreatedAt = DateTime.Now;
            user.CreatedBy = "System"; // Considera pasar el "CreatedBy" desde el API o contexto de seguridad

            var result = await _userRepository.AddAsync(user);

            if (result.Success)
            {
                if (user.UserId > 0)
                {
                    var userHistory = new UserHistory
                    {
                        UserId = user.UserId,
                        EnteredEmail = user.InstitutionalEmail,
                        AttemptDate = DateTime.Now,
                        IsSuccessful = true,
                        FailureReason = null,
                        ObtainedRole = user.RoleId.ToString(),
                        CreatedAt = DateTime.Now,
                        CreatedBy = "System"
                    };
                    // Validar historial antes de añadir
                    // if (!UserHistoryValidation.IsValid(history)) { /* Manejar error de validación de historial */ }
                    var addHistoryResult = await _userHistoryRepository.AddAsync(userHistory);
                    if (!addHistoryResult.Success)
                    {
                        Console.WriteLine($"Advertencia: Falló el registro del historial para el nuevo usuario {user.InstitutionalEmail}: {addHistoryResult.Message}");
                    }
                }
            }

            return result;
        }

        public async Task<OperationResult> UpdateUserAsync(User user)
        {
            // INICIA LA VALIDACIÓN
            if (!UserValidation.IsValid(user))
            {
                return OperationResult.Fail("Datos de usuario inválidos para la actualización.");
            }
            // TERMINA LA VALIDACIÓN

            var existing = await _userRepository.GetByIdAsync(user.UserId);
            if (existing == null)
            {
                return OperationResult.Fail("Usuario no encontrado para actualizar.");
            }

            // Actualiza solo las propiedades permitidas.
            existing.FullName = user.FullName;
            existing.InstitutionalEmail = user.InstitutionalEmail;
            existing.InstitutionalIdentifier = user.InstitutionalIdentifier;
            existing.RoleId = user.RoleId;
            existing.IsActive = user.IsActive;
            existing.UpdatedAt = DateTime.Now;
            existing.UpdatedBy = user.UpdatedBy ?? "System";
            existing.IsDeleted = user.IsDeleted;
            existing.DeletedAt = user.DeletedAt;
            existing.DeletedBy = user.DeletedBy;

            var result = await _userRepository.UpdateAsync(existing);

            if (result.Success)
            {
                var userHistory = new UserHistory
                {
                    UserId = user.UserId,
                    EnteredEmail = user.InstitutionalEmail,
                    AttemptDate = DateTime.Now,
                    IsSuccessful = true,
                    FailureReason = null,
                    ObtainedRole = user.RoleId.ToString(),
                    CreatedAt = DateTime.Now,
                    CreatedBy = existing.UpdatedBy ?? "System"
                };
                // Validar historial antes de añadir
                // if (!UserHistoryValidation.IsValid(history)) { /* Manejar error de validación de historial */ }
                var addHistoryResult = await _userHistoryRepository.AddAsync(userHistory);
                if (!addHistoryResult.Success)
                    Console.WriteLine($"Advertencia: Falló el registro del historial de actualización para el usuario {user.InstitutionalEmail}: {addHistoryResult.Message}");
            }

            return result;
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
            user.DeletedBy = user.DeletedBy ?? "System";

            var result = await _userRepository.UpdateAsync(user);

            if (result.Success)
            {
                var userHistory = new UserHistory
                {
                    UserId = userId,
                    EnteredEmail = user.InstitutionalEmail,
                    AttemptDate = DateTime.Now,
                    IsSuccessful = true,
                    FailureReason = null,
                    ObtainedRole = user.RoleId.ToString(),
                    CreatedAt = DateTime.Now,
                    CreatedBy = user.DeletedBy ?? "System"
                };
                // Validar historial antes de añadir
                // if (!UserHistoryValidation.IsValid(history)) { /* Manejar error de validación de historial */ }
                var addHistoryResult = await _userHistoryRepository.AddAsync(userHistory);
                if (!addHistoryResult.Success)
                {
                    Console.WriteLine($"Advertencia: Falló el registro del historial de borrado para el usuario {user.InstitutionalEmail}: {addHistoryResult.Message}");
                }
            }

            return result;
        }
    }
}