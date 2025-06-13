using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIGEBI.Domain.Base;

namespace SIGEBI.Domain.Entities
{
    public class SystemSetting : BaseEntity<int>
    {
        [Column("SettingID")]
        [Key]
        public override int ID { get; set; }
        public string SettingName { get; set; } = string.Empty;
        public string SettingValue { get; set; } = string.Empty;
        public string ValueDataType { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
