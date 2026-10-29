using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TorneoEsports.Data;
using TorneoEsports.Models;

namespace TorneoEsports.Controllers
{
    public class EquiposController : Controller
    {
        private readonly TorneoEsportsContext _context;

        public EquiposController(TorneoEsportsContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(string? buscar)
        {
            var consulta = _context.Equipos
                .Include(e => e.Jugadores)
                .AsQueryable();
            if (!string.IsNullOrWhiteSpace(buscar))
            {
                consulta = consulta.Where(e => e.NombreEquipo.Contains(buscar) || e.Pais.Contains(buscar));
            }
            ViewData["Buscar"] = buscar;
            return View(await consulta.OrderBy(e => e.NombreEquipo).ToListAsync());
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var equipo = await _context.Equipos
                .Include(e => e.Jugadores)
                .Include(e => e.Torneos)
                .FirstOrDefaultAsync(e => e.ID == id);

            if (equipo == null) return NotFound();

            return View(equipo);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NombreEquipo,Pais")] Equipo equipo)
        {
            await ValidarNombreUnicoAsync(equipo);
            if (!ModelState.IsValid) return View(equipo);
            _context.Add(equipo);
            await _context.SaveChangesAsync();
            TempData["Exito"] = $"El equipo \"{equipo.NombreEquipo}\" se registró correctamente.";
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var equipo = await _context.Equipos.FindAsync(id);
            if (equipo == null) return NotFound();

            return View(equipo);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,NombreEquipo,Pais")] Equipo equipo)
        {
            if (id != equipo.ID) return NotFound();

            await ValidarNombreUnicoAsync(equipo);

            if (!ModelState.IsValid) return View(equipo);

            try
            {
                _context.Update(equipo);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Equipos.AnyAsync(e => e.ID == id)) return NotFound();
                throw;
            }

            TempData["Exito"] = $"El equipo \"{equipo.NombreEquipo}\" se actualizó correctamente.";
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var equipo = await _context.Equipos
                .Include(e => e.Jugadores)
                .FirstOrDefaultAsync(e => e.ID == id);

            if (equipo == null) return NotFound();

            return View(equipo);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var equipo = await _context.Equipos
                .Include(e => e.Jugadores)
                .Include(e => e.Enfrentamientos)
                .FirstOrDefaultAsync(e => e.ID == id);

            if (equipo == null) return NotFound();
            if (equipo.Jugadores.Count > 0)
            {
                TempData["Error"] = $"No se puede eliminar \"{equipo.NombreEquipo}\" porque tiene {equipo.Jugadores.Count} jugador(es) registrado(s). Elimina o reasigna los jugadores primero.";
                return RedirectToAction(nameof(Index));
            }
            if (equipo.Enfrentamientos.Count > 0)
            {
                TempData["Error"] = $"No se puede eliminar \"{equipo.NombreEquipo}\" porque ya participa en partidas.";
                return RedirectToAction(nameof(Index));
            }

            _context.Equipos.Remove(equipo);
            await _context.SaveChangesAsync();

            TempData["Exito"] = $"El equipo \"{equipo.NombreEquipo}\" se eliminó correctamente.";
            return RedirectToAction(nameof(Index));
        }
        private async Task ValidarNombreUnicoAsync(Equipo equipo)
        {
            var existe = await _context.Equipos.AnyAsync(e =>
                e.ID != equipo.ID && e.NombreEquipo == equipo.NombreEquipo);

            if (existe)
            {
                ModelState.AddModelError(nameof(Equipo.NombreEquipo), "Ya existe un equipo con ese nombre.");
            }
        }
    }
}
