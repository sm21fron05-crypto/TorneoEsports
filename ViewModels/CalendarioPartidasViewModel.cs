using Microsoft.AspNetCore.Mvc.Rendering;
using TorneoEsports.Models;

namespace TorneoEsports.ViewModels
{
    public class CalendarioPartidasViewModel
    {
        // Filtros opcionales
        public int? TorneoId { get; set; }
        public int? SedeId { get; set; }
        public DateOnly? FechaInicio { get; set; }
        public DateOnly? FechaFin { get; set; }

        // Listas para poblar los selects
        public IEnumerable<SelectListItem>? Torneos { get; set; }
        public IEnumerable<SelectListItem>? Sedes { get; set; }

        // Resultados de la consulta
        public List<Partida> Partidas { get; set; } = new List<Partida>();

        // Totalizadores para el resumen
        public int TotalPartidas => Partidas.Count;
        public Dictionary<string, int> PartidasPorSede { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> PartidasPorArbitro { get; set; } = new Dictionary<string, int>();
    }
}