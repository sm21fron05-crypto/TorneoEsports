namespace TorneoEsports.Models
{
    // Entidad intermedia para la relación "Enfrenta" (Equipo <-> Partida).
    // Se necesita como clase explícita porque la relación tiene un atributo propio: Resultado.
    public class EnfrentamientoEquipoPartida
    {
        public int EquipoID { get; set; }
        public Equipo Equipo { get; set; } = null!;

        public int PartidaID { get; set; }
        public Partida Partida { get; set; } = null!;

        public string? Resultado { get; set; }
    }
}
