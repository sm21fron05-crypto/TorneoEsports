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
                .Include(p => p.EnfrentamientoEquipoPartidas)
                    .ThenInclude(e => e.Equipo)
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
            if (model.Equipo1Id == model.Equipo2Id)
            {
                ModelState.AddModelError(string.Empty, "Los dos equipos deben ser distintos.");
            }

            var torneo = await _context.Torneos.FindAsync(model.TorneoId);
            if (torneo != null && (model.FechaHora < torneo.FechaInicio || model.FechaHora > torneo.FechaFin))
            {
                ModelState.AddModelError("FechaHora", "La fecha debe estar entre FechaInicio y FechaFin del torneo.");
            }

            bool sedeOcupada = await _context.Partidas
                .AnyAsync(p => p.SedeId == model.SedeId && p.FechaHora == model.FechaHora);
            if (sedeOcupada)
            {
                ModelState.AddModelError("SedeId", "Una sede no puede tener dos partidas a la misma fecha y hora.");
            }

            if (model.ArbitrosIds != null && model.ArbitrosIds.Any())
            {
                foreach (var arbitroId in model.ArbitrosIds)
                {
                    bool arbitroOcupado = await _context.Partidas
                        .Include(p => p.Arbitros)
                        .AnyAsync(p => p.FechaHora == model.FechaHora && p.Arbitros.Any(a => a.Id == arbitroId));
                    if (arbitroOcupado)
                    {
                        ModelState.AddModelError("ArbitrosIds", "Un árbitro no puede estar en dos partidas a la misma fecha y hora.");
                        break;
                    }
                }
            }

            if (ModelState.IsValid)
            {
                var partida = new Partida
                {
                    TorneoId = model.TorneoId,
                    SedeId = model.SedeId,
                    FechaHora = model.FechaHora,
                    Arbitros = await _context.Arbitros.Where(a => model.ArbitrosIds.Contains(a.Id)).ToListAsync(),
                    EnfrentamientoEquipoPartidas = new List<EnfrentamientoEquipoPartida>
                    {
                        new EnfrentamientoEquipoPartida { EquipoId = model.Equipo1Id, Resultado = null },
                        new EnfrentamientoEquipoPartida { EquipoId = model.Equipo2Id, Resultado = null }
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
            model.Torneos = new SelectList(_context.Torneos, "Id", "Nombre");
            model.Sedes = new SelectList(_context.Sedes, "Id", "Nombre");
            model.Equipos = new SelectList(_context.Equipos, "Id", "Nombre");
            model.Arbitros = new MultiSelectList(_context.Arbitros, "Id", "Nombre");
        }
    }
}