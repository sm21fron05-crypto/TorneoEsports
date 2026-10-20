namespace TorneoEsports.Models
{
    public class Partida
    {
        public int ID { get; set; }

        public DateOnly FechaPartida { get; set; }
        public TimeOnly Hora { get; set; }

        // FK del lado "N" hacia Torneo (corregido: cada Partida pertenece a UN solo Torneo)
        public int TorneoID { get; set; }
        public Torneo? Torneo { get; set; }

        // FK hacia Sede (faltaba en el diagrama original; una Partida se juega en UNA sola Sede)
        public int SedeID { get; set; }
        public Sede? Sede { get; set; }

        // N:N -> Partida es arbitrada por varios Arbitros, sin atributos propios
        public List<Arbitro> Arbitros { get; set; } = new();

        // N:N con atributo propio (resultado) -> entidad intermedia explícita
        public List<EnfrentamientoEquipoPartida> Enfrentamientos { get; set; } = new();
    }
}
