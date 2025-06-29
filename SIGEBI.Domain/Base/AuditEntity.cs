using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thrift.Protocols.Entities;

namespace SIGEBI.Domain.Base
{
    public abstract class AuditEntity<Ttype> : BaseEntity<Ttype>
    {
        public DateTime? CreatedAt { get; set; } 
        public DateTime? UpdatedAt { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string? UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public string? DeletedBy { get; set; }
        public string? Notes { get; set; }

        protected AuditEntity()
        {
            var now = DateTime.UtcNow;
            CreatedAt = now;
            UpdatedAt = now;
            CreatedBy = "system";
            IsDeleted = false;
        }

    }
}
