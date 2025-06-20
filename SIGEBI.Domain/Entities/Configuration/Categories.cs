

using SIGEBI.Domain.Base;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SIGEBI.Domain.Entities.Configuration
{
    public class Categories : Base.BaseEntity<int>
    {
        [Column("CategoryId")]
        [Key]

        public override int Id { get; set; }
        public string CategoryName { get; set; }
        public string? Description { get; set; }


    }
}
