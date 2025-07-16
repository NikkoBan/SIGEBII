
using SIGEBI.Application.Dtos.BooksDtos;
using SIGEBI.Domain.Entities.Configuration;


namespace SIGEBI.Application.mappers
{
    public static class BookMapper
    {
        
        public static BookDTO ToDTO(this Books entity)
        {
            if (entity == null) return null!;

            return new BookDTO
            {
                BookId = entity.Id,
                Title = entity.Title,
                ISBN = entity.ISBN,
                PublicationDate = entity.PublicationDate,
                CategoryId = entity.CategoryId,
                PublisherId = entity.PublisherId,
                Language = entity.Language,
                Summary = entity.Summary,
                TotalCopies = entity.TotalCopies,
                AvailableCopies = entity.AvailableCopies,
                GeneralStatus = entity.GeneralStatus,
                

               
              
            };
        }

      
        public static Books ToEntity(this CreateBookDTO dto)
        {
            if (dto == null) return null!;

            return new Books
            {
                Title = dto.Title,
                ISBN = dto.ISBN,
                PublicationDate = dto.PublicationDate,
                CategoryId = dto.CategoryId,
                PublisherId = dto.PublisherId,
                Language = dto.Language,
                Summary = dto.Summary,
                TotalCopies = dto.TotalCopies,
                AvailableCopies = dto.TotalCopies, 
                GeneralStatus = "Active", 
                IsDeleted= false,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "System",
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = "System",

            };
        }

     
        public static void UpdateEntity(this Books entity, UpdateBookDto dto)
        {
            if (entity == null || dto == null) return;

            entity.Title = dto.Title;
            entity.ISBN = dto.ISBN;
            entity.PublicationDate = dto.PublicationDate;
            entity.CategoryId = dto.CategoryId;
            entity.PublisherId = dto.PublisherId;
            entity.Language = dto.Language;
            entity.Summary = dto.Summary;
            entity.TotalCopies = dto.TotalCopies;
            entity.AvailableCopies = dto.AvailableCopies;
            entity.GeneralStatus = dto.GeneralStatus;
            
        }
    }
}
