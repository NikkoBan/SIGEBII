using System.ComponentModel.DataAnnotations;
using System;

namespace SIGEBI.Application.DTOsAplication.UserHistoryDTOs
{
    public class UserHistoryCreationDto
    {
        [Required(ErrorMessage = "El ID de usuario es requerido para el historial.")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID de usuario debe ser un número positivo.")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "El correo ingresado es requerido.")]
        [EmailAddress(ErrorMessage = "Formato de correo electrónico inválido.")]
        public string EnteredEmail { get; set; } = null!;

        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }

        [Required(ErrorMessage = "El resultado de éxito es requerido.")]
        public bool IsSuccessful { get; set; }

        [StringLength(100, ErrorMessage = "La razón del fallo no puede exceder los 100 caracteres.")]
        public string? FailureReason { get; set; }

        [StringLength(50, ErrorMessage = "El rol obtenido no puede exceder los 50 caracteres.")]
        public string? ObtainedRole { get; set; }
    }
}