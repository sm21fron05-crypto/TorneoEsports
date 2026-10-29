using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TorneoEsports.Data;
using TorneoEsports.Models;
using TorneoEsports.ViewModels;

namespace TorneoEsports.Controllers
{
    public class PartidasController : Controller
    {
        private readonly TorneoEsportsContext _context;

        public PartidasController(TorneoEsportsContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var partidas = await _context.Partidas
                .Include(p => p.Torneo)
                .Include(p => p.Sede)
                .Include(p => p.Arbitros)
                .Include(p => p.Enfrentamientos)
                    .ThenInclude(e => e.Equipo)
                .OrderBy(p => p.FechaPartida)
                .ThenBy(p => p.Hora)
                .ToListAsync();
            return View(partidas);
        }

        public IActionResult Create()
        {
            var model = new PartidaFormViewModel();
            CargarListasDesplegables(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PartidaFormViewModel model)
        {
            if (model.Equipo1Id != 0 && model.Equipo1Id == model.Equipo2Id)
            {
                ModelState.AddModelError(string.Empty, "Los dos equipos deben ser distintos.");
            }

            if (model.ArbitrosIds == null || !model.ArbitrosIds.Any())
            {
                ModelState.AddModelError(nameof(model.ArbitrosIds), "Debe asignar al menos un árbitro.");
            }

            var torneo = await _context.Torneos.FindAsync(model.TorneoId);
            if (torneo != null && (model.FechaPartida < torneo.FechaInicio || model.FechaPartida > torneo.FechaFin))
            {
                ModelState.AddModelError(nameof(model.FechaPartida),
                    $"La fecha debe estar entre {torneo.FechaInicio:dd/MM/yyyy} y {torneo.FechaFin:dd/MM/yyyy} (fechas del torneo).");
            }

            bool sedeOcupada = await _context.Partidas
                .AnyAsync(p => p.SedeID == model.SedeId
                            && p.FechaPartida == model.FechaPartida
                            && p.Hora == model.Hora);
            if (sedeOcupada)
            {
                ModelState.AddModelError(nameof(model.SedeId), "Esa sede ya tiene una partida a la misma fecha y hora.");
            }

            if (model.ArbitrosIds != null && model.ArbitrosIds.Any())
            {
                var ids = model.ArbitrosIds;
                bool arbitroOcupado = await _context.Partidas
                    .AnyAsync(p => p.FechaPartida == model.FechaPartida
                                && p.Hora == model.Hora
                                && p.Arbitros.Any(a => ids.Contains(a.ID)));
                if (arbitroOcupado)
                {
                    ModelState.AddModelError(nameof(model.ArbitrosIds), "Un árbitro seleccionado ya está en otra partida a la misma fecha y hora.");
                }
            }

            if (ModelState.IsValid)
            {
                var ids = model.ArbitrosIds!;
                var partida = new Partida
                {
                    TorneoID = model.TorneoId,
                    SedeID = model.SedeId,
                    FechaPartida = model.FechaPartida,
                    Hora = model.Hora,
                    Arbitros = await _context.Arbitros.Where(a => ids.Contains(a.ID)).ToListAsync(),
                    Enfrentamientos = new List<EnfrentamientoEquipoPartida>
                    {
                        new EnfrentamientoEquipoPartida { EquipoID = model.Equipo1Id, Resultado = null },
                        new EnfrentamientoEquipoPartida { EquipoID = model.Equipo2Id, Resultado = null }
                    }
                };

                _context.Partidas.Add(partida);
                await _context.SaveChangesAsync();
                TempData["Exito"] = "Partida registrada correctamente.";
                return RedirectToAction(nameof(Index));
            }

            CargarListasDesplegables(model);
            return View(model);
        }

        private void CargarListasDesplegables(PartidaFormViewModel model)
        {
            model.Torneos = new SelectList(_context.Torneos.OrderBy(t => t.NombreTorneo), "ID", "NombreTorneo", model.TorneoId);
            model.Sedes = new SelectList(_context.Sedes.OrderBy(s => s.NombreSede), "ID", "NombreSede", model.SedeId);
            model.Equipos = new SelectList(_context.Equipos.OrderBy(e => e.NombreEquipo), "ID", "NombreEquipo");
            model.Arbitros = new MultiSelectList(_context.Arbitros.OrderBy(a => a.NombreArbitro), "ID", "NombreArbitro", model.ArbitrosIds);
        }
    }
}