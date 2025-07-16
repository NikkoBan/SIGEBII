using SIGEBI.Application.Dtos.BaseDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Application.Dtos.CategoryDto
{
    public class CreateCategoryDTO 
    {
        public string CategoryName { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
