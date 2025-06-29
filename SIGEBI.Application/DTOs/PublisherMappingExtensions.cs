using SIGEBI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Application.DTOs
{
    //Para mapear entre DTOs y entidades

    public static class PublisherMappingExtensions
    {
        public static PublisherDto ToDto(this Publishers entity)
        {
            return new PublisherDto
            {
                ID = entity.ID,
                PublisherName = entity.PublisherName,
                Address = entity.Address,
                PhoneNumber = entity.PhoneNumber,
                Email = entity.Email,
                Website = entity.Website
            };
        }

        public static Publishers ToEntity(this PublisherCreateUpdateDto dto)
        {
            return new Publishers
            {
                PublisherName = dto.PublisherName,
                Address = dto.Address,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email,
                Website = dto.Website
            };
        }

        public static void MapToEntity(this PublisherCreateUpdateDto dto, Publishers entity)
        {
            entity.PublisherName = dto.PublisherName;
            entity.Address = dto.Address;
            entity.PhoneNumber = dto.PhoneNumber;
            entity.Email = dto.Email;
            entity.Website = dto.Website;
        }




    }
}
