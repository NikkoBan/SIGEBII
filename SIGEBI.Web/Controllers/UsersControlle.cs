// SIGEBI.Web/Controllers/UsersController.cs

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration; // Para IConfiguration

// Modelos/DTOs del frontend ESPECÍFICOS DE LA WEB
using WebUsersModels = SIGEBI.Web.Models.Users; // Alias para los modelos de usuario de la Web
// Notar que LoginRequest se toma de WebUsersModels directamente

// DTOs para comunicación con la API (vienen de SIGEBI.Application)
using ApplicationUserDTOs = SIGEBI.Application.DTOsAplication.UserDTOs; // Alias para DTOs de Usuario de Application
// No necesitamos 'using SIGEBI.Application.DTOsAplication.AuthDTOs;' aquí, ya que LoginRequest se toma de WebUsersModels.

namespace SIGEBI.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly string _apiBaseUrl;

        public UsersController(IConfiguration configuration)
        {
            _apiBaseUrl = configuration.GetValue<string>("ApiSettings:BaseUrl") + "Users";
        }

        // GET: /Users/
        public async Task<IActionResult> Index()
        {
            List<WebUsersModels.UserViewModel> users = new List<WebUsersModels.UserViewModel>(); // Usar alias
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
                        WebUsersModels.UserApiResponse? apiResponse = null; // Usar alias
                        try
                        {
                            apiResponse = JsonSerializer.Deserialize<WebUsersModels.UserApiResponse>(rawJsonResponse,
                                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                        }
                        catch (JsonException) { /* Ignorar */ }

                        if (apiResponse != null && apiResponse.IsSuccess && apiResponse.DataAsList != null)
                        {
                            users = apiResponse.DataAsList;
                            ViewBag.SuccessMessage = apiResponse.Message;
                        }
                        else if (apiResponse != null && !apiResponse.IsSuccess)
                        {
                            ViewBag.ErrorMessage = apiResponse.Message ?? "API de Usuarios indicó un fallo o datos nulos.";
                        }
                        else
                        {
                            try
                            {
                                users = JsonSerializer.Deserialize<List<WebUsersModels.UserViewModel>>(rawJsonResponse,
                                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }); // Usar alias
                                ViewBag.WarningMessage = "La API de Usuarios no devolvió la estructura esperada (UserApiResponse), pero los datos se pudieron cargar directamente como lista.";
                            }
                            catch (Exception directDeserializeEx)
                            {
                                ViewBag.ErrorMessage = $"Error de formato JSON inesperado para el listado de usuarios. Detalles: {directDeserializeEx.Message}. JSON recibido: {rawJsonResponse}";
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
                ViewBag.ErrorMessage = $"Ocurrió una excepción al intentar cargar los usuarios: {ex.Message}";
            }

            return View(users);
        }

        // GET: /Users/Details/{id}
        public async Task<IActionResult> Details(int id)
        {
            WebUsersModels.UserViewModel? user = null; // Usar alias
            string rawJsonResponse = string.Empty; // Solución CS8600

            if (id <= 0)
            {
                ViewBag.ErrorMessage = "ID de usuario inválido para la búsqueda de detalles.";
                return View(user);
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
                        WebUsersModels.UserApiResponse? apiResponse = null; // Usar alias
                        try
                        {
                            apiResponse = JsonSerializer.Deserialize<WebUsersModels.UserApiResponse>(rawJsonResponse,
                                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }); // Usar alias
                        }
                        catch (JsonException) { /* Ignorar */ }

                        if (apiResponse != null && apiResponse.IsSuccess && apiResponse.DataAsSingleObject != null)
                        {
                            user = apiResponse.DataAsSingleObject;
                            ViewBag.SuccessMessage = apiResponse.Message;
                        }
                        else if (apiResponse != null && !apiResponse.IsSuccess)
                        {
                            ViewBag.ErrorMessage = apiResponse.Message ?? "API de Detalles de Usuarios indicó un fallo.";
                        }
                        else
                        {
                            try // Intentar directamente a UserViewModel
                            {
                                user = JsonSerializer.Deserialize<WebUsersModels.UserViewModel>(rawJsonResponse, // Usar alias
                                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                                ViewBag.WarningMessage = "La API de Detalles de Usuarios no devolvió la estructura esperada (UserApiResponse), pero el usuario se pudo cargar directamente.";
                            }
                            catch (Exception directDeserializeEx)
                            {
                                ViewBag.ErrorMessage = $"Error de formato JSON inesperado para los detalles del usuario. Detalles: {directDeserializeEx.Message}. JSON recibido: {rawJsonResponse}";
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
                ViewBag.ErrorMessage = $"Ocurrió una excepción al obtener los detalles del usuario: {ex.Message}";
            }

            if (user == null && string.IsNullOrEmpty(ViewBag.ErrorMessage))
            {
                ViewBag.ErrorMessage = $"Usuario con ID {id} no encontrado.";
            }

            return View(user);
        }

        // GET: /Users/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: https://localhost:7276/api/Users/login
        [HttpPost]
        [ValidateAntiForgeryToken]
        // SOLUCIÓN CS0104: Usar el alias explícito para LoginRequest de la Web
        public async Task<IActionResult> Login(WebUsersModels.LoginRequest model)
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

                    HttpResponseMessage response = await client.PostAsync("login", httpContent);

                    rawJsonResponse = await response.Content.ReadAsStringAsync();
                    WebUsersModels.UserApiResponse? apiResponse = JsonSerializer.Deserialize<WebUsersModels.UserApiResponse>(rawJsonResponse, // Usar alias
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (apiResponse != null && apiResponse.IsSuccess)
                    {
                        TempData["SuccessMessage"] = apiResponse.Message ?? "¡Login exitoso!";
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = apiResponse?.Message ?? "Credenciales inválidas o error desconocido.";
                        if (!response.IsSuccessStatusCode)
                        {
                            ViewBag.ErrorMessage = $"Error en la API: {response.StatusCode} - {response.ReasonPhrase}. {ViewBag.ErrorMessage} Detalles: {rawJsonResponse}";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Ocurrió una excepción durante el login: {ex.Message}";
            }

            return View(model);
        }

        // GET: /Users/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: https://localhost:7276/api/Users/register
        [HttpPost]
        [ValidateAntiForgeryToken]
        // SOLUCIÓN CS0246, CS0104, etc: Usar el alias explícito para UserCreationDto de Application
        public async Task<IActionResult> Register(ApplicationUserDTOs.UserCreationDto model)
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

                    HttpResponseMessage response = await client.PostAsync("register", httpContent);

                    rawJsonResponse = await response.Content.ReadAsStringAsync();
                    WebUsersModels.UserApiResponse? apiResponse = JsonSerializer.Deserialize<WebUsersModels.UserApiResponse>(rawJsonResponse, // Usar alias
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (apiResponse != null && apiResponse.IsSuccess)
                    {
                        TempData["SuccessMessage"] = apiResponse.Message ?? "Usuario registrado exitosamente. Ya puede iniciar sesión.";
                        return RedirectToAction(nameof(Login));
                    }
                    else
                    {
                        ViewBag.ErrorMessage = apiResponse?.Message ?? "Error al registrar el usuario.";
                        if (!response.IsSuccessStatusCode)
                        {
                            ViewBag.ErrorMessage = $"Error en la API: {response.StatusCode} - {response.ReasonPhrase}. {ViewBag.ErrorMessage} Detalles: {rawJsonResponse}";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Ocurrió una excepción durante el registro: {ex.Message}";
            }

            return View(model);
        }

        // GET: /Users/Edit/{id}
        public async Task<IActionResult> Edit(int id)
        {
            if (id <= 0)
            {
                ViewBag.ErrorMessage = "ID de usuario inválido para edición.";
                return View();
            }

            string rawJsonResponse = string.Empty;
            ApplicationUserDTOs.UserUpdateDto? userToEdit = null; // Usar alias para UserUpdateDto

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_apiBaseUrl);
                    HttpResponseMessage response = await client.GetAsync($"{id}");

                    if (response.IsSuccessStatusCode)
                    {
                        rawJsonResponse = await response.Content.ReadAsStringAsync();
                        try
                        {
                            WebUsersModels.UserApiResponse? apiResponse = JsonSerializer.Deserialize<WebUsersModels.UserApiResponse>(rawJsonResponse, // Usar alias
                                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                            if (apiResponse != null && apiResponse.IsSuccess && apiResponse.DataAsSingleObject != null)
                            {
                                var userViewModel = apiResponse.DataAsSingleObject;
                                userToEdit = new ApplicationUserDTOs.UserUpdateDto // Usar alias aquí al instanciar
                                {
                                    UserId = userViewModel.UserId,
                                    FullName = userViewModel.FullName,
                                    InstitutionalEmail = userViewModel.InstitutionalEmail,
                                    InstitutionalIdentifier = userViewModel.InstitutionalIdentifier,
                                    RoleId = userViewModel.RoleId,
                                    IsActive = userViewModel.IsActive
                                };
                            }
                            else
                            {
                                ViewBag.ErrorMessage = apiResponse?.Message ?? "No se encontró el usuario en la estructura esperada para edición.";
                            }
                        }
                        catch (JsonException)
                        {
                            // Intenta deserializar directamente si no viene con la estructura de ApiResponse
                            userToEdit = JsonSerializer.Deserialize<ApplicationUserDTOs.UserUpdateDto>(rawJsonResponse, // Usar alias
                                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                            if (userToEdit != null) ViewBag.WarningMessage = "La API de Edit de Usuarios no devolvió la estructura esperada (UserApiResponse).";
                        }

                        // Solución CS0019: Comparar 'userToEdit' con null de forma segura.
                        if (userToEdit == null)
                        { // Operador '==' para tipos anulables es seguro aquí.
                            ViewBag.ErrorMessage = ViewBag.ErrorMessage ?? "Usuario no encontrado para edición.";
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
                ViewBag.ErrorMessage = $"Ocurrió una excepción al cargar el usuario para edición: {ex.Message}";
            }

            return View(userToEdit);
        }

        // POST: https://localhost:7276/api/Users/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        // SOLUCIÓN CS0246, CS0104, CS0019: Usar el alias explícito para UserUpdateDto de Application
        public async Task<IActionResult> Edit(int id, ApplicationUserDTOs.UserUpdateDto model)
        {
            // Solución CS0019: Comparar 'model' con null de forma segura.
            if (model == null || id != model.UserId) // El compilador es inteligente con 'is null' o '!= null'
            {
                ViewBag.ErrorMessage = "ID de usuario no coincide o modelo nulo.";
                return View(model);
            }

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

                    HttpResponseMessage response = await client.PutAsync($"{id}", httpContent);

                    rawJsonResponse = await response.Content.ReadAsStringAsync();

                    WebUsersModels.UserApiResponse? apiResponse = JsonSerializer.Deserialize<WebUsersModels.UserApiResponse>(rawJsonResponse, // Usar alias
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (apiResponse != null && apiResponse.IsSuccess)
                    {
                        TempData["SuccessMessage"] = apiResponse.Message ?? "Usuario actualizado exitosamente.";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ViewBag.ErrorMessage = apiResponse?.Message ?? "Error al actualizar el usuario.";
                        if (!response.IsSuccessStatusCode)
                        {
                            ViewBag.ErrorMessage = $"Error en la API: {response.StatusCode} - {response.ReasonPhrase}. {ViewBag.ErrorMessage} Detalles: {rawJsonResponse}";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Ocurrió una excepción al actualizar el usuario: {ex.Message}";
            }

            return View(model);
        }

        // GET: /Users/Delete/{id}
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                ViewBag.ErrorMessage = "ID de usuario inválido para borrado.";
                return View();
            }

            var result = await Details(id) as ViewResult;
            if (result?.Model is WebUsersModels.UserViewModel userModel) // Usar alias
            {
                return View(userModel);
            }
            ViewBag.ErrorMessage = "Usuario no encontrado para confirmar borrado.";
            return View();
        }

        // POST: https://localhost:7276/api/Users/{id}
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

                    WebUsersModels.UserApiResponse? apiResponse = JsonSerializer.Deserialize<WebUsersModels.UserApiResponse>(rawJsonResponse, // Usar alias
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (apiResponse != null && apiResponse.IsSuccess)
                    {
                        TempData["SuccessMessage"] = apiResponse.Message ?? "Usuario eliminado (lógicamente) exitosamente.";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ViewBag.ErrorMessage = apiResponse?.Message ?? "Error al eliminar el usuario.";
                        if (!response.IsSuccessStatusCode)
                        {
                            ViewBag.ErrorMessage = $"Error en la API: {response.StatusCode} - {response.ReasonPhrase}. {ViewBag.ErrorMessage} Detalles: {rawJsonResponse}";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Ocurrió una excepción al intentar eliminar el usuario: {ex.Message}";
            }

            return RedirectToAction(nameof(Delete), new { id = id });
        }
    }
}