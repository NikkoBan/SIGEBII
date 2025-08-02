namespace SIGEBI.Application.DTOs
{
    public class AuthorDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Nationality { get; set; }
        public DateTime? BirthDate { get; set; }
    }
}
