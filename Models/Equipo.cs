using System.ComponentModel.DataAnnotations;

namespace TorneoEsports.Models
{
    public class Equipo
    {
        public int ID { get; set; }

        [Required]
        public string NombreEquipo { get; set; } = string.Empty;

        public string Pais { get; set; } = string.Empty;

        // 1:N -> Equipo tiene muchos Jugadores (corregido: antes estaba invertido)
        public List<Jugador> Jugadores { get; set; } = new();

        // N:N -> Equipo participa en muchos Torneos (relación "Participa")
        public List<Torneo> Torneos { get; set; } = new();

        // N:N con atributo propio (resultado) -> se resuelve con entidad intermedia explícita
        public List<EnfrentamientoEquipoPartida> Enfrentamientos { get; set; } = new();
    }
}
