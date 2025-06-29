
namespace SIGEBI.Application.Dtos
{
    public abstract class ModifiableDTO : AuditableDTO
    {
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
    }
}
