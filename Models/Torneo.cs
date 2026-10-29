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
        public List<Partida> Partidas { get; set; } = new();
        public List<Equipo> Equipos { get; set; } = new();
    }
}
