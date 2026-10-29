using System.ComponentModel.DataAnnotations;

namespace TorneoEsports.Models
{
    public class Arbitro
    {
        public int ID { get; set; }

        [Required]
        public string NombreArbitro { get; set; } = string.Empty;
        public List<Partida> Partidas { get; set; } = new();
    }
}
