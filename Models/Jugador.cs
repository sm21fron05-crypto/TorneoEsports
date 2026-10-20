using System.ComponentModel.DataAnnotations;

namespace TorneoEsports.Models
{
    public class Jugador
    {
        public int ID { get; set; }

        [Display(Name = "Nickname")]
        [Required(ErrorMessage = "El nickname es obligatorio.")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "El nickname debe tener entre 2 y 30 caracteres.")]
        public string Nickname { get; set; } = string.Empty;

        [Display(Name = "Nombre real")]
        [Required(ErrorMessage = "El nombre real es obligatorio.")]
        [StringLength(80, ErrorMessage = "El nombre real no puede superar los 80 caracteres.")]
        public string NombreReal { get; set; } = string.Empty;

        // FK del lado "N" (corregido: cada Jugador pertenece a UN solo Equipo)
        [Display(Name = "Equipo")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar un equipo.")]
        public int EquipoID { get; set; }
        public Equipo? Equipo { get; set; }
    }
}
