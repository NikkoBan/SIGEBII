using System.ComponentModel.DataAnnotations;

namespace SIGEBI.Web0.Models.BookAuthorModel
{
    public abstract class BookAuthorBase
    {
        public abstract class BaseBookAuthorModel
        {
            [Display(Name = "ID Libro")]
            public int BookId { get; set; }

            [Display(Name = "ID Autor")]
            public int AuthorId { get; set; }
        }
    }
}
