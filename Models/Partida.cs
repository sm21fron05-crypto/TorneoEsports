namespace TorneoEsports.Models
{
    public class Partida
    {
        public int ID { get; set; }

        public DateOnly FechaPartida { get; set; }
        public TimeOnly Hora { get; set; }
        public int TorneoID { get; set; }
        public Torneo? Torneo { get; set; }
        public int SedeID { get; set; }
        public Sede? Sede { get; set; }
        public List<Arbitro> Arbitros { get; set; } = new();
        public List<EnfrentamientoEquipoPartida> Enfrentamientos { get; set; } = new();
    }
}
