using Microsoft.AspNetCore.Mvc;
using SIGEBI.Web.Models.UserHistory;
using SIGEBI.Web.Repositories.interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SIGEBI.Web.Controllers
{
    public class UserHistoryController : Controller
    {
        private readonly IUserHistoryWebRepository _repo;

        public UserHistoryController(IUserHistoryWebRepository repo)
        {
            _repo = repo;
        }

        // GET: /UserHistory/
        public async Task<IActionResult> Index()
        {
            var response = await _repo.GetAllAsync();
            if (!response.Success)
                ViewBag.ErrorMessage = response.Message + $" TraceId: {response.TraceId}";
            return View(response.Data);
        }

        // GET: /UserHistory/Details/{id}
        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0)
            {
                ViewBag.ErrorMessage = "ID de historial inválido.";
                return View();
            }

            var response = await _repo.GetByIdAsync(id);
            if (!response.Success || response.Data == null)
            {
                ViewBag.ErrorMessage = response.Message + $" TraceId: {response.TraceId}";
                return View();
            }
            return View(response.Data);
        }

        // GET: /UserHistory/Create
        public IActionResult Create() => View();

        // POST: /UserHistory/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserHistoryRequest model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var response = await _repo.CreateAsync(model);
            if (response.Success && response.Data)
            {
                TempData["SuccessMessage"] = "Entrada de historial registrada exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.ErrorMessage = response.Message + $" TraceId: {response.TraceId}";
                return View(model);
            }
        }

        // GET: /UserHistory/Delete/{id}
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                ViewBag.ErrorMessage = "ID de historial inválido para borrado.";
                return View();
            }

            var response = await _repo.GetByIdAsync(id);
            if (!response.Success || response.Data == null)
            {
                ViewBag.ErrorMessage = response.Message + $" TraceId: {response.TraceId}";
                return View();
            }
            return View(response.Data);
        }

        // POST: /UserHistory/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _repo.DeleteAsync(id);
            if (response.Success && response.Data)
            {
                TempData["SuccessMessage"] = "Entrada de historial eliminada exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.ErrorMessage = response.Message + $" TraceId: {response.TraceId}";
                return RedirectToAction(nameof(Delete), new { id });
            }
        }

        // GET: /UserHistory/GetUserHistoryByUserId/{userId}
        public async Task<IActionResult> GetUserHistoryByUserId(int userId)
        {
            if (userId <= 0)
            {
                ViewBag.ErrorMessage = "ID de usuario inválido para buscar historial.";
                return View("Index", new List<UserHistoryViewModel>());
            }

            var resp = await _repo.GetByUserIdAsync(userId);
            if (!resp.Success)
                ViewBag.ErrorMessage = resp.Message + $" TraceId: {resp.TraceId}";
            return View("Index", resp.Data);
        }
    }
}