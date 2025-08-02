using System.ComponentModel.DataAnnotations;

namespace SIGEBI.Application.DTOs
{
    public class UpdateBookDto
    {
        [Required(ErrorMessage = "El ID del libro es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID debe ser positivo.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El título es obligatorio.")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "El título debe tener entre 3 y 200 caracteres.")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "El ISBN es obligatorio.")]
        [StringLength(20, ErrorMessage = "El ISBN no puede tener más de 20 caracteres.")]
        public string ISBN { get; set; } = null!;

        public DateTime? PublicationDate { get; set; }

        [Required(ErrorMessage = "El ID de la categoría es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID de la categoría debe ser positivo.")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "El ID del editorial es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID del editorial debe ser positivo.")]
        public int PublisherId { get; set; }

        [Required(ErrorMessage = "El idioma es obligatorio.")]
        [StringLength(50, ErrorMessage = "El idioma no puede exceder los 50 caracteres.")]
        public string Language { get; set; } = null!;

        [StringLength(1000, ErrorMessage = "El resumen no puede exceder los 1000 caracteres.")]
        public string? Summary { get; set; }

        [Required(ErrorMessage = "El total de copias es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El total de copias debe ser mayor que 0.")]
        public int TotalCopies { get; set; }

        [Required(ErrorMessage = "Las copias disponibles son obligatorias.")]
        [Range(0, int.MaxValue, ErrorMessage = "Las copias disponibles no pueden ser negativas.")]
        public int AvailableCopies { get; set; }

        [Required(ErrorMessage = "El estado general es obligatorio.")]
        [StringLength(100, ErrorMessage = "El estado general no puede exceder los 100 caracteres.")]
        public string GeneralStatus { get; set; } = null!;
    }
}
