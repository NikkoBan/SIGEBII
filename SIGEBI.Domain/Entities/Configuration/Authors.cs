
using SIGEBI.Domain.Base;

namespace SIGEBI.Domain.Entities.Configuration
{
    public class Authors : AuditableEntity 
    {

        public int AuthorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public DateOnly BirthDate { get; set; }

        public string Nationality { get; set; }

    }
}
