
namespace SIGEBI.Application.Dtos
{
    public class CreateBookAuthorDTO : AuditableDTO
    {
        public int BookId { get; set; }
        public int AuthorId { get; set; }
    }

}
