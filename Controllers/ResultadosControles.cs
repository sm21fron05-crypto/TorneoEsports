using Microsoft.AspNetCore.Mvc;

namespace TorneoEsports.Controllers
{
    public class ResultadosControles : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
