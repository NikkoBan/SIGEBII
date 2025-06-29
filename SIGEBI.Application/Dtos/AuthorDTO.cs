

namespace SIGEBI.Application.Dtos
{
    public class AuthorDTO : ModifiableDTO
    {
        public int AuthorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Nationality { get; set; }
    }
}
