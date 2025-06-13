using SIGEBI.Domain.Base;

namespace SIGEBI.Domain.Entities
{
    public class SystemSettings : AuditEntity<int>
    {
        public override int Id { get; set; }
        public required string SettingName { get; set; }
        public required string SettingValue { get; set; }
        public required string ValueDataType { get; set; } 
        public string? Description { get; set; }
    }
}
