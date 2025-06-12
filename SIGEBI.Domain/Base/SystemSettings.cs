using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Domain.Base
{
    public class SystemSettings
    {
        public int SettingID { get; set; }
        public string SettingName { get; set; } = string.Empty;
        public string? SettingValue { get; set; }

        public string? Description { get; set; }
    }
}
