
namespace SIGEBI.Application.Dtos.PublisherDto
{
    public class UpdatePublisherDto
    {
        public int PublisherId { get; set; }
        public string PublisherName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Website { get; set; } = string.Empty;
    }
}
