using System.ComponentModel.DataAnnotations;

namespace TorneoEsports.Models
{
    public class Arbitro
    {
        public int ID { get; set; }

        [Required]
        public string NombreArbitro { get; set; } = string.Empty;

        // N:N -> un Arbitro arbitra muchas Partidas, sin atributos propios en la relación
        public List<Partida> Partidas { get; set; } = new();
    }
}
