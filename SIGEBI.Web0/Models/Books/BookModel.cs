
using System.ComponentModel.DataAnnotations;
namespace SIGEBI.Web0.Models.Book;

public class BookModel : BaseBookModel
{
    public int BookId { get; set; }

    [Display(Name = "Nombre de Categoría")]
    public string CategoryName { get; set; } = string.Empty;

    [Display(Name = "Nombre de Editorial")]
    public string PublisherName { get; set; } = string.Empty;

    [Display(Name = "Copias Disponibles")]
    public int AvailableCopies { get; set; }

    [Display(Name = "Estado General")]
    public required string GeneralStatus { get; set; } = string.Empty;

    [Display(Name = "Disponible")]
    public bool IsAvailable => GeneralStatus.Equals("Disponible", StringComparison.OrdinalIgnoreCase);

}