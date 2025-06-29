
namespace SIGEBI.Application.Dtos
{
    public class CreateBookDTO : AuditableDTO
    {
        public string Title { get; set; }
        public string ISBN { get; set; }
        public DateTime? PublicationDate { get; set; }
        public int CategoryId { get; set; }
        public int PublisherId { get; set; }
        public string Language { get; set; }
        public string Summary { get; set; }
        public int TotalCopies { get; set; }
        public int AvailableCopies { get; set; }
        public string GeneralStatus { get; set; }
    }
}
