using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SIGEBI.Web.Models;

namespace SIGEBI.Web.Controllers
{
    public class AuhtorController : Controller
    {
        // GET: AuhtorController
        public async Task<IActionResult> Index()
        {

            GetAllAuthorResponse getAllAuthorResponse = null;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:7276/api/");
                    var response = await client.GetAsync("Author/GetAuthor");

                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        getAllAuthorResponse = System.Text.Json.JsonSerializer.Deserialize<GetAllAuthorResponse>(responseString);
                    }
                    else
                    {
                        getAllAuthorResponse = new GetAllAuthorResponse
                        {
                            isSucces = false,
                            message = "Error retrieving authors",
                            data = null
                        };
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
                //getAllAuthorResponse = new GetAllAuthorResponse
                //{
                //    isSucces = false,
                //    message = ex.Message,
                //    data = null
                //};
            }
            return View();
        }

        // GET: AuhtorController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AuhtorController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AuhtorController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: AuhtorController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AuhtorController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
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

        // GET: AuhtorController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AuhtorController/Delete/5
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
