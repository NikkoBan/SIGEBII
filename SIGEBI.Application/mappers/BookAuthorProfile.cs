
using SIGEBI.Application.Dtos.BookAuthorDTO;
using SIGEBI.Domain.Entities.Configuration;

namespace SIGEBI.Application.mappers
{
    public static class BookAuthorMapper
    {
        public static BookAuthorDTO ToDTO(this BookAuthor entity)
        {
            if (entity == null) return null!;

            return new BookAuthorDTO
            {
                BookId = entity.BookId,
                AuthorId = entity.AuthorId
            };
        }

        public static BookAuthor ToEntity(this CreateBookAuthorDTO dto)
        {
            if (dto == null) return null!;

            return new BookAuthor
            {
                BookId = dto.BookId,
                AuthorId = dto.AuthorId,
               
            };
        }

        
        public static void UpdateEntity(this BookAuthor entity, UpdateBookAuthorDTO dto)
        {
            if (entity == null || dto == null) return;

            entity.BookId = dto.BookId;
            entity.AuthorId = dto.AuthorId;
        }
    }
}
