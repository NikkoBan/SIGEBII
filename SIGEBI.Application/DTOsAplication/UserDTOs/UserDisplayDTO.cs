using System;

namespace SIGEBI.Application.DTOsAplication.UserDTOs
{
    public class UserDisplayDto
    {
        public int UserId { get; set; }
        public string FullName { get; set; } = null!;
        public string InstitutionalEmail { get; set; } = null!;
        public string? InstitutionalIdentifier { get; set; }
        public int RoleId { get; set; }
        public bool IsActive { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}