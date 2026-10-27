using Microsoft.AspNetCore.Mvc.Rendering;
using TorneoEsports.Models;

namespace TorneoEsports.ViewModels
{
    // =========================================================================================
    // VIEWMODEL: JugadorListaViewModel
    // PROPÓSITO: Manejar la pantalla principal (Index) de Jugadores, combinando:
    // 1. La lista resultante de jugadores filtrados.
    // 2. El estado actual de los filtros (para mantener el texto y la selección en el formulario GET).
    // 3. El catálogo de equipos disponibles para el filtro desplegable.
    // =========================================================================================
    public class JugadorListaViewModel
    {
        // Colección de jugadores que coinciden con los filtros aplicados.
        public List<Jugador> Jugadores { get; set; } = new();

        // Almacena el ID del equipo por el cual se está filtrando (o null si no hay filtro).
        public int? EquipoID { get; set; }

        // Almacena el texto de búsqueda por Nickname o Nombre Real.
        public string? Buscar { get; set; }

        // Opciones del dropdown <select> para filtrar por equipo.
        public IEnumerable<SelectListItem> Equipos { get; set; } = new List<SelectListItem>();
    }
}
