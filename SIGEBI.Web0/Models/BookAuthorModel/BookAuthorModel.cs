using System.ComponentModel.DataAnnotations;
using static SIGEBI.Web0.Models.BookAuthorModel.BookAuthorBase;

namespace SIGEBI.Web0.Models.BookAuthor
{
    public class BookAuthorModel : BaseBookAuthorModel
    {
        [Display(Name = "Título del Libro")]
        public string? BookTitle { get; set; }

        [Display(Name = "Nombre del Autor")]
        public string? AuthorName { get; set; }
    }
}
