using Microsoft.AspNetCore.Mvc;

using SIGEBI.Web0.Models.Author;
using SIGEBI.Web0.Services.Author;

namespace SIGEBI.Web0.Controllers
{

    public class AuthorController: Controller
    {
        private readonly IAuthorWebService _authorWebService;

        public AuthorController(IAuthorWebService authorWebService)
        {
            _authorWebService = authorWebService;
        }
        // GET: AuthorControllerResponse
        public async Task<IActionResult> Index()
        {

            try
            {
                var authors = await _authorWebService.GetAllAuthorsAsync();
                return View(authors);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Ocurrió un error inesperado al cargar los autores: {ex.Message}";
                return View(new List<Authormodel>());
            }
        }
            
        
        // GET: AuthorControllerResponse/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var author = await _authorWebService.GetAuthorByIdAsync(id);
                if (author == null)
                {
                    TempData["ErrorMessage"] = "Autor no encontrado.";
                    return NotFound();
                }
                return View(author);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Ocurrió un error inesperado al obtener el autor: {ex.Message}";
                return View("Error");
            }
        }
        // GET: AuthorControllerResponse/Create

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

       
        // POST: AuthorControllerResponse/Create

        [ValidateAntiForgeryToken]

        [HttpPost]

       
        public async Task<IActionResult> Create(CreateAuthorModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                bool isSuccess = await _authorWebService.CreateAuthorAsync(model);
                if (isSuccess)
                {
                    TempData["SuccessMessage"] = "Autor creado exitosamente.";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "No se pudo crear el autor. Por favor, intente de nuevo.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Ocurrió un error inesperado al crear el autor: {ex.Message}");
            }
            return View(model);
        }

       

        // GET: AuthorControllerResponse/Edit/5
        public async Task<IActionResult>  Edit(int id)
        {

            try
            {
                var editModel = await _authorWebService.GetEditAuthorModelByIdAsync(id);
                if (editModel == null)
                {
                    TempData["ErrorMessage"] = "Autor no encontrado para edición.";
                    return NotFound();
                }
                return View(editModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Ocurrió un error inesperado al cargar el autor para edición: {ex.Message}";
                return View("Error");
            }
        }


        // POST: AuthorControllerResponse/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditAuthorModel model)
        {
            if (id != model.AuthorId)
            {
                TempData["ErrorMessage"] = "ID de autor no coincide.";
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                bool isSuccess = await _authorWebService.UpdateAuthorAsync(id, model);
                if (isSuccess)
                {
                    TempData["SuccessMessage"] = "Autor actualizado exitosamente.";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "No se pudo actualizar el autor.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Ocurrió un error inesperado al actualizar el autor: {ex.Message}");
            }
            return View(model);
        }

        // GET: AuthorController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var author = await _authorWebService.GetAuthorByIdAsync(id);
                if (author == null)
                {
                    TempData["ErrorMessage"] = "Autor no encontrado para eliminar.";
                    return NotFound();
                }
                return View(author);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Ocurrió un error inesperado al cargar el autor para eliminar: {ex.Message}";
                return View("Error");
            }
        }




        // GET: AuthorControllerResponse/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        // POST: AuthorControllerResponse/Delete/5
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
