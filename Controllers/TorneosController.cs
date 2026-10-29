using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TorneoEsports.Data;
using TorneoEsports.Models;
using TorneoEsports.ViewModels;

namespace TorneoEsports.Controllers
{
    public class TorneosController : Controller
    {
        private readonly TorneoEsportsContext _context;

        public TorneosController(TorneoEsportsContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(string? buscar)
        {
            var consulta = _context.Torneos
                .Include(t => t.Equipos)
                .Include(t => t.Partidas)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(buscar))
            {
                consulta = consulta.Where(t => t.NombreTorneo.Contains(buscar));
            }

            ViewData["Buscar"] = buscar;
            return View(await consulta.OrderByDescending(t => t.FechaInicio).ToListAsync());
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var torneo = await _context.Torneos
                .Include(t => t.Equipos)
                .Include(t => t.Partidas)
                    .ThenInclude(p => p.Enfrentamientos)
                        .ThenInclude(en => en.Equipo)
                .FirstOrDefaultAsync(t => t.ID == id);

            if (torneo == null) return NotFound();

            return View(torneo);
        }
        public async Task<IActionResult> Create()
        {
            var modelo = new TorneoFormViewModel
            {
                Equipos = await ObtenerEquiposAsync()
            };

            return View(modelo);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TorneoFormViewModel modelo)
        {
            await ValidarReglasAsync(modelo);

            if (!ModelState.IsValid)
            {
                modelo.Equipos = await ObtenerEquiposAsync(modelo.EquiposSeleccionados);
                return View(modelo);
            }

            var torneo = new Torneo
            {
                NombreTorneo = modelo.NombreTorneo.Trim(),
                FechaInicio = modelo.FechaInicio!.Value,
                FechaFin = modelo.FechaFin!.Value
            };

            var equipos = await _context.Equipos
                .Where(e => modelo.EquiposSeleccionados.Contains(e.ID))
                .ToListAsync();
            torneo.Equipos.AddRange(equipos);

            _context.Add(torneo);
            await _context.SaveChangesAsync();

            TempData["Exito"] = $"El torneo \"{torneo.NombreTorneo}\" se registró correctamente.";
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var torneo = await _context.Torneos
                .Include(t => t.Equipos)
                .FirstOrDefaultAsync(t => t.ID == id);
            if (torneo == null) return NotFound();

            var seleccionados = torneo.Equipos.Select(e => e.ID).ToList();

            var modelo = new TorneoFormViewModel
            {
                ID = torneo.ID,
                NombreTorneo = torneo.NombreTorneo,
                FechaInicio = torneo.FechaInicio,
                FechaFin = torneo.FechaFin,
                EquiposSeleccionados = seleccionados,
                Equipos = await ObtenerEquiposAsync(seleccionados)
            };

            return View(modelo);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TorneoFormViewModel modelo)
        {
            if (id != modelo.ID) return NotFound();

            await ValidarReglasAsync(modelo);

            if (!ModelState.IsValid)
            {
                modelo.Equipos = await ObtenerEquiposAsync(modelo.EquiposSeleccionados);
                return View(modelo);
            }

            var torneo = await _context.Torneos
                .Include(t => t.Equipos)
                .FirstOrDefaultAsync(t => t.ID == id);
            if (torneo == null) return NotFound();

            torneo.NombreTorneo = modelo.NombreTorneo.Trim();
            torneo.FechaInicio = modelo.FechaInicio!.Value;
            torneo.FechaFin = modelo.FechaFin!.Value;
            torneo.Equipos.RemoveAll(e => !modelo.EquiposSeleccionados.Contains(e.ID));
            var actuales = torneo.Equipos.Select(e => e.ID).ToList();
            var nuevos = await _context.Equipos
                .Where(e => modelo.EquiposSeleccionados.Contains(e.ID) && !actuales.Contains(e.ID))
                .ToListAsync();
            torneo.Equipos.AddRange(nuevos);

            await _context.SaveChangesAsync();

            TempData["Exito"] = $"El torneo \"{torneo.NombreTorneo}\" se actualizó correctamente.";
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var torneo = await _context.Torneos
                .Include(t => t.Partidas)
                .Include(t => t.Equipos)
                .FirstOrDefaultAsync(t => t.ID == id);

            if (torneo == null) return NotFound();

            return View(torneo);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var torneo = await _context.Torneos
                .Include(t => t.Partidas)
                .FirstOrDefaultAsync(t => t.ID == id);

            if (torneo == null) return NotFound();
            if (torneo.Partidas.Count > 0)
            {
                TempData["Error"] = $"No se puede eliminar \"{torneo.NombreTorneo}\" porque tiene {torneo.Partidas.Count} partida(s) registrada(s).";
                return RedirectToAction(nameof(Index));
            }

            _context.Torneos.Remove(torneo);
            await _context.SaveChangesAsync();

            TempData["Exito"] = $"El torneo \"{torneo.NombreTorneo}\" se eliminó correctamente.";
            return RedirectToAction(nameof(Index));
        }
        private async Task ValidarReglasAsync(TorneoFormViewModel modelo)
        {
            if (modelo.FechaInicio.HasValue && modelo.FechaFin.HasValue &&
                modelo.FechaFin.Value < modelo.FechaInicio.Value)
            {
                ModelState.AddModelError(nameof(modelo.FechaFin), "La fecha de fin no puede ser anterior a la fecha de inicio.");
            }

            var nombre = modelo.NombreTorneo?.Trim();
            if (!string.IsNullOrEmpty(nombre) &&
                await _context.Torneos.AnyAsync(t => t.ID != modelo.ID && t.NombreTorneo == nombre))
            {
                ModelState.AddModelError(nameof(modelo.NombreTorneo), "Ya existe un torneo con ese nombre.");
            }

            if (modelo.EquiposSeleccionados.Count > 0)
            {
                var ids = modelo.EquiposSeleccionados.Distinct().ToList();
                var existentes = await _context.Equipos.CountAsync(e => ids.Contains(e.ID));
                if (existentes != ids.Count)
                {
                    ModelState.AddModelError(nameof(modelo.EquiposSeleccionados), "Alguno de los equipos seleccionados no existe.");
                }
            }
        }

        private async Task<IEnumerable<SelectListItem>> ObtenerEquiposAsync(IEnumerable<int>? seleccionados = null)
        {
            var marcados = seleccionados?.ToList() ?? new List<int>();

            var equipos = await _context.Equipos
                .OrderBy(e => e.NombreEquipo)
                .ToListAsync();

            return equipos.Select(e => new SelectListItem
            {
                Value = e.ID.ToString(),
                Text = $"{e.NombreEquipo} ({e.Pais})",
                Selected = marcados.Contains(e.ID)
            });
        }
    }
}