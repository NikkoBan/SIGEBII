using SIGEBI.Application.Dtos.BaseDTO;

namespace SIGEBI.Application.Dtos.CategoryDto
{
    public class CategoryDTO : AuditableDTO
    {
     public int CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string? Description { get; set; }
    }
}
