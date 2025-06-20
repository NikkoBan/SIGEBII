using SIGEBI.Domain.Base;


namespace SIGEBI.Domain.Entities.Configuration
{
    public class BooksAuthors : AuditableEntity
    {

        public int BookId { get; set; }
        public Books Books { get; set; }

        public int AuthorId { get; set; }
        public Authors Authors { get; set; }
    }
}
