
using SIGEBI.Domain.Base;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SIGEBI.Domain.Entities.Configuration
{
    public class Role : BaseEntity<int>
    {
        [Column("RoleId")]
        [Key]
        public override int Id { get; set; }

        public string RoleName { get; set; }
        public string Description { get; set; }

       
        public ICollection<User> Users { get; set; }
    }
}
