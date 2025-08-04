using System.ComponentModel.DataAnnotations;

namespace SIGEBI.Application.DTOsAplication.LoanStatusDTOs
{
    public class LoanStatusUpdateDTO
    {
        [Required(ErrorMessage = "El identificador del estado es obligatorio.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del estado del préstamo es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede tener más de 100 caracteres.")]
        public string Name { get; set; } = string.Empty;
    }
}
