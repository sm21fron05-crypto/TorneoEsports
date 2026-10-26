using Microsoft.AspNetCore.Mvc.Rendering;
using TorneoEsports.Models;

namespace TorneoEsports.ViewModels
{
    // Lista de jugadores con el filtro por equipo.
    public class JugadorListaViewModel
    {
        public List<Jugador> Jugadores { get; set; } = new();

        public int? EquipoID { get; set; }

        public string? Buscar { get; set; }

        public IEnumerable<SelectListItem> Equipos { get; set; } = new List<SelectListItem>();
    }
}
