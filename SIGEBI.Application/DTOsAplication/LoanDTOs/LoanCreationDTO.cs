using System.ComponentModel.DataAnnotations;

namespace SIGEBI.Application.DTOsAplication.LoanDTOs
{
    public class LoanCreationDTO
    {
        [Required]
        public DateTime LoanDate { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int StatusId { get; set; }

        // Si se agregan detalles desde la creación
        public List<int>? BookId { get; set; }
    }
}
