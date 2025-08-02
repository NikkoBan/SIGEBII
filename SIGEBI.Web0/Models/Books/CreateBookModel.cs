using System.ComponentModel.DataAnnotations;

namespace SIGEBI.Web0.Models.Book
{
    public class CreateBookModel
    {
        [Required]
        public required string Title { get; set; }

        [Required]
        public required string ISBN { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required]
        public DateTime? PublicationDate { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public int PublisherId { get; set; }

        [Required]
        public required string Language { get; set; }

        [Required]
        public required string Summary { get; set; }

        [Required]
        public int TotalCopies { get; set; }

        [Required]
        public List<int> AuthorIds { get; set; } = new();
    }
}
