using SIGEBI.Domain.Base;

namespace SIGEBI.Domain.Entities
{
    public class Notifications : AuditEntity<int>
    {
        public override int Id { get; set; }
        public int UserId { get; set; }
        public string? NotificationType { get; set; } 
        public required string Message { get; set; }
        public DateTime SentDate { get; set; }
        public bool IsRead { get; set; } 
        public DateTime ReadDate { get; set; }

    }
}
