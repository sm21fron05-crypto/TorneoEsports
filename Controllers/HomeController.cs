using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TorneoEsports.Data;
using TorneoEsports.ViewModels;

namespace TorneoEsports.Controllers
{
    public class HomeController : Controller
    {
        private readonly TorneoEsportsContext _context;

        public HomeController(TorneoEsportsContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var modelo = new InicioViewModel
            {
                TotalEquipos = await _context.Equipos.CountAsync(),
                TotalJugadores = await _context.Jugadores.CountAsync()
            };

            return View(modelo);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var modelo = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };

            return View(modelo);
        }
    }
}
