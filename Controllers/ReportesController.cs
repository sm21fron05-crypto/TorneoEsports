using Microsoft.AspNetCore.Mvc;

namespace TorneoEsports.Controllers
{
    public class ReportesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
