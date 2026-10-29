using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TorneoEsports.Data;
using TorneoEsports.Models;

namespace TorneoEsports.Controllers
{
    public class ArbitrosController : Controller
    {
        private readonly TorneoEsportsContext _context;

        public ArbitrosController(TorneoEsportsContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Arbitros.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Apellido")] Arbitro arbitro)
        {
            if (ModelState.IsValid)
            {
                _context.Add(arbitro);
                await _context.SaveChangesAsync();
                TempData["Exito"] = "Árbitro registrado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            return View(arbitro);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Asumiendo que la propiedad de navegación N:N en el modelo se llama Partidas
            var arbitro = await _context.Arbitros
                .Include(a => a.Partidas)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (arbitro == null) return NotFound();

            if (arbitro.Partidas != null && arbitro.Partidas.Any())
            {
                TempData["Error"] = "No se puede eliminar el árbitro porque tiene partidas asignadas.";
                return RedirectToAction(nameof(Index));
            }

            _context.Arbitros.Remove(arbitro);
            await _context.SaveChangesAsync();
            TempData["Exito"] = "Árbitro eliminado correctamente.";
            return RedirectToAction(nameof(Index));
        }
    }
}