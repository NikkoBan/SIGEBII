using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SIGEBI.Application.Contracts.Service;
using SIGEBI.Application.Dtos.AuthorDTO;
using SIGEBI.Web0.Models;
using System.Net.Http.Headers;
using System.Text;

namespace SIGEBI.Web0.Controllers
{
    public class AuthorController: Controller
    {
       
        // GET: AuthorControllerResponse
        public async Task<IActionResult> Index()
        {

            GetAllAuthorResponse? getAllAuthorResponse = null;

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:7276/api/");
                    var response = await client.GetAsync("Author");

                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        var options = new System.Text.Json.JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        };
                        getAllAuthorResponse = System.Text.Json.JsonSerializer.Deserialize<GetAllAuthorResponse>(responseString, options)                            
                        ?? new GetAllAuthorResponse
                            {
                                IsSucces = false,
                                message = "Error al deserializar autores",
                                data = new List<Authormodel>()
                            };
                    }
                    else
                    {
                        getAllAuthorResponse = new GetAllAuthorResponse
                        {
                            IsSucces = false,
                            message = "Error retrieving authors",
                            data = new List<Authormodel>()
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                getAllAuthorResponse = new GetAllAuthorResponse
                {
                    IsSucces = false,
                    message = ex.Message,
                    data = new List<Authormodel>()
                };
            }

            return View(getAllAuthorResponse.data);
        }
        // GET: AuthorControllerResponse/Details/5
        public async Task<IActionResult> Details(int id)
        {
            Authormodel? author = null;

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:7276/api/");
                    var response = await client.GetAsync($"/api/Author/{id}");

                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();

                        var options = new System.Text.Json.JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        };

                        var getAuthorResponse = System.Text.Json.JsonSerializer.Deserialize<GetAuthorResponse>(responseString, options);
                        author = getAuthorResponse?.data;
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "No se encontró el autor.";
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }

            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        // GET: AuthorControllerResponse/Create
        [HttpGet]
        public ActionResult Create()
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

            var dto = new CreateAuthorDTO
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                BirthDate = model.BirthDate,
                Nationality = model.Nationality
            };

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7276/api/");
                var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(dto), System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync("Author", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "Error al crear el autor.");
                    return View(model);
                }
            }
        }
        // GET: AuthorControllerResponse/Edit/5
        public async Task<IActionResult>  Edit(int id)
        {

            Authormodel? author = null;

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:7276/api/");
                    var response = await client.GetAsync($"/api/Author/{id}");

                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();

                        var options = new System.Text.Json.JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        };

                        var getAuthorResponse = System.Text.Json.JsonSerializer.Deserialize<GetAuthorResponse>(responseString, options);
                        author = getAuthorResponse?.data;
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "No se encontró el autor.";
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }

            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }


        // POST: AuthorControllerResponse/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Authormodel model)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:7276/");

                    // Enviar la solicitud PUT con el id correcto
                    var response = await client.PutAsJsonAsync($"api/Author/{id}", model);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();

                        // Opciones para ignorar mayúsculas y minúsculas
                        var options = new System.Text.Json.JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        };

                        var authorEditResponse = System.Text.Json.JsonSerializer.Deserialize<AuthorEditResponse>(responseString, options);

                        if (authorEditResponse != null && authorEditResponse.isSucces)
                        {
                            TempData["SuccessMessage"] = "Author updated successfully.";
                        }
                        else
                        {
                            TempData["ErrorMessage"] = authorEditResponse?.message ?? "Something went wrong.";
                        }
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Failed to update the author.";
                    }

                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
                return View(model); // Así puedes devolver el modelo con errores si los hay
            }
        }

        // GET: AuthorControllerResponse/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

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
