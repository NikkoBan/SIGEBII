// SIGEBI.Web/Models/Users/UserViewModel.cs

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Text.Json; // Para JsonElement

namespace SIGEBI.Web.Models.Users
{
    public class UserViewModel
    {
        [JsonPropertyName("userId")]
        [Display(Name = "ID de Usuario")]
        public int UserId { get; set; }

        [JsonPropertyName("fullName")]
        [Display(Name = "Nombre Completo")]
        public string FullName { get; set; } = string.Empty;

        [JsonPropertyName("institutionalEmail")]
        [Display(Name = "Correo Institucional")]
        public string InstitutionalEmail { get; set; } = string.Empty;

        [JsonPropertyName("institutionalIdentifier")]
        [Display(Name = "Identificador Institucional")]
        public string? InstitutionalIdentifier { get; set; }

        [JsonPropertyName("roleId")]
        [Display(Name = "ID de Rol")]
        public int RoleId { get; set; }

        [JsonPropertyName("isActive")]
        [Display(Name = "Activo")]
        public bool IsActive { get; set; }

        [JsonPropertyName("registrationDate")]
        [Display(Name = "Fecha de Registro")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime RegistrationDate { get; set; }
    }

    public class UserApiResponse
    {
        [JsonPropertyName("isSuccess")]
        public bool IsSuccess { get; set; }

        [JsonPropertyName("message")]
        public string? Message { get; set; }

        [JsonPropertyName("data")]
        public JsonElement? Data { get; set; }

        [JsonIgnore]
        public List<UserViewModel>? DataAsList
        {
            get
            {
                if (Data?.ValueKind == JsonValueKind.Array)
                {
                    return JsonSerializer.Deserialize<List<UserViewModel>>(Data.ToString()!, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                return null;
            }
        }

        [JsonIgnore]
        public UserViewModel? DataAsSingleObject
        {
            get
            {
                if (Data?.ValueKind == JsonValueKind.Object)
                {
                    return JsonSerializer.Deserialize<UserViewModel>(Data.ToString()!, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                return null;
            }
        }
    }
}