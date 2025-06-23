
namespace SIGEBI.Domain.Base
{
    public class OperationResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public dynamic? Data { get; set; } 
        public OperationResult() { }

        protected OperationResult(bool isSuccess, string message, dynamic? data = null)
        {
            IsSuccess = isSuccess;
            Message = message;
        }
        public static OperationResult Success(string message, dynamic? data = null)
        {
            return new OperationResult(true, message);
        }
        public static OperationResult Failure(string message)
        {
            return new OperationResult(false, message);
        }

    }
}
