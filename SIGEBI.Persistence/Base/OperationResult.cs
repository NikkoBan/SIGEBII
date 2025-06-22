using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Persistence.Base
{
    public  class OperationResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public object? Data { get; set; }
        public int? StatusCode { get; set; }
    }
}
