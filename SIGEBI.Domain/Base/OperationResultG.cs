
namespace SIGEBI.Domain.Base
{
    public class OperationResultG
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;

        public static OperationResultG Success(string message = "Operación exitosa.")
            => new OperationResultG { IsSuccess = true, Message = message };

        public static OperationResultG Fail(string message)
            => new OperationResultG { IsSuccess = false, Message = message };
    }
}
