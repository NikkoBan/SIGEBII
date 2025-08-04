using System;

namespace SIGEBI.Web.Models.Common
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public string? TraceId { get; set; }
        public ApiError? Error { get; set; }
    }

    public class ApiError
    {
        public string? ExceptionType { get; set; }
        public string? ExceptionMessage { get; set; }
        public string? StackTrace { get; set; }
    }
}