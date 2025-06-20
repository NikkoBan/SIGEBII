using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIGEBI.Domain.Base;

namespace SIGEBI.Domain.Entities
{
    public class Categories : AuditEntity<int>
    {
       
        public override int ID { get; set; }
        public required string CategoryName { get; set; }
        public string? Description { get; set; }
    }
}
