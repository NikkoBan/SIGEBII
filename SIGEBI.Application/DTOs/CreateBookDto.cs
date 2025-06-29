namespace SIGEBI.Application.DTOs
{
    public class CreateBookDto
    {
        public required string Title { get; set; }
        public required string ISBN { get; set; }
        public DateTime? PublicationDate { get; set; }
        public required int CategoryId { get; set; }
        public required int PublisherId { get; set; }
        public required string Language { get; set; }
        public string? Summary { get; set; }
        public required int TotalCopies { get; set; }
        public required int AvailableCopies { get; set; }
        public required string GeneralStatus { get; set; }
    }
}
