using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using Microsoft.VisualStudio.Services.ClientNotification;
using SIGEBI.Web.Models;


namespace SIGEBI.Web.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string Key = "Notifications";

        public NotificationService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private void AddNotification(string type, string message)
        {
            var tempDataFactory = _httpContextAccessor.HttpContext!.RequestServices.GetService<ITempDataDictionaryFactory>();
            var tempData = tempDataFactory!.GetTempData(_httpContextAccessor.HttpContext!);

            var notifications = tempData.ContainsKey(Key)
                ? JsonSerializer.Deserialize<List<SIGEBI.Web.Models.NotificationMessage>>(tempData[Key]!.ToString()!) ?? new List<SIGEBI.Web.Models.NotificationMessage>()
                : new List<SIGEBI.Web.Models.NotificationMessage>();

            notifications.Add(new SIGEBI.Web.Models.NotificationMessage { Type = type, Message = message });
            tempData[Key] = JsonSerializer.Serialize(notifications);
        }

        public void Success(string message) => AddNotification("success", message);
        public void Error(string message) => AddNotification("error", message);
        public void Info(string message) => AddNotification("info", message);
        public void Warning(string message) => AddNotification("warning", message);


    }
}
