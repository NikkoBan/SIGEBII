using Microsoft.AspNetCore.Mvc;
using SIGEBI.Web.Repositories;
using SIGEBI.Web.Models.Users;
using SIGEBI.Application.DTOsAplication.UserDTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SIGEBI.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserApiRepository _userApiRepository;

        public UsersController(IUserApiRepository userApiRepository)
        {
            _userApiRepository = userApiRepository;
        }

        // GET: /Users/
        public async Task<IActionResult> Index()
        {
            var userDtos = await _userApiRepository.GetAllAsync() ?? new List<UserDisplayDto>();
            var viewModels = userDtos.Select(MapToViewModel).ToList();
            return View(viewModels); // Espera IEnumerable<UserViewModel>
        }

        // GET: /Users/Details/{id}
        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0)
            {
                ViewBag.ErrorMessage = "ID de usuario inválido para la búsqueda de detalles.";
                return View();
            }

            var dto = await _userApiRepository.GetByIdAsync(id);
            if (dto == null)
            {
                ViewBag.ErrorMessage = $"Usuario con ID {id} no encontrado.";
                return View();
            }

            var vm = MapToViewModel(dto);
            return View(vm); // Espera UserViewModel
        }

        // GET: /Users/Edit/{id}
        public async Task<IActionResult> Edit(int id)
        {
            if (id <= 0)
            {
                ViewBag.ErrorMessage = "ID de usuario inválido para edición.";
                return View();
            }

            var dto = await _userApiRepository.GetByIdAsync(id);
            if (dto == null)
            {
                ViewBag.ErrorMessage = "Usuario no encontrado para edición.";
                return View();
            }

            var updateDto = new UserUpdateDto
            {
                UserId = dto.UserId,
                FullName = dto.FullName,
                InstitutionalEmail = dto.InstitutionalEmail,
                InstitutionalIdentifier = dto.InstitutionalIdentifier,
                RoleId = dto.RoleId,
                IsActive = dto.IsActive,
                UpdatedBy = "" // Puedes setear esto desde el usuario logueado si aplica
            };
            return View(updateDto); // Espera UserUpdateDto
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UserUpdateDto model)
        {
            if (model == null || id != model.UserId)
            {
                ViewBag.ErrorMessage = "ID de usuario no coincide o modelo nulo.";
                return View(model);
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var success = await _userApiRepository.UpdateAsync(id, model);

            if (success)
            {
                TempData["SuccessMessage"] = "Usuario actualizado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.ErrorMessage = "Error al actualizar el usuario.";
                return View(model);
            }
        }

        // GET: /Users/Delete/{id}
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                ViewBag.ErrorMessage = "ID de usuario inválido para borrado.";
                return View();
            }

            var dto = await _userApiRepository.GetByIdAsync(id);
            if (dto == null)
            {
                ViewBag.ErrorMessage = "Usuario no encontrado para confirmar borrado.";
                return View();
            }

            var vm = MapToViewModel(dto);
            return View(vm); // Espera UserViewModel
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var success = await _userApiRepository.DeleteAsync(id);

            if (success)
            {
                TempData["SuccessMessage"] = "Usuario eliminado (lógicamente) exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.ErrorMessage = "Error al eliminar el usuario.";
                return RedirectToAction(nameof(Delete), new { id });
            }
        }

        // GET: /Users/Register
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserCreationDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var success = await _userApiRepository.RegisterAsync(model);

            if (success)
            {
                TempData["SuccessMessage"] = "Usuario registrado exitosamente. Ya puede iniciar sesión.";
                return RedirectToAction(nameof(Login));
            }
            else
            {
                ViewBag.ErrorMessage = "Error al registrar el usuario.";
                return View(model);
            }
        }

        // GET: /Users/Login
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequest model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var success = await _userApiRepository.LoginAsync(model.InstitutionalEmail, model.Password);

            if (success)
            {
                TempData["SuccessMessage"] = "¡Login exitoso!";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.ErrorMessage = "Credenciales inválidas o error desconocido.";
                return View(model);
            }
        }

        // Utilidad para mapear DTO a ViewModel
        private UserViewModel MapToViewModel(UserDisplayDto dto)
        {
            return new UserViewModel
            {
                UserId = dto.UserId,
                FullName = dto.FullName,
                InstitutionalEmail = dto.InstitutionalEmail,
                InstitutionalIdentifier = dto.InstitutionalIdentifier,
                RoleId = dto.RoleId,
                IsActive = dto.IsActive,
                RegistrationDate = dto.RegistrationDate
            };
        }
    }
}