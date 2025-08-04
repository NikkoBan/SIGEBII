using System.ComponentModel.DataAnnotations;

namespace SIGEBI.Application.DTOsAplication.LoanDetailsDTOs
{
    public class LoanDetailsCreationDTO
    {
        [Required(ErrorMessage = "El identificador del préstamo es obligatorio.")]
        public int LoanId { get; set; }

        [Required(ErrorMessage = "El identificador del libro es obligatorio.")]
        public int BookId { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor que cero.")]
        public int Quantity { get; set; }
    }
}
