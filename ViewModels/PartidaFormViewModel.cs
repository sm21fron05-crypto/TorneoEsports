using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TorneoEsports.ViewModels
{
    public class PartidaFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Seleccione un torneo.")]
        [Display(Name = "Torneo")]
        public int TorneoId { get; set; }

        [Required(ErrorMessage = "Seleccione una sede.")]
        [Display(Name = "Sede")]
        public int SedeId { get; set; }

        [Required(ErrorMessage = "La fecha y hora son obligatorias.")]
        [Display(Name = "Fecha y Hora")]
        public DateTime FechaHora { get; set; }

        [Required(ErrorMessage = "Seleccione el primer equipo.")]
        [Display(Name = "Equipo 1")]
        public int Equipo1Id { get; set; }

        [Required(ErrorMessage = "Seleccione el segundo equipo.")]
        [Display(Name = "Equipo 2")]
        public int Equipo2Id { get; set; }

        [Required(ErrorMessage = "Debe asignar al menos un árbitro.")]
        [Display(Name = "Árbitros")]
        public List<int> ArbitrosIds { get; set; } = new List<int>();

        // Propiedades auxiliares para poblar los selects en la vista
        public IEnumerable<SelectListItem>? Torneos { get; set; }
        public IEnumerable<SelectListItem>? Sedes { get; set; }
        public IEnumerable<SelectListItem>? Equipos { get; set; }
        public IEnumerable<SelectListItem>? Arbitros { get; set; }
    }
}