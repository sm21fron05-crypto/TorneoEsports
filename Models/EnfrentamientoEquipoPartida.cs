namespace TorneoEsports.Models
{
    public class EnfrentamientoEquipoPartida
    {
        public int EquipoID { get; set; }
        public Equipo Equipo { get; set; } = null!;

        public int PartidaID { get; set; }
        public Partida Partida { get; set; } = null!;

        public string? Resultado { get; set; }
    }
}
