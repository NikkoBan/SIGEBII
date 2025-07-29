using System.ComponentModel.DataAnnotations;
using System;

namespace SIGEBI.Application.DTOsAplication.UserDTOs
{
    public class UserCreationDto
    {
        public string FullName { get; set; } = string.Empty;
        public string InstitutionalEmail { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string InstitutionalIdentifier { get; set; } = string.Empty;
        public int RoleId { get; set; }
    }
}