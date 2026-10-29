using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TorneoEsports.ViewModels
{
    // =========================================================================================
    // VIEWMODEL: JugadorFormViewModel
    // PROPÓSITO: Transportar los datos entre el formulario de la vista (Create/Edit) y el controlador.
    // ¿POR QUÉ UN VIEWMODEL?:
    // 1. Evita vulnerabilidades de sobreasignación (Overposting / Mass Assignment).
    // 2. Incluye propiedades que no existen en la BD pero sí en la vista, como la lista de equipos
    //    para el <select> desplegable (IEnumerable<SelectListItem>).
    // =========================================================================================
    public class JugadorFormViewModel
    {
        // Se utiliza principalmente en el Edit para saber qué registro actualizar; en Create es 0.
        public int ID { get; set; }

        [Display(Name = "Nickname")]
        [Required(ErrorMessage = "El nickname es obligatorio.")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "El nickname debe tener entre 2 y 30 caracteres.")]
        public string Nickname { get; set; } = string.Empty;

        [Display(Name = "Nombre real")]
        [Required(ErrorMessage = "El nombre real es obligatorio.")]
        [StringLength(80, ErrorMessage = "El nombre real no puede superar los 80 caracteres.")]
        public string NombreReal { get; set; } = string.Empty;

        [Display(Name = "Equipo")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar un equipo.")]
        public int EquipoID { get; set; }

        // Lista de opciones para llenar el elemento HTML <select> en el formulario.
        public IEnumerable<SelectListItem> Equipos { get; set; } = new List<SelectListItem>();
    }
}
