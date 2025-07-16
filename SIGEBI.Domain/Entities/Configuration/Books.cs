
using SIGEBI.Domain.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGEBI.Domain.Entities.Configuration
{
    public  class Books : AuditableEntity
    {
        [Key]
        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("BookId")]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(13)]
        public string ISBN { get; set; } = string.Empty;

        public DateTime? PublicationDate { get; set; }

        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }

        [ForeignKey(nameof(Publisher))]
        public int PublisherId { get; set; }

        [MaxLength(50)]
        public string Language { get; set; } = string.Empty;

        public string Summary { get; set; } = string.Empty;

        public int TotalCopies { get; set; }

        public int AvailableCopies { get; set; }

        [Required]
        [MaxLength(50)]
        public string GeneralStatus { get; set; } = string.Empty;

        // Relaciones
        public virtual Categories? Category { get; set; }

        public virtual Publisher? Publisher { get; set; }

        public virtual ICollection<BookAuthor> BooksAuthors { get; set; } = new List<BookAuthor>();
    }

}






