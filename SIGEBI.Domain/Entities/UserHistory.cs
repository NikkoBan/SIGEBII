using System;

namespace SIGEBI.Domain.Entities
{
    public class UserHistory
    {
        public int LogId { get; set; }
        public int UserId { get; set; }
        public string? EnteredEmail { get; set; } 
        public DateTime AttemptDate { get; set; }
        public string? IpAddress { get; set; } 
        public string? UserAgent { get; set; } 
        public bool IsSuccessful { get; set; }
        public string? FailureReason { get; set; } 
        public string? ObtainedRole { get; set; } 
        public DateTime CreatedAt { get; set; } 
        public string CreatedBy { get; set; } = null!;
        public DateTime? UpdatedAt { get; set; } 
        public string? UpdatedBy { get; set; } 
        public bool IsDeleted { get; set; } 
        public DateTime? DeletedAt { get; set; } 
        public string? DeletedBy { get; set; } 

        public User User { get; set; } = null!; 
    }
}