using System.ComponentModel.DataAnnotations;
namespace SIGEBI.Web0.Models.Author
{
    public class EditAuthorModel : BaseAuthorModel
    {
       
        [Display(Name = "ID del Autor")]
        public int AuthorId { get; set; }
    }
    
       
    
}