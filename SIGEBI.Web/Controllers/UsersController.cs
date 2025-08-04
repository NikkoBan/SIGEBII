using Microsoft.AspNetCore.Mvc;
using SIGEBI.Web.Models.Users;
using SIGEBI.Web.Repositories.interfaces;
using System.Threading.Tasks;

namespace SIGEBI.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserWebRepository _repo;

        public UsersController(IUserWebRepository repo)
        {
            _repo = repo;
        }

        // GET: /Users/
        public async Task<IActionResult> Index()
        {
            var resp = await _repo.GetAllAsync();
            if (!resp.Success)
                ViewBag.ErrorMessage = resp.Message + $" TraceId: {resp.TraceId}";
            return View(resp.Data);
        }

        // GET: /Users/Details/{id}
        public async Task<IActionResult> Details(int id)
        {
            var resp = await _repo.GetByIdAsync(id);
            UserViewModel? model = resp.Data; // Solución CS8601: variable nullable
            if (!resp.Success || model == null)
            {
                ViewBag.ErrorMessage = resp.Message + $" TraceId: {resp.TraceId}";
                return View();
            }
            return View(model);
        }

        // GET: /Users/Register
        public IActionResult Register() => View();

        // POST: /Users/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserRequest model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var resp = await _repo.RegisterAsync(model);
            if (resp.Success && resp.Data)
            {
                TempData["SuccessMessage"] = "Usuario registrado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.ErrorMessage = resp.Message + $" TraceId: {resp.TraceId}";
                return View(model);
            }
        }

        // GET: /Users/Edit/{id}
        public async Task<IActionResult> Edit(int id)
        {
            var resp = await _repo.GetByIdAsync(id);
            if (!resp.Success || resp.Data == null)
            {
                ViewBag.ErrorMessage = resp.Message + $" TraceId: {resp.TraceId}";
                return View();
            }

            var updateModel = new UserUpdateRequest
            {
                UserId = resp.Data.UserId,
                FullName = resp.Data.FullName,
                InstitutionalEmail = resp.Data.InstitutionalEmail,
                InstitutionalIdentifier = resp.Data.InstitutionalIdentifier,
                RoleId = resp.Data.RoleId,
                IsActive = resp.Data.IsActive,
                UpdatedBy = "" // Setea aquí el usuario logueado si aplica
            };

            return View(updateModel);
        }

        // POST: /Users/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UserUpdateRequest model)
        {
            if (model == null || id != model.UserId)
            {
                ViewBag.ErrorMessage = "ID de usuario no coincide o modelo nulo.";
                return View(model);
            }

            if (!ModelState.IsValid)
                return View(model);

            var resp = await _repo.UpdateAsync(id, model);

            if (resp.Success && resp.Data)
            {
                TempData["SuccessMessage"] = "Usuario actualizado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.ErrorMessage = resp.Message + $" TraceId: {resp.TraceId}";
                return View(model);
            }
        }

        // GET: /Users/Delete/{id}
        public async Task<IActionResult> Delete(int id)
        {
            var resp = await _repo.GetByIdAsync(id);
            if (!resp.Success || resp.Data == null)
            {
                ViewBag.ErrorMessage = resp.Message + $" TraceId: {resp.TraceId}";
                return View();
            }
            return View(resp.Data);
        }

        // POST: /Users/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var resp = await _repo.DeleteAsync(id);

            if (resp.Success && resp.Data)
            {
                TempData["SuccessMessage"] = "Usuario eliminado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.ErrorMessage = resp.Message + $" TraceId: {resp.TraceId}";
                return RedirectToAction(nameof(Delete), new { id });
            }
        }

        // GET: /Users/Login
        public IActionResult Login() => View();

        // POST: /Users/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> Login(LoginRequest model)
        {
            if (!ModelState.IsValid)
                return Task.FromResult<IActionResult>(View(model));

            // Aquí deberías tener un método LoginAsync que devuelva ApiResponse<bool> en IUserWebRepository,
            // por ahora lo dejamos como ejemplo simulado:
            TempData["SuccessMessage"] = "¡Login exitoso! (simulado)";
            return Task.FromResult<IActionResult>(RedirectToAction("Index", "Home"));
        }
    }
}