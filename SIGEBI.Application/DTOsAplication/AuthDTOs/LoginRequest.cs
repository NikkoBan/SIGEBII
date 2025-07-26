// SIGEBI.Application/DTOsAplication/AuthDTOs/LoginRequest.cs
using System.ComponentModel.DataAnnotations;

namespace SIGEBI.Application.DTOsAplication.AuthDTOs
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "El correo es requerido.")]
        [EmailAddress(ErrorMessage = "Formato de correo inválido.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "La contraseña es requerida.")]
        public string Password { get; set; } = null!;
    }
}