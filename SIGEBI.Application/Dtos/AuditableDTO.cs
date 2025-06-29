

namespace SIGEBI.Application.Dtos
{
    public abstract class AuditableDTO
    {
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }

    
    }

}
