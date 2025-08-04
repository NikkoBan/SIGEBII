using System.ComponentModel.DataAnnotations;

namespace SIGEBI.Application.DTOsAplication.LoanDTOs
{
    public class LoanUpdateDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public DateTime LoanDate { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int StatusId { get; set; }

        public List<int>? BookIds { get; set; }
    }
}
