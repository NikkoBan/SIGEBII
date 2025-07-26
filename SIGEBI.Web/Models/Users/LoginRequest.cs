// SIGEBI.Web/Models/Users/LoginRequest.cs

using System.ComponentModel.DataAnnotations;

namespace SIGEBI.Web.Models.Users
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "El correo institucional es requerido.")]
        [EmailAddress(ErrorMessage = "Formato de correo electrónico inválido.")]
        [Display(Name = "Correo Institucional")]
        public string InstitutionalEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es requerida.")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; } = string.Empty;
    }
}