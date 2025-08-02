namespace SIGEBI.Application.DTOs
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public DateTime? PublicationDate { get; set; }
        public int CategoryId { get; set; }
        public int PublisherId { get; set; }
        public string Language { get; set; } = string.Empty;
        public string? Summary { get; set; }
        public int TotalCopies { get; set; }
        public int AvailableCopies { get; set; }
        public string GeneralStatus { get; set; } = string.Empty;
    }
}
