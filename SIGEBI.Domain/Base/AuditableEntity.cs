

using System.ComponentModel.DataAnnotations;

namespace SIGEBI.Domain.Base
{
    public abstract class AuditableEntity 
    {
        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        [MaxLength(100)]
        public string CreatedBy { get; set; } = string.Empty;

        public DateTime? UpdatedAt { get; set; }

        [MaxLength(100)]
        public string? UpdatedBy { get; set; } = string.Empty;

        public DateTime? DeletedAt { get; set; }

        [MaxLength(100)]
        public string? DeletedBy { get; set; }

        [Required]
        public bool IsDeleted { get; set; } = false;
    }


}

