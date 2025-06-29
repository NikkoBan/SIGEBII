

namespace SIGEBI.Application.Dtos
{
    public class CreatePublisherDTO : AuditableDTO
    {
        public string PublisherName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
    }
}
