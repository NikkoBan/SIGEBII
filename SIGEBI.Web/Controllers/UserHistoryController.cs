// SIGEBI.Web/Controllers/UserHistoryController.cs

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json; // Necesario para JsonElement y JsonValueKind
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration; // Para IConfiguration

// Modelos/DTOs del frontend ESPECÍFICOS DE LA WEB
using WebUserHistoryModels = SIGEBI.Web.Models.UserHistory; // Alias para los modelos de historial de la Web

// DTOs para comunicación con la API (UserHistoryCreationDto, UserHistoryUpdateDto). 
// Asumimos que están en SIGEBI.Application y la referencia está correcta.
using ApplicationHistoryDTOs = SIGEBI.Application.DTOsAplication.UserHistoryDTOs;

namespace SIGEBI.Web.Controllers
{
    public class UserHistoryController : Controller
    {
        private readonly string _apiBaseUrl;

        public UserHistoryController(IConfiguration configuration)
        {
            _apiBaseUrl = configuration.GetValue<string>("ApiSettings:BaseUrl") + "UserHistory";
        }

        // GET: https://localhost:7276/api/UserHistory/
        public async Task<IActionResult> Index()
        {
            List<WebUserHistoryModels.UserHistoryViewModel> historyEntries = new List<WebUserHistoryModels.UserHistoryViewModel>(); // Usar alias
            string rawJsonResponse = string.Empty; // Solución CS8600

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_apiBaseUrl);
                    HttpResponseMessage response = await client.GetAsync("");
                    rawJsonResponse = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        WebUserHistoryModels.UserHistoryApiResponse? apiResponse = null; // Usar alias
                        try
                        {
                            apiResponse = JsonSerializer.Deserialize<WebUserHistoryModels.UserHistoryApiResponse>(rawJsonResponse,
                                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }); // Usar alias
                        }
                        catch (JsonException) { /* Ignorar */ }

                        if (apiResponse != null && apiResponse.IsSuccess && apiResponse.DataAsList != null)
                        {
                            historyEntries = apiResponse.DataAsList;
                            ViewBag.SuccessMessage = apiResponse.Message;
                        }
                        else if (apiResponse != null && !apiResponse.IsSuccess)
                        {
                            ViewBag.ErrorMessage = apiResponse.Message ?? "API de Historial indicó un fallo o datos nulos.";
                        }
                        else
                        {
                            try
                            {
                                historyEntries = JsonSerializer.Deserialize<List<WebUserHistoryModels.UserHistoryViewModel>>(rawJsonResponse, // Usar alias
                                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                                ViewBag.WarningMessage = "La API de Historial no devolvió la estructura esperada (UserHistoryApiResponse), pero los datos se pudieron cargar directamente como lista.";
                            }
                            catch (Exception directDeserializeEx)
                            {
                                ViewBag.ErrorMessage = $"Error de formato JSON inesperado para el listado de historial. Detalles: {directDeserializeEx.Message}. JSON recibido: {rawJsonResponse}";
                            }
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMessage = $"Error HTTP en la API: {response.StatusCode} - {response.ReasonPhrase}. Detalles: {rawJsonResponse}";
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Ocurrió una excepción al obtener el historial de usuarios: {ex.Message}";
            }

            return View(historyEntries);
        }

        // GET: https://localhost:7276/api/UserHistory/{id}
        public async Task<IActionResult> Details(int id)
        {
            WebUserHistoryModels.UserHistoryViewModel? historyEntry = null; // Usar alias
            string rawJsonResponse = string.Empty; // Solución CS8600

            if (id <= 0)
            {
                ViewBag.ErrorMessage = "ID de historial inválido.";
                return View(historyEntry);
            }

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_apiBaseUrl);
                    HttpResponseMessage response = await client.GetAsync($"{id}");
                    rawJsonResponse = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        WebUserHistoryModels.UserHistoryApiResponse? apiResponse = null; // Usar alias
                        try
                        {
                            apiResponse = JsonSerializer.Deserialize<WebUserHistoryModels.UserHistoryApiResponse>(rawJsonResponse,
                                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }); // Usar alias
                        }
                        catch (JsonException) { /* Ignorar */ }

                        if (apiResponse != null && apiResponse.IsSuccess && apiResponse.DataAsSingleObject != null)
                        {
                            historyEntry = apiResponse.DataAsSingleObject;
                            ViewBag.SuccessMessage = apiResponse.Message;
                        }
                        else if (apiResponse != null && !apiResponse.IsSuccess)
                        {
                            ViewBag.ErrorMessage = apiResponse.Message ?? "API de Detalles de Historial indicó un fallo.";
                        }
                        else
                        {
                            try // Intentar directamente a UserHistoryViewModel
                            {
                                historyEntry = JsonSerializer.Deserialize<WebUserHistoryModels.UserHistoryViewModel>(rawJsonResponse, // Usar alias
                                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                                ViewBag.WarningMessage = "La API de Detalles de Historial no devolvió la estructura esperada (UserHistoryApiResponse), pero la entrada se pudo cargar directamente.";
                            }
                            catch (Exception directDeserializeEx)
                            {
                                ViewBag.ErrorMessage = $"Error de formato JSON inesperado para los detalles del historial. Detalles: {directDeserializeEx.Message}. JSON recibido: {rawJsonResponse}";
                            }
                        }
                    }
                    else
                    {
                        rawJsonResponse = await response.Content.ReadAsStringAsync();
                        ViewBag.ErrorMessage = $"Error HTTP en la API: {response.StatusCode} - {response.ReasonPhrase}. Detalles: {rawJsonResponse}";
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Ocurrió una excepción al obtener los detalles del historial: {ex.Message}";
            }

            if (historyEntry == null && string.IsNullOrEmpty(ViewBag.ErrorMessage))
            {
                ViewBag.ErrorMessage = $"Entrada de historial con ID {id} no encontrada.";
            }

            return View(historyEntry);
        }

        // GET: https://localhost:7276/api/UserHistory/user/{userId}
        public async Task<IActionResult> GetUserHistoryByUserId(int userId)
        {
            List<WebUserHistoryModels.UserHistoryViewModel> userHistoryEntries = new List<WebUserHistoryModels.UserHistoryViewModel>(); // Usar alias
            string rawJsonResponse = string.Empty; // Solución CS8600

            if (userId <= 0)
            {
                ViewBag.ErrorMessage = "ID de usuario inválido para buscar historial.";
                return View("Index", userHistoryEntries);
            }

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_apiBaseUrl);
                    HttpResponseMessage response = await client.GetAsync($"user/{userId}");
                    rawJsonResponse = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        WebUserHistoryModels.UserHistoryApiResponse? apiResponse = null; // Usar alias
                        try
                        {
                            apiResponse = JsonSerializer.Deserialize<WebUserHistoryModels.UserHistoryApiResponse>(rawJsonResponse,
                                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }); // Usar alias
                        }
                        catch (JsonException) { /* Ignorar */ }

                        if (apiResponse != null && apiResponse.IsSuccess && apiResponse.DataAsList != null)
                        {
                            userHistoryEntries = apiResponse.DataAsList;
                            ViewBag.SuccessMessage = apiResponse.Message;
                        }
                        else if (apiResponse != null && !apiResponse.IsSuccess)
                        {
                            ViewBag.ErrorMessage = apiResponse.Message ?? $"API de Historial para UserId {userId} indicó un fallo o datos nulos.";
                        }
                        else
                        {
                            try
                            {
                                userHistoryEntries = JsonSerializer.Deserialize<List<WebUserHistoryModels.UserHistoryViewModel>>(rawJsonResponse, // Usar alias
                                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                                ViewBag.WarningMessage = $"La API de Historial para UserId {userId} no devolvió la estructura esperada, pero se cargó directamente como lista.";
                            }
                            catch (Exception directDeserializeEx)
                            {
                                ViewBag.ErrorMessage = $"Error de formato JSON inesperado para el historial del UserId {userId}. Detalles: {directDeserializeEx.Message}. JSON recibido: {rawJsonResponse}";
                            }
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMessage = $"Error HTTP en la API: {response.StatusCode} - {response.ReasonPhrase}. Detalles: {rawJsonResponse}";
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Ocurrió una excepción al obtener el historial del UserId {userId}: {ex.Message}";
            }

            return View("Index", userHistoryEntries);
        }

        // GET: /UserHistory/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: https://localhost:7276/api/UserHistory/
        [HttpPost]
        [ValidateAntiForgeryToken]
        // SOLUCIÓN CS0246, CS0104: Usar el alias explícito para UserHistoryCreationDto de Application
        public async Task<IActionResult> Create(ApplicationHistoryDTOs.UserHistoryCreationDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string rawJsonResponse = string.Empty;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_apiBaseUrl);
                    string jsonContent = JsonSerializer.Serialize(model);
                    var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync("", httpContent);

                    rawJsonResponse = await response.Content.ReadAsStringAsync();
                    WebUserHistoryModels.UserHistoryApiResponse? apiResponse = JsonSerializer.Deserialize<WebUserHistoryModels.UserHistoryApiResponse>(rawJsonResponse, // Usar alias
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (apiResponse != null && apiResponse.IsSuccess)
                    {
                        TempData["SuccessMessage"] = apiResponse.Message ?? "Entrada de historial registrada exitosamente.";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ViewBag.ErrorMessage = apiResponse?.Message ?? "Error al registrar la entrada de historial.";
                        if (!response.IsSuccessStatusCode)
                        {
                            ViewBag.ErrorMessage = $"Error en la API: {response.StatusCode} - {response.ReasonPhrase}. {ViewBag.ErrorMessage} Detalles: {rawJsonResponse}";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Ocurrió una excepción durante el registro de historial: {ex.Message}";
            }

            return View(model);
        }

        // GET: /UserHistory/Delete/{id}
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                ViewBag.ErrorMessage = "ID de historial inválido para borrado.";
                return View();
            }

            var result = await Details(id) as ViewResult;
            return View(result?.Model);
        }

        // POST: https://localhost:7276/api/UserHistory/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string rawJsonResponse = string.Empty;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_apiBaseUrl);
                    HttpResponseMessage response = await client.DeleteAsync($"{id}");

                    rawJsonResponse = await response.Content.ReadAsStringAsync();

                    WebUserHistoryModels.UserHistoryApiResponse? apiResponse = JsonSerializer.Deserialize<WebUserHistoryModels.UserHistoryApiResponse>(rawJsonResponse, // Usar alias
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (apiResponse != null && apiResponse.IsSuccess)
                    {
                        TempData["SuccessMessage"] = apiResponse.Message ?? "Entrada de historial eliminada exitosamente.";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ViewBag.ErrorMessage = apiResponse?.Message ?? "Error al eliminar la entrada de historial.";
                        if (!response.IsSuccessStatusCode)
                        {
                            ViewBag.ErrorMessage = $"Error en la API: {response.StatusCode} - {response.ReasonPhrase}. {ViewBag.ErrorMessage} Detalles: {rawJsonResponse}";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Ocurrió una excepción al intentar eliminar la entrada de historial: {ex.Message}";
            }

            return RedirectToAction(nameof(Delete), new { id = id });
        }
    }
}