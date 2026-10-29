using Microsoft.AspNetCore.Mvc;

namespace TorneoEsports.Controllers
{

    public class ResultadosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
