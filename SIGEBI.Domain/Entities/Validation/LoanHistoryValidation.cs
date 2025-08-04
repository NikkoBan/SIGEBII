using System;
using System.Collections.Generic;
using SIGEBI.Domain.Entities;

namespace SIGEBI.Domain.Validations
{
    public static class LoanHistoryValidation
    {
        public static List<string> ValidateLoanHistory(LoanHistory entity)
        {
            var errors = new List<string>();

            if (entity == null)
            {
                errors.Add("La entidad LoanHistory no puede ser null.");
                return errors;
            }

            if (entity.LoanId <= 0)
                errors.Add("El LoanId debe ser mayor que cero.");

            if (entity.StatusId <= 0)
                errors.Add("El StatusId debe ser mayor que cero.");

            if (string.IsNullOrWhiteSpace(entity.ChangedBy))
                errors.Add("El campo ChangedBy es obligatorio.");

            if (entity.ChangedAt == default(DateTime))
                errors.Add("El campo ChangedAt es obligatorio.");

            // Opcionales
            if (entity.Notes != null && entity.Notes.Length > 500)
                errors.Add("El campo Notes no puede tener más de 500 caracteres.");

            if (entity.BookId <= 0)
                errors.Add("El BookId debe ser mayor que cero.");

            if (entity.UserId <= 0)
                errors.Add("El UserId debe ser mayor que cero.");

            if (entity.LoanDate == default(DateTime))
                errors.Add("El campo LoanDate es obligatorio.");

            if (entity.DueDate == default(DateTime))
                errors.Add("El campo DueDate es obligatorio.");

            // ReturnDate puede ser null o default si aún no se ha devuelto el libro, así que no lo hacemos obligatorio.

            if (entity.FinalStatus != null && entity.FinalStatus.Length > 100)
                errors.Add("El campo FinalStatus no puede tener más de 100 caracteres.");

            if (entity.Observations != null && entity.Observations.Length > 500)
                errors.Add("El campo Observations no puede tener más de 500 caracteres.");

            return errors;
        }
    }
}
