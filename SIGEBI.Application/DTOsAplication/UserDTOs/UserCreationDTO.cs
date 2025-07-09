using System.ComponentModel.DataAnnotations;
using System;

namespace SIGEBI.Application.DTOsAplication.UserDTOs
{
    public class UserCreationDto
    {
        [Required(ErrorMessage = "El nombre completo es requerido.")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "El nombre completo debe tener entre 3 y 200 caracteres.")]
        public string FullName { get; set; } = null!;

        [Required(ErrorMessage = "El correo institucional es requerido.")]
        [EmailAddress(ErrorMessage = "Formato de correo electrónico inválido.")]
        [StringLength(100, ErrorMessage = "El correo institucional no puede exceder los 100 caracteres.")]
        public string InstitutionalEmail { get; set; } = null!;

        [Required(ErrorMessage = "La contraseña es requerida.")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 50 caracteres.")]
        public string Password { get; set; } = null!;

        [StringLength(50, ErrorMessage = "El identificador institucional no puede exceder los 50 caracteres.")]
        public string? InstitutionalIdentifier { get; set; }

        [Required(ErrorMessage = "El rol es requerido.")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID del rol debe ser un número positivo.")]
        public int RoleId { get; set; }
    }
}