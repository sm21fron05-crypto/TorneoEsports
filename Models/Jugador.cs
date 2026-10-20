using System.ComponentModel.DataAnnotations;

namespace TorneoEsports.Models
{
    public class Jugador
    {
        public int ID { get; set; }

        [Required]
        public string Nickname { get; set; } = string.Empty;

        public string NombreReal { get; set; } = string.Empty;

        // FK del lado "N" (corregido: cada Jugador pertenece a UN solo Equipo)
        public int EquipoID { get; set; }
        public Equipo? Equipo { get; set; }
    }
}
