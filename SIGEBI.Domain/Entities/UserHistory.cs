namespace SIGEBI.Domain.Entities
{
    public class UserHistory
    {
        public int LogId { get; set; }
        public int UserId { get; set; }
        public string EnteredEmail { get; set; } = null!;
        public DateTime AttemptDate { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public bool IsSuccessful { get; set; }
        public string? FailureReason { get; set; }
        public string? ObtainedRole { get; set; }
    }
}
