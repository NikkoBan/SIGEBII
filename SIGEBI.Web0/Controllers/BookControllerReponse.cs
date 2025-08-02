
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using SIGEBI.Web0.Models.Book;
using SIGEBI.Web0.Services.Book;

namespace SIGEBI.Web0.Controllers
{
    public class BookController : Controller

    {
        private readonly IBookWebService _bookService;
        private readonly ILogger<BookController> _logger;

        public BookController(IBookWebService bookService, ILogger<BookController> logger)
        {
            _bookService = bookService;
            _logger = logger;
        }


        // GET: BookControllerReponse

        // GET: /Book/Index
        public async Task<IActionResult> Index()
        {
            try
            {
                var books = await _bookService.GetAllBooksAsync();
                return View(books);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de libros.");
                TempData["ErrorMessage"] = $"Ocurrió un error al cargar los libros: {ex.Message}";
                return View(new List<BookModel>());
            }
        }

        // GET: /Book/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "ID de libro no válido.";
                return RedirectToAction(nameof(Index));
            }
            try
            {
                var book = await _bookService.GetBookDetailsAsync(id);
                if (book == null)
                {
                    _logger.LogWarning("Libro con ID {BookId} no encontrado para detalles.", id);
                    TempData["ErrorMessage"] = $"No se encontró el libro con ID {id}.";
                    return NotFound();
                }
                return View(book);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener detalles del libro con ID {BookId}.", id);
                TempData["ErrorMessage"] = $"Ocurrió un error al cargar los detalles del libro: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }
        
        // GET: /Book/Create
        public async Task<IActionResult> Create()
        {
            await _bookService.PopulateDropdownsForBookForm(ViewBag);
            return View();
        }

        // POST: /Book/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateBookModel model)
        {
            if (!ModelState.IsValid)
            {
                await _bookService.PopulateDropdownsForBookForm(ViewBag);
                return View(model);
            }

            try
            {
                // Le pide al servicio que cree el libro
                bool isSuccess = await _bookService.CreateBookAsync(model);
                if (isSuccess)
                {
                    TempData["SuccessMessage"] = "Libro creado exitosamente.";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "No se pudo crear el libro. Por favor, intente de nuevo.");
                }
            }
            catch (ApplicationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Ocurrió un error inesperado al crear el libro: {ex.Message}");
            }

            await _bookService.PopulateDropdownsForBookForm(ViewBag);
            return View(model);
        }

        // GET: /Book/Edit/5

        // GET: /Book/Edit/5

        public async Task<IActionResult> Edit(int id)
        {
            EditBookModel? editModel = null;
            try
            {
                // Le pide al servicio que prepare el modelo para edición
                editModel = await _bookService.GetEditBookModelByIdAsync(id);
                if (editModel == null)
                {
                    TempData["ErrorMessage"] = "Libro no encontrado para edición.";
                    return NotFound();
                }
                await _bookService.PopulateDropdownsForBookForm(ViewBag);
            }
            catch (ApplicationException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return NotFound();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Ocurrió un error inesperado al cargar el libro para edición: {ex.Message}";
                return View("Error");
            }
            return View(editModel);
        }
        


        
        // POST:/Book/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditBookModel model)
        {
                if (id != model.BookId)
                {
                    TempData["ErrorMessage"] = "ID de libro no coincide.";
                    return NotFound();
                }

                if (!ModelState.IsValid)
                {
                    await _bookService.PopulateDropdownsForBookForm(ViewBag);
                    return View(model);
                }

                try
                {
                    bool isSuccess = await _bookService.UpdateBookAsync(id, model);
                    if (isSuccess)
                    {
                        TempData["SuccessMessage"] = "Libro actualizado exitosamente.";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "No se pudo actualizar el libro.");
                    }
                }
                catch (ApplicationException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Ocurrió un error inesperado al actualizar el libro: {ex.Message}");
                }

                await _bookService.PopulateDropdownsForBookForm(ViewBag);
                return View(model);
         }




        // GET: BookControllerReponse/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: BookControllerReponse/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

      

    }

}
    

