
using SIGEBI.Domain.Base;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SIGEBI.Domain.Entities.Configuration
{
    public class Publisher : BaseEntity<int>
    {
        [Column("PublisherId")]
        [Key]
        public override int Id { get; set; }

        public string PublisherName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }

        public ICollection<Books> Books { get; set; }
    }
}
