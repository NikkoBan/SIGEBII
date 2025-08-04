using System.ComponentModel.DataAnnotations;

namespace SIGEBI.Web0.Models.Book;

public abstract class BaseBookModel
{
    [Display(Name = "Título")]
    [Required(ErrorMessage = "El título es obligatorio.")]
    public required string Title { get; set; }

    [Display(Name = "ISBN")]
    [Required(ErrorMessage = "El ISBN es obligatorio.")]
    public required string ISBN { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [Display(Name = "Fecha de Publicación")]
    [Required(ErrorMessage = "La fecha de publicación es obligatoria.")]
    public DateTime? PublicationDate { get; set; }

    [Display(Name = "Categoría")]
    [Required(ErrorMessage = "La categoría es obligatoria.")]
    public int CategoryId { get; set; }

    [Display(Name = "Editorial")]
    [Required(ErrorMessage = "La editorial es obligatoria.")]
    public int PublisherId { get; set; }

    [Display(Name = "Idioma")]
    [Required(ErrorMessage = "El idioma es obligatorio.")]
    public required string Language { get; set; }

    [Display(Name = "Resumen")]
    [Required(ErrorMessage = "El resumen es obligatorio.")]
    public required string Summary { get; set; }

    [Display(Name = "Total de Copias")]
    [Required(ErrorMessage = "El número total de copias es obligatorio.")]
    public int TotalCopies { get; set; }

    [Display(Name = "Autores")]
    [Required(ErrorMessage = "Debe seleccionar al menos un autor.")]
    public List<int> AuthorIds { get; set; } = new();
}