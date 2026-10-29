using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TorneoEsports.Data;
using TorneoEsports.ViewModels;

namespace TorneoEsports.Controllers
{
    public class ReportesController : Controller
    {
        private readonly TorneoEsportsContext _context;

        public ReportesController(TorneoEsportsContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> CalendarioPartidas(CalendarioPartidasViewModel model)
        {
            model.Torneos = new SelectList(await _context.Torneos.OrderBy(t => t.NombreTorneo).ToListAsync(),
                "ID", "NombreTorneo", model.TorneoId);
            model.Sedes = new SelectList(await _context.Sedes.OrderBy(s => s.NombreSede).ToListAsync(),
                "ID", "NombreSede", model.SedeId);

            var query = _context.Partidas
                .Include(p => p.Torneo)
                .Include(p => p.Sede)
                .Include(p => p.Arbitros)
                .Include(p => p.Enfrentamientos)
                    .ThenInclude(e => e.Equipo)
                .AsQueryable();

            if (model.TorneoId.HasValue)
                query = query.Where(p => p.TorneoID == model.TorneoId.Value);

            if (model.SedeId.HasValue)
                query = query.Where(p => p.SedeID == model.SedeId.Value);

            if (model.FechaInicio.HasValue)
                query = query.Where(p => p.FechaPartida >= model.FechaInicio.Value);

            if (model.FechaFin.HasValue)
                query = query.Where(p => p.FechaPartida <= model.FechaFin.Value);

            model.Partidas = await query
                .OrderBy(p => p.FechaPartida)
                .ThenBy(p => p.Hora)
                .ToListAsync();

            model.PartidasPorSede = model.Partidas
                .Where(p => p.Sede != null)
                .GroupBy(p => p.Sede!.NombreSede)
                .ToDictionary(g => g.Key, g => g.Count());

            model.PartidasPorArbitro = model.Partidas
                .SelectMany(p => p.Arbitros)
                .GroupBy(a => a.NombreArbitro)
                .ToDictionary(g => g.Key, g => g.Count());

            return View(model);
        }
    }
}
