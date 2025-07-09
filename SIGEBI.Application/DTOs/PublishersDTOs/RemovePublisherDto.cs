using SIGEBI.Application.DTOs.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Application.DTOs.PublishersDTOs
{
    public class RemovePublisherDto : BasePublisherDto
    {
        [Required]
        public int Id { get; set; }

        [StringLength(100)]
        public string? DeletedBy { get; set; }

        [StringLength(500)]
        public string? Reason { get; set; }
    }
}
