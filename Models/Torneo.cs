using System.ComponentModel.DataAnnotations;

namespace TorneoEsports.Models
{
    public class Torneo
    {
        public int ID { get; set; }

        [Required]
        public string NombreTorneo { get; set; } = string.Empty;

        public DateOnly FechaInicio { get; set; }
        public DateOnly FechaFin { get; set; }

        // 1:N -> Torneo genera muchas Partidas (corregido: antes estaba invertido)
        public List<Partida> Partidas { get; set; } = new();

        // N:N -> Torneo participa con muchos Equipos (relación "Participa", sin atributos propios)
        public List<Equipo> Equipos { get; set; } = new();
    }
}
