using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SIGEBI.WebApplication.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Http;
using System.Threading.Tasks;


namespace SIGEBI.WebApplication.Controllers
{
    public class ReservationController : Controller
    {
        private readonly HttpClient _httpClient;

        public ReservationController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // GET: ReservationController
        //aqui se muestra la lista de reservas. Como el getall no funciona, queda pendiente de implementar
        public async Task<IActionResult> Index()
        {
            //aqui debo llamar al endpoint de Get "api/Reservation"
            //y mostrar la lista de reservas en la vista
            return View();
        }               

        // GET: ReservationController/Details/5
        //getbyid
        public async Task<IActionResult> Details(int id)
        { 
            
            try
            {
                //llamo al endpoint de GetById "api/Reservation/{id}"
                var response = await _httpClient.GetFromJsonAsync<ReservationResponse<ReservationModel>>($"Reservation/1005{id}");

                if (response != null && response.isSuccess)
                {
                    //var responseContent = response.data;
                    return View(response.data);
                }

                ModelState.AddModelError(
                    string.Empty, "No se pudo obtener la reserva.");

            }
            catch (Exception ex)
            {               
                ModelState.AddModelError(
                    string.Empty, $"Error al obtener los detalles de la reserva: {ex.Message}");
            }

            return View();
        }

        // GET: ReservationController/Create
        //aqui se muestra el formulario para crear una nueva reserva, no llama a ningun endpoint
        public ActionResult Create()
        {
            return View();
        }

        // POST: ReservationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateReservationModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            ReservationResponse createReservationResponse = null;
            try
            {

                var response = await _httpClient.PostAsJsonAsync("Reservation/", model);


                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

            }
            catch (Exception ex)
            {
                createReservationResponse = new ReservationResponse
                {
                    isSuccess = false,
                    message = "Error al crear la reserva: " + ex.Message,
                    data = null
                };
            }
            return View(model);
        }

        // GET: ReservationController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ReservationController/Edit/5
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

        // GET: ReservationController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            return View();
        }
    }
}
