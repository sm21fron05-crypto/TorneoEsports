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
            // 1. Cargar desplegables
            model.Torneos = new SelectList(await _context.Torneos.ToListAsync(), "Id", "Nombre");
            model.Sedes = new SelectList(await _context.Sedes.ToListAsync(), "Id", "Nombre");

            // 2. Consulta base con relaciones
            var query = _context.Partidas
                .Include(p => p.Torneo)
                .Include(p => p.Sede)
                .Include(p => p.Arbitros)
                .Include(p => p.EnfrentamientoEquipoPartidas)
                    .ThenInclude(e => e.Equipo)
                .AsQueryable();

            // 3. Aplicar filtros solo si tienen valor
            if (model.TorneoId.HasValue)
                query = query.Where(p => p.TorneoId == model.TorneoId.Value);

            if (model.SedeId.HasValue)
                query = query.Where(p => p.SedeId == model.SedeId.Value);

            if (model.FechaInicio.HasValue)
                query = query.Where(p => p.FechaHora >= model.FechaInicio.Value);

            if (model.FechaFin.HasValue)
                query = query.Where(p => p.FechaHora <= model.FechaFin.Value);

            // 4. Ejecutar consulta
            model.Partidas = await query.OrderBy(p => p.FechaHora).ToListAsync();

            // 5. Agrupar datos para el resumen
            if (model.Partidas.Any())
            {
                model.PartidasPorSede = model.Partidas
                    .Where(p => p.Sede != null)
                    .GroupBy(p => p.Sede.Nombre)
                    .ToDictionary(g => g.Key, g => g.Count());

                model.PartidasPorArbitro = model.Partidas
                    .SelectMany(p => p.Arbitros)
                    .GroupBy(a => $"{a.Nombre} {a.Apellido}")
                    .ToDictionary(g => g.Key, g => g.Count());
            }

            return View(model);
        }
    }
}