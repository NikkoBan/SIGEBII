using Microsoft.AspNetCore.Mvc;
using SIGEBI.Web0.Interfaz.BookAuthor;
using SIGEBI.Web0.Models.BookAuthor;
using SIGEBI.Web0.Interfaz.Author;
using Microsoft.AspNetCore.Mvc.Rendering;

using SIGEBI.Web0.Interfaces.Book;
using SIGEBI.Web0.Services.BookAuthor;

namespace SIGEBI.Web0.Controllers
{
    public class BookAuthorController : Controller
    {
        private readonly IBookAuthorWebService _bookAuthorService;
        private readonly ILogger<BookAuthorController> _logger;

        public BookAuthorController(IBookAuthorWebService bookAuthorService, ILogger<BookAuthorController> logger)
        {
            _bookAuthorService = bookAuthorService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var relationships = await _bookAuthorService.GetAllRelationshipsAsync();
                return View(relationships);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar las relaciones Libro-Autor.");
                TempData["ErrorMessage"] = $"Ocurrió un error inesperado: {ex.Message}";
                return View(new List<BookAuthorModel>());
            }
        }
        [HttpGet("{bookId:int}/{authorId:int}")]
        public async Task<IActionResult> Details(int bookId, int authorId)
        {
            try
            {
                var relationship = await _bookAuthorService.GetRelationshipByIdsAsync(bookId, authorId);
                if (relationship == null)
                {
                    TempData["ErrorMessage"] = "Relación Libro-Autor no encontrada.";
                    return NotFound();
                }
                return View(relationship);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar los detalles de la relación Libro-Autor.");
                TempData["ErrorMessage"] = $"Ocurrió un error inesperado: {ex.Message}";
                return View("Error");
            }
        }
        public async Task<IActionResult> Create()
        {
            await _bookAuthorService.PopulateDropdownsForBookAuthorForm(ViewBag);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateBookAuthorModel model)
        {
            if (!ModelState.IsValid)
            {
                await _bookAuthorService.PopulateDropdownsForBookAuthorForm(ViewBag);
                return View(model);
            }
            try
            {
                bool isSuccess = await _bookAuthorService.CreateRelationshipAsync(model);
                if (isSuccess)
                {
                    TempData["SuccessMessage"] = "Relación Libro-Autor creada exitosamente.";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "No se pudo crear la relación Libro-Autor. Puede que ya exista o hubo un problema en la API.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al crear la relación Libro-Autor.");
                ModelState.AddModelError(string.Empty, $"Ocurrió un error inesperado: {ex.Message}");
            }

            await _bookAuthorService.PopulateDropdownsForBookAuthorForm(ViewBag);
            return View(model);
        }
        public async Task<IActionResult> Delete(int bookId, int authorId)
        {
            try
            {
                var deleteModel = await _bookAuthorService.GetDeleteModelAsync(bookId, authorId);
                if (deleteModel == null)
                {
                    TempData["ErrorMessage"] = "Relación Libro-Autor no encontrada para eliminar.";
                    return NotFound();
                }
                return View(deleteModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar relación Libro-Autor para eliminación.");
                TempData["ErrorMessage"] = $"Ocurrió un error inesperado: {ex.Message}";
                return View("Error");
            }
        }

            [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int bookId, int authorId)
        {
            try
            {
                bool isSuccess = await _bookAuthorService.DeleteRelationshipAsync(bookId, authorId);
                if (isSuccess)
                {
                    TempData["SuccessMessage"] = "Relación Libro-Autor eliminada exitosamente.";
                }
                else
                {
                    TempData["ErrorMessage"] = "No se pudo eliminar la relación Libro-Autor. Puede que no exista.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al eliminar relación Libro-Autor: LibroId {BookId}, AuthorId {AuthorId}", bookId, authorId);
                TempData["ErrorMessage"] = $"Ocurrió un error inesperado: {ex.Message}";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}

      