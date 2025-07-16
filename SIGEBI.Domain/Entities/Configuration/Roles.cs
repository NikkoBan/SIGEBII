
using SIGEBI.Domain.Base;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SIGEBI.Domain.Entities.Configuration
{
    public class Role : BaseEntity<int>
    {
       
        public override int Id { get; set; }

        public required string RoleName { get; set; }
        public required string Description { get; set; }


        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
