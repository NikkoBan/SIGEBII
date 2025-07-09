using System.ComponentModel.DataAnnotations;

namespace SIGEBI.Application.DTOs
{
    public class CreateAuthorDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres.")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El apellido debe tener entre 2 y 100 caracteres.")]
        public string LastName { get; set; } = null!;

        [StringLength(100, ErrorMessage = "La nacionalidad no puede exceder los 100 caracteres.")]
        public string? Nationality { get; set; }

        public DateTime? BirthDate { get; set; }
    }
}
