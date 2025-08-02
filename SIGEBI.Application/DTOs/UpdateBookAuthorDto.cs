using System.ComponentModel.DataAnnotations;

namespace SIGEBI.Application.DTOs
{
    public class UpdateBookAuthorDto
    {
        [Required(ErrorMessage = "El ID del libro es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID del libro debe ser positivo.")]
        public int BookId { get; set; }

        [Required(ErrorMessage = "El ID del autor es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID del autor debe ser positivo.")]
        public int AuthorId { get; set; }
    }
}
