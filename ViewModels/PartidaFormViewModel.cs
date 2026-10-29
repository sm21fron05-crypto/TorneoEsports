using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TorneoEsports.ViewModels
{
    public class PartidaFormViewModel
    {
        [Required(ErrorMessage = "Seleccione un torneo.")]
        [Display(Name = "Torneo")]
        public int TorneoId { get; set; }

        [Required(ErrorMessage = "Seleccione una sede.")]
        [Display(Name = "Sede")]
        public int SedeId { get; set; }

        [Required(ErrorMessage = "La fecha es obligatoria.")]
        [Display(Name = "Fecha de la partida")]
        public DateOnly FechaPartida { get; set; } = DateOnly.FromDateTime(DateTime.Today);

        [Required(ErrorMessage = "La hora es obligatoria.")]
        [Display(Name = "Hora")]
        public TimeOnly Hora { get; set; } = new(10, 0);

        [Required(ErrorMessage = "Seleccione el primer equipo.")]
        [Display(Name = "Equipo 1")]
        public int Equipo1Id { get; set; }

        [Required(ErrorMessage = "Seleccione el segundo equipo.")]
        [Display(Name = "Equipo 2")]
        public int Equipo2Id { get; set; }

        [Required(ErrorMessage = "Debe asignar al menos un árbitro.")]
        [Display(Name = "Árbitros")]
        public List<int> ArbitrosIds { get; set; } = new List<int>();
        public IEnumerable<SelectListItem>? Torneos { get; set; }
        public IEnumerable<SelectListItem>? Sedes { get; set; }
        public IEnumerable<SelectListItem>? Equipos { get; set; }
        public IEnumerable<SelectListItem>? Arbitros { get; set; }
    }
}