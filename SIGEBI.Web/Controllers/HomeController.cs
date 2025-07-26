// SIGEBI.Web/Controllers/HomeController.cs

using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using SIGEBI.Web.Models;
using Microsoft.Extensions.Logging;

namespace SIGEBI.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("La p�gina de inicio est�ndar ha sido visitada.");
            return View();
        }

        public IActionResult Privacy()
        {
            _logger.LogWarning("La p�gina de privacidad est�ndar ha sido visitada.");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            _logger.LogError("Se ha producido un error est�ndar. RequestId: {RequestId}", Activity.Current?.Id ?? HttpContext.TraceIdentifier);
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}