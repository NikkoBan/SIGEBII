using System.Collections.Generic;

namespace SIGEBI.Persistence.Base
{
    public class OperationResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public object? Data { get; set; }
        public int? StatusCode { get; set; }

        public OperationResult()
        {
            Success = true;
            Message = "Operación completada exitosamente.";
        }

        public OperationResult(string errorMessage)
        {
            Success = false;
            Message = errorMessage;
            Errors.Add(errorMessage);
        }

        public static OperationResult Ok(string? message = "Operación completada exitosamente.", object? data = null, int? statusCode = 200)
        {
            return new OperationResult
            {
                Success = true,
                Message = message,
                Data = data,
                StatusCode = statusCode
            };
        }

        public static OperationResult Fail(string errorMessage, object? data = null, int? statusCode = 400)
        {
            return new OperationResult
            {
                Success = false,
                Message = errorMessage,
                Errors = new List<string> { errorMessage },
                Data = data,
                StatusCode = statusCode
            };
        }

        public static OperationResult Fail(List<string> errors, string? message = "La operación falló debido a errores.", object? data = null, int? statusCode = 400)
        {
            return new OperationResult
            {
                Success = false,
                Message = message,
                Errors = errors,
                Data = data,
                StatusCode = statusCode
            };
        }
    }

    public class OperationResult<T> : OperationResult
    {
        public new T? Data { get; set; }

        public OperationResult() : base() { }
        public OperationResult(string errorMessage) : base(errorMessage) { }

        public static OperationResult<T> Ok(T? data, string? message = "Operación completada exitosamente.", int? statusCode = 200)
        {
            return new OperationResult<T>
            {
                Success = true,
                Message = message,
                Data = data,
                StatusCode = statusCode
            };
        }

        public static OperationResult<T> Fail(string errorMessage, T? data = default, int? statusCode = 400)
        {
            return new OperationResult<T>
            {
                Success = false,
                Message = errorMessage,
                Errors = new List<string> { errorMessage },
                Data = data,
                StatusCode = statusCode
            };
        }

        public static OperationResult<T> Fail(List<string> errors, string? message = "La operación falló debido a errores.", T? data = default, int? statusCode = 400)
        {
            return new OperationResult<T>
            {
                Success = false,
                Message = message,
                Errors = errors,
                Data = data,
                StatusCode = statusCode
            };
        }
    }
}