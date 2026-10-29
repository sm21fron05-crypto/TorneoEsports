using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TorneoEsports.Data;
using TorneoEsports.Models;

namespace TorneoEsports.Controllers
{
    public class SedesController : Controller
    {
        private readonly TorneoEsportsContext _context;

        public SedesController(TorneoEsportsContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Sedes.OrderBy(s => s.NombreSede).ToListAsync());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NombreSede,Ubicacion")] Sede sede)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sede);
                await _context.SaveChangesAsync();
                TempData["Exito"] = "Sede creada correctamente.";
                return RedirectToAction(nameof(Index));
            }
            return View(sede);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sede = await _context.Sedes
                .Include(s => s.Partidas)
                .FirstOrDefaultAsync(s => s.ID == id);

            if (sede == null) return NotFound();

            if (sede.Partidas.Any())
            {
                TempData["Error"] = "No se puede eliminar la sede porque tiene partidas asignadas.";
                return RedirectToAction(nameof(Index));
            }

            _context.Sedes.Remove(sede);
            await _context.SaveChangesAsync();
            TempData["Exito"] = "Sede eliminada correctamente.";
            return RedirectToAction(nameof(Index));
        }
    }
}