using System.ComponentModel.DataAnnotations;
namespace SIGEBI.Web0.Models.Author
{
    public class EditAuthorModel
    {
        public int AuthorId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime BirthDate { get; set; }
        public required string Nationality { get; set; }
    }
}