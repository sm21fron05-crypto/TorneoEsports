using System.ComponentModel.DataAnnotations;

namespace TorneoEsports.Models
{
    public class Equipo
    {
        public int ID { get; set; }

        [Display(Name = "Nombre del equipo")]
        [Required(ErrorMessage = "El nombre del equipo es obligatorio.")]
        [StringLength(60, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 60 caracteres.")]
        public string NombreEquipo { get; set; } = string.Empty;

        [Display(Name = "País")]
        [Required(ErrorMessage = "El país es obligatorio.")]
        [StringLength(50, ErrorMessage = "El país no puede superar los 50 caracteres.")]
        public string Pais { get; set; } = string.Empty;

        // 1:N -> Equipo tiene muchos Jugadores (corregido: antes estaba invertido)
        public List<Jugador> Jugadores { get; set; } = new();

        // N:N -> Equipo participa en muchos Torneos (relación "Participa")
        public List<Torneo> Torneos { get; set; } = new();

        // N:N con atributo propio (resultado) -> se resuelve con entidad intermedia explícita
        public List<EnfrentamientoEquipoPartida> Enfrentamientos { get; set; } = new();
    }
}
