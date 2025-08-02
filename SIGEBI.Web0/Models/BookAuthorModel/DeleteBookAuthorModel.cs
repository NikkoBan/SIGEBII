using System.ComponentModel.DataAnnotations;

namespace SIGEBI.Web0.Models.BookAuthor
{
    public class DeleteBookAuthorModel
    {
        [Display(Name = "ID Libro")]
        public int BookId { get; set; }

        [Display(Name = "ID Autor")]
        public int AuthorId { get; set; }

        [Display(Name = "Título del Libro")]
        public string? BookTitle { get; set; }
        [Display(Name = "Nombre del Autor")]
        public string? AuthorName { get; set; }
    }
}