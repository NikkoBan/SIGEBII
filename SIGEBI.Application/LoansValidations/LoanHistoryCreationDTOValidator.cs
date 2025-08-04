using SIGEBI.Application.DTOsAplication.LoanHistoryDTOs;

namespace SIGEBI.Application.Validations
{
    public static class LoanHistoryCreationDTOValidator
    {
        public static List<string> Validate(LoanHistoryCreationDTO dto)
        {
            var errors = new List<string>();

            if (dto.LoanId <= 0)
                errors.Add("El campo 'LoanId' debe ser mayor que cero.");

            if (dto.StatusId <= 0)
                errors.Add("El campo 'StatusId' debe ser mayor que cero.");

            if (dto.ChangedAt == default)
                errors.Add("El campo 'ChangedAt' es obligatorio y debe contener una fecha válida.");

            if (string.IsNullOrWhiteSpace(dto.ChangedBy))
                errors.Add("El campo 'ChangedBy' es obligatorio.");

            if (!string.IsNullOrWhiteSpace(dto.Notes) && dto.Notes.Length > 500)
                errors.Add("El campo 'Notes' no puede exceder los 500 caracteres.");

            return errors;
        }
    }
}
