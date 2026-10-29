using System.ComponentModel.DataAnnotations;

namespace TorneoEsports.Models
{
    public class Sede
    {
        public int ID { get; set; }

        [Required]
        public string NombreSede { get; set; } = string.Empty;

        public string Ubicacion { get; set; } = string.Empty;
        public List<Partida> Partidas { get; set; } = new();
    }
}
