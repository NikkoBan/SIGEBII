
using SIGEBI.Domain.Base;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SIGEBI.Domain.Entities.Configuration
{
    public class Publisher : BaseEntity<int>
    {
        [Column("PublisherId")]
        public override int Id { get; set; }

        public required string PublisherName { get; set; }
        public required string Address { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Email { get; set; }
        public required string Website { get; set; }

        public ICollection<Books> Books { get; set; } = new List<Books>();
    }
}
