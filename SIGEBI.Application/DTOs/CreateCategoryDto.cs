using System.ComponentModel.DataAnnotations;

namespace SIGEBI.Application.DTOs
{
    public class CreateCategoryDto
    {
        [Required(ErrorMessage = "El nombre de la categoría es obligatorio.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres.")]
        public string Name { get; set; } = null!;

        [StringLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres.")]
        public string? Description { get; set; }
    }
}
