using System.ComponentModel.DataAnnotations;

namespace SIGEBI.Web0.Models.Book
{
    public class BookModel
    {
        public int BookId { get; set; }
        public required string Title { get; set; }
        public required string ISBN { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? PublicationDate { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public int PublisherId { get; set; }
        public string PublisherName { get; set; } = string.Empty;
        public required string Language { get; set; }
        public required string Summary { get; set; }
        public int TotalCopies { get; set; }
        public int AvailableCopies { get; set; }
        public required string GeneralStatus { get; set; }
        public bool IsAvailable { get; set; }
    }
}
