using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TorneoEsports.ViewModels
{
    public class TorneoFormViewModel
    {
        public int ID { get; set; }

        [Display(Name = "Nombre del torneo")]
        [Required(ErrorMessage = "El nombre del torneo es obligatorio.")]
        [StringLength(80, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 80 caracteres.")]
        public string NombreTorneo { get; set; } = string.Empty;

        [Display(Name = "Fecha de inicio")]
        [Required(ErrorMessage = "La fecha de inicio es obligatoria.")]
        [DataType(DataType.Date)]
        public DateOnly? FechaInicio { get; set; }

        [Display(Name = "Fecha de fin")]
        [Required(ErrorMessage = "La fecha de fin es obligatoria.")]
        [DataType(DataType.Date)]
        public DateOnly? FechaFin { get; set; }

        [Display(Name = "Equipos participantes")]
        public List<int> EquiposSeleccionados { get; set; } = new();

        public IEnumerable<SelectListItem> Equipos { get; set; } = new List<SelectListItem>();
    }
}