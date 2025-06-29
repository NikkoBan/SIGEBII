namespace SIGEBI.Domain.Base
{
    public class OperationResult
    {
        public string Message { get; set; } = string.Empty;
        public bool Success { get; set; }
        public dynamic? Data { get; set; } = null;

        public static OperationResult SuccessResult(dynamic? data = null)
        {
            return new OperationResult
            {
                Success = true,
                Message = "Operación exitosa.",
                Data = data
            };
        }

        public static OperationResult FailResult(string message, dynamic? data = null)
        {
            return new OperationResult
            {
                Success = false,
                Message = message,
                Data = data
            };
            /*/ Respuesta exitosa con datos
            var ok = OperationResult.SuccessResult(someData);

            Respuesta de error con mensaje
            var error = OperationResult.FailResult("Error al guardar el registro.");
            /*/
        }
    }
}
