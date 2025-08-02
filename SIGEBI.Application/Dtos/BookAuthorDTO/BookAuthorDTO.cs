using SIGEBI.Application.Dtos.BaseDTO;

namespace SIGEBI.Application.Dtos.BookAuthorDTO
{
    public class BookAuthorDTO 
    {
        public int BookId { get; set; }
        public int AuthorId { get; set; }
        public string? BookTitle { get; set; }
        public string? AuthorName { get; set; }
    }
}
