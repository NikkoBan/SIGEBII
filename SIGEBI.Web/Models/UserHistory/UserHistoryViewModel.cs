// SIGEBI.Web/Models/UserHistory/UserHistoryViewModel.cs

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace SIGEBI.Web.Models.UserHistory
{
    public class UserHistoryViewModel
    {
        [JsonPropertyName("logId")]
        [Display(Name = "ID de Log")]
        public int LogId { get; set; }

        [JsonPropertyName("userId")]
        [Display(Name = "ID de Usuario")]
        public int UserId { get; set; }

        [JsonPropertyName("enteredEmail")]
        [Display(Name = "Correo Ingresado")]
        public string EnteredEmail { get; set; } = string.Empty;

        [JsonPropertyName("attemptDate")]
        [Display(Name = "Fecha de Intento")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime AttemptDate { get; set; }

        [JsonPropertyName("ipAddress")]
        [Display(Name = "Dirección IP")]
        public string IpAddress { get; set; } = string.Empty;

        [JsonPropertyName("userAgent")]
        [Display(Name = "Agente de Usuario")]
        public string UserAgent { get; set; } = string.Empty;

        [JsonPropertyName("isSuccessful")]
        [Display(Name = "Exitoso")]
        public bool IsSuccessful { get; set; }

        [JsonPropertyName("failureReason")]
        [Display(Name = "Razón de Fallo")]
        public string? FailureReason { get; set; }

        [JsonPropertyName("obtainedRole")]
        [Display(Name = "Rol Obtenido")]
        public string? ObtainedRole { get; set; }
    }

    public class UserHistoryApiResponse
    {
        [JsonPropertyName("isSuccess")]
        public bool IsSuccess { get; set; }

        [JsonPropertyName("message")]
        public string? Message { get; set; }

        [JsonPropertyName("data")]
        public JsonElement? Data { get; set; }

        [JsonIgnore]
        public List<UserHistoryViewModel>? DataAsList
        {
            get
            {
                if (Data?.ValueKind == JsonValueKind.Array)
                {
                    return JsonSerializer.Deserialize<List<UserHistoryViewModel>>(Data.ToString()!, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                return null;
            }
        }

        [JsonIgnore]
        public UserHistoryViewModel? DataAsSingleObject
        {
            get
            {
                if (Data?.ValueKind == JsonValueKind.Object)
                {
                    return JsonSerializer.Deserialize<UserHistoryViewModel>(Data.ToString()!, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                return null;
            }
        }
    }
}