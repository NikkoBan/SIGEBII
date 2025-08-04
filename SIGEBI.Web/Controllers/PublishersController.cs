using Microsoft.AspNetCore.Mvc;
using SIGEBI.Web.Models.Publishers;
using SIGEBI.Web.Services;
using System.Threading.Tasks;

namespace SIGEBI.Web.Controllers
{
    public class PublishersController : Controller
    {
        private readonly IPublishersHttpService _publishersService;
        private readonly INotificationService _notificationService;

        public PublishersController(IPublishersHttpService publishersService, INotificationService notificationService)
        {
            _publishersService = publishersService;
            _notificationService = notificationService;
        }

        // GET: Publishers
        public async Task<IActionResult> Index()
        {
            try
            {
                var publishers = await _publishersService.GetAllAsync();
                return View(publishers);
            }
            catch (Exception)
            {
                _notificationService.Error("Ocurrió un error inesperado al cargar el listado.");
                return View(new List<PublishersViewModel>());
            }
        }

        // GET: Publishers/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0)
            {
                _notificationService.Warning("ID inválido.");
                return View(null);
            }

            try
            {
                var publisher = await _publishersService.GetByIdAsync(id);
                if (publisher == null)
                {
                    _notificationService.Info("Editorial no encontrada.");
                }
                return View(publisher);
            }
            catch (Exception)
            {
                _notificationService.Error("Ocurrió un error inesperado al consultar la editorial.");
                return View(null);
            }
        }

        // GET: Publishers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Publishers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PublisherCreateModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var response = await _publishersService.CreateAsync(model);

                if (response?.success == true)
                {
                    _notificationService.Success("Editorial creada correctamente.");
                    return RedirectToAction(nameof(Index));
                }

                _notificationService.Error(response?.message ?? "Error al crear la editorial.");
                return View(model);
            }
            catch (Exception)
            {
                _notificationService.Error("Ocurrió un error inesperado al crear la editorial.");
                return View(model);
            }
        }

        // GET: Publishers/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id <= 0)
            {
                _notificationService.Warning("ID inválido.");
                return View(null);
            }

            try
            {
                var publisher = await _publishersService.GetByIdAsync(id);
                if (publisher == null)
                {
                    _notificationService.Info("Editorial no encontrada.");
                    return View(null);
                }

                var updateModel = new PublisherUpdateModel
                {
                    id = publisher.id,
                    publisherName = publisher.publisherName,
                    address = publisher.address,
                    phoneNumber = publisher.phoneNumber,
                    email = publisher.email,
                    website = publisher.website,
                    notes = publisher.notes
                };

                return View(updateModel);
            }
            catch (Exception)
            {
                _notificationService.Error("Ocurrió un error inesperado al cargar la editorial para edición.");
                return View(null);
            }
        }

        // POST: Publishers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PublisherUpdateModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var response = await _publishersService.UpdateAsync(model);

                if (response?.success == true)
                {
                    _notificationService.Success("Editorial actualizada correctamente.");
                    return RedirectToAction(nameof(Index));
                }

                _notificationService.Error(response?.message ?? "Error al actualizar la editorial.");
                return View(model);
            }
            catch (Exception)
            {
                _notificationService.Error("Ocurrió un error inesperado al actualizar la editorial.");
                return View(model);
            }
        }
    }
}