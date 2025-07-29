using Microsoft.AspNetCore.Mvc;
using SIGEBI.Web.Repositories;
using SIGEBI.Web.Models.UserHistory;
using SIGEBI.Application.DTOsAplication.UserHistoryDTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SIGEBI.Web.Controllers
{
    public class UserHistoryController : Controller
    {
        private readonly IUserHistoryApiRepository _historyApiRepository;

        public UserHistoryController(IUserHistoryApiRepository historyApiRepository)
        {
            _historyApiRepository = historyApiRepository;
        }

        // GET: /UserHistory/
        public async Task<IActionResult> Index()
        {
            var historyDtos = await _historyApiRepository.GetAllAsync() ?? new List<UserHistoryDisplayDto>();
            var viewModels = historyDtos.Select(MapToViewModel).ToList();
            return View(viewModels); // Espera IEnumerable<UserHistoryViewModel>
        }

        // GET: /UserHistory/Details/{id}
        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0)
            {
                ViewBag.ErrorMessage = "ID de historial inválido.";
                return View();
            }

            var dto = await _historyApiRepository.GetByIdAsync(id);
            if (dto == null)
            {
                ViewBag.ErrorMessage = $"Entrada de historial con ID {id} no encontrada.";
                return View();
            }

            var vm = MapToViewModel(dto);
            return View(vm); // Espera UserHistoryViewModel
        }

        // GET: /UserHistory/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /UserHistory/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserHistoryCreationDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var success = await _historyApiRepository.CreateAsync(model);

            if (success)
            {
                TempData["SuccessMessage"] = "Entrada de historial registrada exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.ErrorMessage = "Error al registrar la entrada de historial.";
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

            var dto = await _historyApiRepository.GetByIdAsync(id);
            if (dto == null)
            {
                ViewBag.ErrorMessage = "Entrada de historial no encontrada para borrar.";
                return View();
            }

            var vm = MapToViewModel(dto);
            return View(vm); // Espera UserHistoryViewModel
        }

        // POST: /UserHistory/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var success = await _historyApiRepository.DeleteAsync(id);

            if (success)
            {
                TempData["SuccessMessage"] = "Entrada de historial eliminada exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.ErrorMessage = "Error al eliminar la entrada de historial.";
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

            var dtos = await _historyApiRepository.GetByUserIdAsync(userId) ?? new List<UserHistoryDisplayDto>();
            var vms = dtos.Select(MapToViewModel).ToList();
            return View("Index", vms);
        }

        // Utilidad para mapear DTO a ViewModel
        private UserHistoryViewModel MapToViewModel(UserHistoryDisplayDto dto)
        {
            return new UserHistoryViewModel
            {
                LogId = dto.LogId,
                UserId = dto.UserId,
                EnteredEmail = dto.EnteredEmail,
                AttemptDate = dto.AttemptDate,
                IpAddress = dto.IpAddress,
                UserAgent = dto.UserAgent,
                IsSuccessful = dto.IsSuccessful,
                FailureReason = dto.FailureReason,
                ObtainedRole = dto.ObtainedRole
            };
        }
    }
}