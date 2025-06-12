using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Domain.Entities
{
    public class SystemSetting
    {
        public int SettingId { get; set; }
        public string SettingName { get; set; } = string.Empty;
        public string SettingValue { get; set; } = string.Empty;
        public string ValueDataType { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
