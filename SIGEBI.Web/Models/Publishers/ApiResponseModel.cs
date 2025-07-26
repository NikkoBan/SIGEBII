namespace SIGEBI.Web.Models.Publishers
{
    public class ApiResponseModel<T>
    {
        public required string message { get; set; }
        public bool success { get; set; }
        public required T data { get; set; }
        public bool failed { get; set; }
    }
}
