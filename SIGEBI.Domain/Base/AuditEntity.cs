
namespace SIGEBI.Domain.Base
{
    public abstract class AuditEntity<TKey> : BaseEntity<TKey>
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; }
        public required string CreatedBy { get; set; }
        public required string UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public required string DeletedBy { get; set; }
        public string? Notes { get; set; }

    }
}
