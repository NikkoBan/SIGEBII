using SIGEBI.Application.Dtos.BaseDTO;

namespace SIGEBI.Application.Dtos.BooksDtos
{
    public class BookDTO 
    {
        public int BookId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public DateTime? PublicationDate { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public int PublisherId { get; set; }
        public string PublisherName { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public int TotalCopies { get; set; }
        public int AvailableCopies { get; set; }
        public string GeneralStatus { get; set; } = string.Empty;
        public bool IsAvailable { get; set; }

    }
}
