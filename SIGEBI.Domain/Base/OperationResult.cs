﻿
namespace SIGEBI.Domain.Base
{
    public class OperationResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }

        public static OperationResult SuccessResult(object? data = null, string? message = null)
        {
            return new OperationResult
            {
                Success = true,
                Data = data,
                Message = message
            };
        }

        public static OperationResult FailureResult(string message)
        {
            return new OperationResult
            {
                Success = false,
                Message = message
            };
        }


    }
}
