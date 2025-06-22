using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Domain.Base
{
    public class ﻿OperationResult
    {
        public OperationResult()
        {
            this.Success = true;
        }

        public OperationResult(string message, bool success = true, dynamic data = null)
        {
            this.Message = message;
            this.Success = success;
            this.Data = data;
        }

        public string Message { get; set; }
        public bool Success { get; set; }
        public dynamic Data { get; set; }

        public bool Failed => !Success;

        public OperationResult With(string message, dynamic data = null)
        {
            this.Message = message;
            this.Data = data;
            return this;
        }

        public static OperationResult Ok(string message = "Resultado exitoso", dynamic data = null)
            => new OperationResult(message, true, data);

        public static OperationResult Fail(string message = "Se ha producido un error", dynamic data = null)
            => new OperationResult(message, false, data);
    }
}
