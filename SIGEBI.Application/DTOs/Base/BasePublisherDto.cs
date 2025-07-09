using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Application.DTOs.Base
{
    public abstract class BasePublisherDto
    {
        [Required]
        [StringLength(255)]
        public string PublisherName { get; set; } = string.Empty;

        [StringLength(255)]
        public string? Address { get; set; }

        [StringLength(20)]
        public string? PhoneNumber { get; set; }

        [StringLength(255)]
        [EmailAddress]
        public string? Email { get; set; }

        [StringLength(255)]
        public string? Website { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }
    }
}
