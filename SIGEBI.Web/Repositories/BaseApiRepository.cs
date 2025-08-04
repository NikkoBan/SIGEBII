using SIGEBI.Web.Models.Common;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SIGEBI.Web.Repositories
{
    public abstract class BaseApiRepository
    {
        protected readonly ILogger _logger;

        protected BaseApiRepository(ILogger logger)
        {
            _logger = logger;
        }

        protected async Task<ApiResponse<T>> SafeApiCall<T>(Func<Task<T>> func, string operation)
        {
            var traceId = Guid.NewGuid().ToString();
            try
            {
                var result = await func();
                return new ApiResponse<T>
                {
                    Success = true,
                    Data = result,
                    TraceId = traceId
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en {Operation}. TraceId: {TraceId}", operation, traceId);
                return new ApiResponse<T>
                {
                    Success = false,
                    TraceId = traceId,
                    Message = $"Error inesperado en '{operation}'.",
                    Error = new ApiError
                    {
                        ExceptionType = ex.GetType().Name,
                        ExceptionMessage = ex.Message,
                        StackTrace = ex.StackTrace
                    }
                };
            }
        }
    }
}