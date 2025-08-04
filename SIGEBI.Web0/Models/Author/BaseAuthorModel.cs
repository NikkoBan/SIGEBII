using System.ComponentModel.DataAnnotations;

namespace SIGEBI.Web0.Models.Author
{
    public abstract class BaseAuthorModel
    {
        [Display(Name = "Nombre")]
        public required string FirstName { get; set; }

        [Display(Name = "Apellido")]
        public required string LastName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha De Nacimiento")]
        public DateTime BirthDate { get; set; }

        [Display(Name = "Nacionalidad")]
        public required string Nationality { get; set; }
    }
}
