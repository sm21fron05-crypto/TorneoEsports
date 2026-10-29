using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TorneoEsports.Data;
using TorneoEsports.Models;
using TorneoEsports.ViewModels;

namespace TorneoEsports.Controllers
{
    public class JugadoresController : Controller
    {
        private readonly TorneoEsportsContext _context;

        public JugadoresController(TorneoEsportsContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int? equipoId, string? buscar)
        {
            var consulta = _context.Jugadores
                .Include(j => j.Equipo)
                .AsQueryable();
            if (equipoId.HasValue)
            {
                consulta = consulta.Where(j => j.EquipoID == equipoId.Value);
            }
            if (!string.IsNullOrWhiteSpace(buscar))
            {
                consulta = consulta.Where(j => j.Nickname.Contains(buscar) || j.NombreReal.Contains(buscar));
            }
            var modelo = new JugadorListaViewModel
            {
                Jugadores = await consulta.OrderBy(j => j.Nickname).ToListAsync(),
                EquipoID = equipoId,
                Buscar = buscar,
                Equipos = await ObtenerEquiposAsync(equipoId)
            };

            return View(modelo);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var jugador = await _context.Jugadores
                .Include(j => j.Equipo)
                .FirstOrDefaultAsync(j => j.ID == id);

            if (jugador == null) return NotFound();

            return View(jugador);
        }
        public async Task<IActionResult> Create()
        {
            var modelo = new JugadorFormViewModel
            {
                Equipos = await ObtenerEquiposAsync()
            };

            return View(modelo);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(JugadorFormViewModel modelo)
        {
            await ValidarReglasAsync(modelo);
            if (!ModelState.IsValid)
            {
                modelo.Equipos = await ObtenerEquiposAsync(modelo.EquipoID);
                return View(modelo);
            }
            var jugador = new Jugador
            {
                Nickname = modelo.Nickname.Trim(),
                NombreReal = modelo.NombreReal.Trim(),
                EquipoID = modelo.EquipoID
            };
            _context.Add(jugador);
            await _context.SaveChangesAsync();
            TempData["Exito"] = $"El jugador \"{jugador.Nickname}\" se registró correctamente.";
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var jugador = await _context.Jugadores.FindAsync(id);
            if (jugador == null) return NotFound();

            var modelo = new JugadorFormViewModel
            {
                ID = jugador.ID,
                Nickname = jugador.Nickname,
                NombreReal = jugador.NombreReal,
                EquipoID = jugador.EquipoID,
                Equipos = await ObtenerEquiposAsync(jugador.EquipoID)
            };

            return View(modelo);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, JugadorFormViewModel modelo)
        {
            if (id != modelo.ID) return NotFound();

            await ValidarReglasAsync(modelo);
            if (!ModelState.IsValid)
            {
                modelo.Equipos = await ObtenerEquiposAsync(modelo.EquipoID);
                return View(modelo);
            }

            var jugador = await _context.Jugadores.FindAsync(id);
            if (jugador == null) return NotFound();
            jugador.Nickname = modelo.Nickname.Trim();
            jugador.NombreReal = modelo.NombreReal.Trim();
            jugador.EquipoID = modelo.EquipoID;

            await _context.SaveChangesAsync();

            TempData["Exito"] = $"El jugador \"{jugador.Nickname}\" se actualizó correctamente.";
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var jugador = await _context.Jugadores
                .Include(j => j.Equipo)
                .FirstOrDefaultAsync(j => j.ID == id);

            if (jugador == null) return NotFound();

            return View(jugador);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jugador = await _context.Jugadores.FindAsync(id);
            if (jugador == null) return NotFound();

            _context.Jugadores.Remove(jugador);
            await _context.SaveChangesAsync();

            TempData["Exito"] = $"El jugador \"{jugador.Nickname}\" se eliminó correctamente.";
            return RedirectToAction(nameof(Index));
        }
        private async Task ValidarReglasAsync(JugadorFormViewModel modelo)
        {
            if (modelo.EquipoID > 0 && !await _context.Equipos.AnyAsync(e => e.ID == modelo.EquipoID))
            {
                ModelState.AddModelError(nameof(modelo.EquipoID), "El equipo seleccionado no existe.");
            }
            var nickname = modelo.Nickname?.Trim();
            if (!string.IsNullOrEmpty(nickname) &&
                await _context.Jugadores.AnyAsync(j => j.ID != modelo.ID && j.Nickname == nickname))
            {
                ModelState.AddModelError(nameof(modelo.Nickname), "Ya existe un jugador con ese nickname.");
            }
        }
        private async Task<IEnumerable<SelectListItem>> ObtenerEquiposAsync(int? seleccionado = null)
        {
            var equipos = await _context.Equipos
                .OrderBy(e => e.NombreEquipo)
                .ToListAsync();

            return equipos.Select(e => new SelectListItem
            {
                Value = e.ID.ToString(),
                Text = e.NombreEquipo,
                Selected = e.ID == seleccionado
            });
        }
    }
}
