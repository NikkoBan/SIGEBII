using System.ComponentModel.DataAnnotations;
using System;

namespace SIGEBI.Application.DTOsAplication.UserDTOs
{
    public class UserUpdateDto
    {
        public int UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string InstitutionalEmail { get; set; } = string.Empty;
        public string InstitutionalIdentifier { get; set; } = string.Empty;
        public int RoleId { get; set; }
        public bool IsActive { get; set; }
        public string UpdatedBy { get; set; } = string.Empty;
    }
}