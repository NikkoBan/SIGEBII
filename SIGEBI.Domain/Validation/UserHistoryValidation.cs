using SIGEBI.Domain.Entities;
using System;

namespace SIGEBI.Application.Validation
{
    public static class UserHistoryValidation
    {
        public static bool IsValid(UserHistory history)
        {
            if (history == null)
            {
                return false;
            }

            // UserId debe ser válido (positivo)
            if (history.UserId <= 0)
            {
                return false;
            }

            // EnteredEmail no puede ser nulo si es una entrada importante
            // (tu DB lo permite nulo, pero la lógica puede requerirlo)
            if (string.IsNullOrWhiteSpace(history.EnteredEmail))
            {
                return false;
            }

            // AttemptDate no puede ser una fecha por defecto inválida (aunque DateTime.Now lo asegura)
            if (history.AttemptDate == DateTime.MinValue)
            {
                return false;
            }

            // IsSuccessful siempre debe ser un booleano

            // Si es un registro de éxito, FailureReason debería ser nulo
            if (history.IsSuccessful && !string.IsNullOrWhiteSpace(history.FailureReason))
            {
                return false;
            }

            // Si pasa todas las validaciones
            return true;
        }
    }
}