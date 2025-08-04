namespace SIGEBI.Web.Models.Users
{ // nuevos dtos para la gestion de usuarios en la aplicacion SIGEBI
    public class UserRequest
    {
        public string FullName { get; set; } = string.Empty;
        public string InstitutionalEmail { get; set; } = string.Empty;
        public string InstitutionalIdentifier { get; set; } = string.Empty;
        public int RoleId { get; set; }
        public string Password { get; set; } = string.Empty;
    }

    public class UserUpdateRequest
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