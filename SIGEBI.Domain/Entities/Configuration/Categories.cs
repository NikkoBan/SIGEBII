

using SIGEBI.Domain.Base;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SIGEBI.Domain.Entities.Configuration
{
    public class Categories : BaseEntity<int>
    {

       
        public override  int Id { get; set; }
        public string CategoryName { get; set; }= string.Empty;
        public string? Description { get; set; }


    }
}
