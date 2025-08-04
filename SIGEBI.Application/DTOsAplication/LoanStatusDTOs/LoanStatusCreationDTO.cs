using System.ComponentModel.DataAnnotations;

namespace SIGEBI.Application.DTOsAplication.LoanStatusDTOs
{
    public class LoanStatusCreationDTO
    {
        [Required(ErrorMessage = "El nombre del estado del préstamo es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede tener más de 100 caracteres.")]
        public string Name { get; set; } = string.Empty;
    }
}
