using SIGEBI.Application.Dtos.BaseDTO;

namespace SIGEBI.Application.Dtos.AuthorDTO
{
    public class AuthorDTO
    {
        public int AuthorId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime? BirthDate { get; set; }
        public string? Nationality { get; set; }
    }
}
