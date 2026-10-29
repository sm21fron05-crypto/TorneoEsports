using Microsoft.AspNetCore.Mvc.Rendering;
using TorneoEsports.Models;

namespace TorneoEsports.ViewModels
{
    public class CalendarioPartidasViewModel
    {
        public int? TorneoId { get; set; }
        public int? SedeId { get; set; }
        public DateOnly? FechaInicio { get; set; }
        public DateOnly? FechaFin { get; set; }
        public IEnumerable<SelectListItem>? Torneos { get; set; }
        public IEnumerable<SelectListItem>? Sedes { get; set; }
        public List<Partida> Partidas { get; set; } = new List<Partida>();
        public int TotalPartidas => Partidas.Count;
        public Dictionary<string, int> PartidasPorSede { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> PartidasPorArbitro { get; set; } = new Dictionary<string, int>();
    }
}