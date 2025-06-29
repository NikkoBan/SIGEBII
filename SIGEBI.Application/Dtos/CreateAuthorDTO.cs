

namespace SIGEBI.Application.Dtos
{
    public class CreateAuthorDTO : AuditableDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Nationality { get; set; }
    }
}
