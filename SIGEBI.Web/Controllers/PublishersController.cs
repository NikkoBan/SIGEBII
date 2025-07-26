using Microsoft.AspNetCore.Mvc;
using SIGEBI.Web.Models.Publishers;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace SIGEBI.Web.Controllers
{
    public class PublishersController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl = "https://localhost:7276/api/Publishers";

        public PublishersController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        // GET: Publishers
        public async Task<IActionResult> Index()
        {
            List<PublishersViewModel> publishers = new();
            try
            {
                var response = await _httpClient.GetAsync(_apiBaseUrl);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    publishers = JsonSerializer.Deserialize<List<PublishersViewModel>>(responseString) ?? new List<PublishersViewModel>();
                }
                else
                {
                    ViewBag.Message = "No se pudo obtener la lista de editoriales.";
                }
            }
            catch
            {
                ViewBag.Message = "Error al obtener la lista de editoriales.";
            }
            return View(publishers);
        }

        // GET: Publishers/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0)
            {
                ViewBag.Message = "ID inválido.";
                return View(null);
            }

            PublishersViewModel? publisher = null;
            try
            {
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}/{id}");

                var responseString = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    publisher = JsonSerializer.Deserialize<PublishersViewModel>(responseString) ?? new PublishersViewModel
                    {
                        id = 0,
                        publisherName = string.Empty,
                        address = string.Empty,
                        phoneNumber = string.Empty,
                        email = string.Empty,
                        website = string.Empty,
                        notes = string.Empty
                    };
                }
                else
                {
                    var apiResponse = JsonSerializer.Deserialize<ApiResponseModel<object>>(responseString);
                    ViewBag.Message = apiResponse?.message ?? "Editorial no encontrada.";
                }
            }
            catch
            {
                ViewBag.Message = "Error al obtener la editorial.";
            }
            return View(publisher);
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
                var response = await _httpClient.PostAsJsonAsync(_apiBaseUrl, model);
                var responseString = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<ApiResponseModel<PublishersViewModel>>(responseString);

                if (response.IsSuccessStatusCode && apiResponse?.success == true)
                    return RedirectToAction(nameof(Index));

                ViewBag.Message = apiResponse?.message ?? "Error al crear la editorial.";
            }
            catch
            {
                ViewBag.Message = "Error al crear la editorial.";
            }
            return View(model);
        }

        // GET: Publishers/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id <= 0)
            {
                ViewBag.Message = "ID inválido.";
                return View(null);
            }

            PublisherUpdateModel? updateModel = null;
            try
            {
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}/{id}");
                var responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var publisher = JsonSerializer.Deserialize<PublishersViewModel>(responseString);
                    if (publisher != null)
                    {
                        updateModel = new PublisherUpdateModel
                        {
                            id = publisher.id,
                            publisherName = publisher.publisherName,
                            address = publisher.address,
                            phoneNumber = publisher.phoneNumber,
                            email = publisher.email,
                            website = publisher.website,
                            notes = publisher.notes
                        };
                    }
                    else
                    {
                        updateModel = new PublisherUpdateModel
                        {
                            id = 0,
                            publisherName = string.Empty,
                            address = string.Empty,
                            phoneNumber = string.Empty,
                            email = string.Empty,
                            website = string.Empty,
                            notes = string.Empty
                        };
                    }
                }
                else
                {
                    var apiResponse = JsonSerializer.Deserialize<ApiResponseModel<object>>(responseString);
                    ViewBag.Message = apiResponse?.message ?? "Editorial no encontrada.";
                }
            }
            catch
            {
                ViewBag.Message = "Error al obtener la editorial para editar.";
            }
            return View(updateModel);
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
                var response = await _httpClient.PutAsJsonAsync($"{_apiBaseUrl}/{model.id}", model);
                var responseString = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<ApiResponseModel<PublishersViewModel>>(responseString);

                if (response.IsSuccessStatusCode && apiResponse?.success == true)
                    return RedirectToAction(nameof(Index));

                ViewBag.Message = apiResponse?.message ?? "Error al actualizar la editorial.";
            }
            catch
            {
                ViewBag.Message = "Error al actualizar la editorial.";
            }
            return View(model);
        }
    }
}