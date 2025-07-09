using SIGEBI.Application.DTOs.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Application.DTOs.PublishersDTOs
{
    public class UpdatePublisherDto : BasePublisherDto
    {
        [Required]
        public int Id { get; set; }

    }
}
