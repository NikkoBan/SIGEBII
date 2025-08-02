using System.ComponentModel.DataAnnotations;

namespace SIGEBI.Application.DTOs
{
    public class CreateLoanHistoryDto
    {
        [Required(ErrorMessage = "El ID del préstamo es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID del préstamo debe ser positivo.")]
        public int LoanId { get; set; }

        [Required(ErrorMessage = "La fecha de acción es obligatoria.")]
        public DateTime ActionDate { get; set; }

        [Required(ErrorMessage = "El tipo de acción es obligatorio.")]
        [StringLength(100, ErrorMessage = "El tipo de acción no puede exceder los 100 caracteres.")]
        public string ActionType { get; set; } = null!;

        [StringLength(1000, ErrorMessage = "Las notas no pueden exceder los 1000 caracteres.")]
        public string? Notes { get; set; }
    }
}
