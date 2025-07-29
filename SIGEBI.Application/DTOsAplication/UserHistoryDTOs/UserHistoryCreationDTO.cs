// SIGEBI.Application/DTOsAplication/UserHistoryDTOs/UserHistoryCreationDto.cs

using System.ComponentModel.DataAnnotations;
using System;

namespace SIGEBI.Application.DTOsAplication.UserHistoryDTOs
{
    public class UserHistoryCreationDto
    {
        public int UserId { get; set; }
        public string EnteredEmail { get; set; } = string.Empty;
        public DateTime AttemptDate { get; set; }
        public string IpAddress { get; set; } = string.Empty;
        public string UserAgent { get; set; } = string.Empty;
        public bool IsSuccessful { get; set; }
        public string FailureReason { get; set; } = string.Empty;
        public string ObtainedRole { get; set; } = string.Empty;
    }
}