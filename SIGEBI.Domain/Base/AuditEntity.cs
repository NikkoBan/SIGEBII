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
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public string? DeletedBy { get; set; }
        public string? Notes { get; set; }

    }
}
