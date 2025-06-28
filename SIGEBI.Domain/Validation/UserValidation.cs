using SIGEBI.Domain.Entities;
using System.Text.RegularExpressions; // Para validación de email si quieres

namespace SIGEBI.Application.Validation
{
    public static class UserValidation
    {
        public static bool IsValid(User user)
        {
            // Validaciones básicas para la entidad User
            if (user == null)
            {
                return false;
            }

            // FullName no puede ser nulo o vacío, y con longitud razonable
            if (string.IsNullOrWhiteSpace(user.FullName) || user.FullName.Length < 3 || user.FullName.Length > 200)
            {
                return false;
            }

            // InstitutionalEmail no puede ser nulo o vacío y debe ser un formato de email válido
            if (string.IsNullOrWhiteSpace(user.InstitutionalEmail) || !IsValidEmail(user.InstitutionalEmail))
            {
                return false;
            }

            // PasswordHash no puede ser nulo o vacío (asumiendo que ya está hasheado)
            if (string.IsNullOrWhiteSpace(user.PasswordHash) || user.PasswordHash.Length < 6) // Longitud mínima para hash
            {
                return false;
            }

            // RoleId debe ser un valor positivo (o un ID de rol válido si conoces los rangos)
            if (user.RoleId <= 0)
            {
                return false;
            }

            // IsActive siempre debe ser un booleano (no necesita comprobación explícita)

            // CreatedBy no puede ser nulo o vacío
            if (string.IsNullOrWhiteSpace(user.CreatedBy) || user.CreatedBy.Length < 2)
            {
                return false;
            }

            // Si pasa todas las validaciones
            return true;
        }

        // Método auxiliar para validar formato de email
        private static bool IsValidEmail(string email)
        {
            // Expresión regular simple para validar formato de email
            // Considera usar librerías más robustas para validación de email real si es crítico.
            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, emailPattern);
        }
    }
}