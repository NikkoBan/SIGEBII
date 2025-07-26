
using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.Dtos.BooksDtos;
using SIGEBI.Web0.Models;
using System.Text.Json;

namespace SIGEBI.Web0.Controllers
{
    public class BookController : Controller

    {
        private readonly string _apiBase = "https://localhost:7276/api/";
        // GET: BookControllerReponse
        public async Task<IActionResult> Index()
        {
            GetAllBookResponse? getAllBookResponse = null;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:7276/api/");
                    var response = await client.GetAsync("Book");

                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        getAllBookResponse = System.Text.Json.JsonSerializer.Deserialize<GetAllBookResponse>(responseString);
                    }
                    else
                    {
                        getAllBookResponse = new GetAllBookResponse
                        {
                            IsSuccess = false,
                            message = "Error retrieving books",
                            data = new List<BookModel>()
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                getAllBookResponse = new GetAllBookResponse
                {
                    IsSuccess = false,
                    message = ex.Message,
                    data = new List<BookModel>()
                };
            }

            return View(getAllBookResponse?.data ?? new List<BookModel>());
        }

        // GET: BookControllerReponse/Details/5
        public async Task<IActionResult> Details(int id)
        {
            BookModel? book = null;

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:7276/api/");
                    var response = await client.GetAsync($"/api/Book/{id}");

                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();

                        var getBookResponse = System.Text.Json.JsonSerializer.Deserialize<GetBookResponse>(responseString);
                        book = getBookResponse?.data;
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "No se encontró el libro.";
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: BookControllerReponse/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BookControllerReponse/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateBookModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);  
            }

            try
            {
                using var client = new HttpClient();
                client.BaseAddress = new Uri("https://localhost:7276/api/");

                var response = await client.PostAsJsonAsync("Book", model);  

                var responseString = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<AuthorCreateResponse>(responseString);

               
                if (response.IsSuccessStatusCode && apiResponse?.IsSucces == true)
                {
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.Message = apiResponse?.message ?? "Error al crear el libro.";
            }
            catch
            {
                ViewBag.Message = "Error al crear el libro.";
            }

            return View(model);  
        }

        // GET: BookControllerReponse/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            BookModel? book = null;

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:7276/api/");
                    var response = await client.GetAsync($"Book/{id}");

                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        var getBookResponse = System.Text.Json.JsonSerializer.Deserialize<GetBookResponse>(responseString);
                        book = getBookResponse?.data;
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "No se encontró el libro.";
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }


        // POST: BookControllerReponse/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BookModel model)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:7276/");

                    var response = await client.PutAsJsonAsync($"api/Book/{id}", model);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        var bookEditResponse = System.Text.Json.JsonSerializer.Deserialize<AuthorEditResponse>(responseString);

                        if (bookEditResponse != null && bookEditResponse.isSucces)
                        {
                            TempData["SuccessMessage"] = "Book updated successfully.";
                        }
                        else
                        {
                            TempData["ErrorMessage"] = bookEditResponse?.message ?? "Something went wrong.";
                        }
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Failed to update the book.";
                    }

                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
                return View(model);
            }
        }

        // GET: BookControllerReponse/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: BookControllerReponse/Delete/5
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
