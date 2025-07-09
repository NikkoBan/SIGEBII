using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIGEBI.Domain.Entities;


namespace SIGEBI.Application.DTOs.PublishersDTOs
{
    public static class PublisherMappings
    {
        public static PublisherDto ToDto(this Publishers publisher)
        {
            return new PublisherDto
            {
                Id = publisher.ID,
                PublisherName = publisher.PublisherName,
                Address = publisher.Address,
                PhoneNumber = publisher.PhoneNumber,
                Email = publisher.Email,
                Website = publisher.Website,
                Notes = publisher.Notes,
                
            };
        }

        public static Publishers ToEntity(this CreationPublisherDto dto)
        {
            return new Publishers
            {
                PublisherName = dto.PublisherName,
                Address = dto.Address,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email,
                Website = dto.Website,
                Notes = dto.Notes
            };
        }

        public static void UpdateEntity(this UpdatePublisherDto dto, Publishers entity)
        {
            entity.PublisherName = dto.PublisherName;
            entity.Address = dto.Address;
            entity.PhoneNumber = dto.PhoneNumber;
            entity.Email = dto.Email;
            entity.Website = dto.Website;
            entity.Notes = dto.Notes;
        }

        public static void MarkAsDeleted(this Publishers entity, string? deletedBy, string? reason)
        {
            entity.IsDeleted = true;
            entity.UpdatedBy = deletedBy ?? "system";
            entity.Notes = reason ?? entity.Notes;
            entity.UpdatedAt = DateTime.UtcNow;
        }
    }
}
