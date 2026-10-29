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
            return View(await _context.Arbitros.OrderBy(a => a.NombreArbitro).ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NombreArbitro")] Arbitro arbitro)
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
            var arbitro = await _context.Arbitros
                .Include(a => a.Partidas)
                .FirstOrDefaultAsync(a => a.ID == id);

            if (arbitro == null) return NotFound();

            if (arbitro.Partidas.Any())
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