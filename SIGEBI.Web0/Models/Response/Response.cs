using System.Text.Json.Serialization;

namespace SIGEBI.Web0.Models
{
    public class ApiResponse<T>
    {
        public bool success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
    }

}