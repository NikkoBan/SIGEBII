
using SIGEBI.Domain.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGEBI.Domain.Entities.Configuration
{
    public class Authors : AuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("AuthorId")]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        public DateTime? BirthDate { get; set; }

        [MaxLength(100)]
        public string? Nationality { get; set; }

        public ICollection<BookAuthor> BooksAuthors { get; set; } = new List<BookAuthor>();
    }
}

