using System.ComponentModel.DataAnnotations;

namespace SIGEBI.Web.Models.Publishers
{
    public class PublisherBaseModel
    {
        [Required(ErrorMessage = "El nombre de la editorial es obligatorio.")]
        public required string publisherName { get; set; }

        [Required(ErrorMessage = "La dirección es obligatoria.")]
        public required string address { get; set; }

        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        [StringLength(20, ErrorMessage = "El teléfono no puede exceder los 20 caracteres.")]
        public required string phoneNumber { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo electrónico no es válido.")]
        public required string email { get; set; }

        [Required(ErrorMessage = "El sitio web es obligatorio.")]
        public required string website { get; set; }

        public required string notes { get; set; }


    }
}
