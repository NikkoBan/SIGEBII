
using SIGEBI.Application.Dtos.AuthorDTO;
using SIGEBI.Domain.Entities.Configuration;
using SIGEBI.Application.Dtos.BaseDTO;

namespace SIGEBI.Application.mappers
{
    public static class AuthorMapper
    {
        public static AuthorDTO ToDto(Authors entity)
        {
            return new AuthorDTO
            {
                AuthorId = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                BirthDate = entity.BirthDate,
                Nationality = entity.Nationality!,
               
            };
        }

        public static Authors ToEntity(CreateAuthorDTO dto)
        {
            return new Authors
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                BirthDate = dto.BirthDate,
                Nationality = dto.Nationality,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "System", 
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = "System",
                IsDeleted = false
            };
        }

        public static void UpdateEntity(Authors entity, UpdateAuthorDTO dto)
        {
            entity.FirstName = dto.FirstName;
            entity.LastName = dto.LastName;
            entity.BirthDate = dto.BirthDate;
            entity.Nationality = dto.Nationality;
            entity.UpdatedAt = DateTime.UtcNow;
        }
    }
}


