namespace SIGEBI.Application.Dtos.BaseDTO
{
    public abstract class AuditableDTO
    {
       
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public  string CreatedBy { get; set; } = string.IsNullOrWhiteSpace("System") ? "System" : string.Empty; // Default to "System" if null or whitespace 


    }

}
