using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Application.DTOs
{
    public class PublisherCreateUpdateDto
    {
        [Required(ErrorMessage = "El nombre de la editorial es obligatorio.")]
        [StringLength(255, ErrorMessage = "El nombre no puede superar los 255 caracteres.")]
        public string PublisherName { get; set; } = string.Empty;

        [StringLength(255, ErrorMessage = "La dirección no puede superar los 255 caracteres.")]
        public string? Address { get; set; }

        [StringLength(50, ErrorMessage = "El teléfono no puede superar los 50 caracteres.")]
        public string? PhoneNumber { get; set; }

        [EmailAddress(ErrorMessage = "El correo electrónico no es válido.")]
        [StringLength(100, ErrorMessage = "El correo electrónico no puede superar los 100 caracteres.")]
        public string? Email { get; set; }

        [StringLength(255, ErrorMessage = "El sitio web no puede superar los 255 caracteres.")]
        public string? Website { get; set; }
    }
}
