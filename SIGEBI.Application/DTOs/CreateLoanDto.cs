using System.ComponentModel.DataAnnotations;

namespace SIGEBI.Application.DTOs
{
    public class CreateLoanDto
    {
        [Required(ErrorMessage = "El ID del usuario es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID del usuario debe ser positivo.")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "El ID del libro es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID del libro debe ser positivo.")]
        public int BookId { get; set; }

        [Required(ErrorMessage = "La fecha del préstamo es obligatoria.")]
        public DateTime LoanDate { get; set; }

        public DateTime? ReturnDate { get; set; }

        [Required(ErrorMessage = "El estado del préstamo es obligatorio.")]
        [StringLength(100, ErrorMessage = "El estado no puede exceder los 100 caracteres.")]
        public string Status { get; set; } = null!;
    }
}
