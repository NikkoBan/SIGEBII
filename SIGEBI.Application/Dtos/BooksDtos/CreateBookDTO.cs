using SIGEBI.Application.Dtos.BaseDTO;
using System.ComponentModel.DataAnnotations;

namespace SIGEBI.Application.Dtos.BooksDtos
{
    public class CreateBookDTO 
    {

        public string Title { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public DateTime? PublicationDate { get; set; }
        public int CategoryId { get; set; }
        public int PublisherId { get; set; }
        public string Language { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public int TotalCopies { get; set; }
        public List<int> AuthorIds { get; set; } = new();
    }
}

