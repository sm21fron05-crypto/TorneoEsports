using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TorneoEsports.Data;
using TorneoEsports.ViewModels;

namespace TorneoEsports.Controllers
{
    // =========================================================================================
    // CONTROLADOR: HomeController
    // Responsabilidad: Pantalla de inicio (Dashboard con contadores generales) y página de error.
    // =========================================================================================
    public class HomeController : Controller
    {
        // Contexto de base de datos inyectado mediante Inyección de Dependencias
        private readonly TorneoEsportsContext _context;

        // El constructor recibe la instancia del DbContext configurada en Program.cs
        public HomeController(TorneoEsportsContext context)
        {
            _context = context;
        }

        // GET: / o /Home/Index
        // Método asíncrono (Task<IActionResult>) para no bloquear el hilo de ejecución durante consultas a BD.
        public async Task<IActionResult> Index()
        {
            // Se consulta de forma asíncrona la cantidad total de equipos y jugadores registrados.
            // Se empaqueta en un ViewModel (InicioViewModel) para mantener la vista tipada fuertemente.
            var modelo = new InicioViewModel
            {
                TotalEquipos = await _context.Equipos.CountAsync(),
                TotalJugadores = await _context.Jugadores.CountAsync()
            };

            // Retorna la vista Views/Home/Index.cshtml pasando el modelo de datos.
            return View(modelo);
        }

        // GET: /Home/Error
        // Manejador centralizado de errores cuando ocurre una excepción en el pipeline.
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
