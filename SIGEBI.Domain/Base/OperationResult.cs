using System;

namespace SIGEBI.Domain.Base
{
    public class OperationResult<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }

        public static OperationResult<T> SuccessResult(T data, string message = "Operación exitosa.")
        {
            return new OperationResult<T>
            {
                Success = true,
                Message = message,
                Data = data
            };
        }

        public static OperationResult<T> FailResult(string message, T? data = default)
        {
            return new OperationResult<T>
            {
                Success = false,
                Message = message,
                Data = data
            };
        }
    }
}
