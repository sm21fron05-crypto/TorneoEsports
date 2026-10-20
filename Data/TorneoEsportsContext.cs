using Microsoft.EntityFrameworkCore;
using TorneoEsports.Models;

namespace TorneoEsports.Data
{
    public class TorneoEsportsContext : DbContext
    {
        public TorneoEsportsContext(DbContextOptions<TorneoEsportsContext> options) : base(options)
        {
        }

        public DbSet<Torneo> Torneos => Set<Torneo>();
        public DbSet<Equipo> Equipos => Set<Equipo>();
        public DbSet<Jugador> Jugadores => Set<Jugador>();
        public DbSet<Partida> Partidas => Set<Partida>();
        public DbSet<Sede> Sedes => Set<Sede>();
        public DbSet<Arbitro> Arbitros => Set<Arbitro>();
        public DbSet<EnfrentamientoEquipoPartida> Enfrentamientos => Set<EnfrentamientoEquipoPartida>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1:N Torneo - Partida (CORREGIDO: antes estaba invertido en el diagrama)
            modelBuilder.Entity<Torneo>()
                .HasMany(t => t.Partidas)
                .WithOne(p => p.Torneo)
                .HasForeignKey(p => p.TorneoID);

            // 1:N Equipo - Jugador (CORREGIDO: antes estaba invertido en el diagrama)
            modelBuilder.Entity<Equipo>()
                .HasMany(e => e.Jugadores)
                .WithOne(j => j.Equipo)
                .HasForeignKey(j => j.EquipoID);

            // 1:N Sede - Partida (FK que faltaba en el diagrama original)
            modelBuilder.Entity<Sede>()
                .HasMany(s => s.Partidas)
                .WithOne(p => p.Sede)
                .HasForeignKey(p => p.SedeID);

            // N:N Torneo - Equipo ("Participa"), sin atributos propios -> tabla simple
            modelBuilder.Entity<Torneo>()
                .HasMany(t => t.Equipos)
                .WithMany(e => e.Torneos)
                .UsingEntity("Participa");

            // N:N Partida - Arbitro ("Arbitra"), sin atributos propios -> tabla simple
            modelBuilder.Entity<Partida>()
                .HasMany(p => p.Arbitros)
                .WithMany(a => a.Partidas)
                .UsingEntity("Arbitraje");

            // N:N Equipo - Partida ("Enfrenta") CON atributo propio (Resultado)
            // -> requiere entidad intermedia explícita, no un UsingEntity("string") simple
            modelBuilder.Entity<EnfrentamientoEquipoPartida>()
                .HasKey(en => new { en.EquipoID, en.PartidaID });

            modelBuilder.Entity<EnfrentamientoEquipoPartida>()
                .HasOne(en => en.Equipo)
                .WithMany(e => e.Enfrentamientos)
                .HasForeignKey(en => en.EquipoID);

            modelBuilder.Entity<EnfrentamientoEquipoPartida>()
                .HasOne(en => en.Partida)
                .WithMany(p => p.Enfrentamientos)
                .HasForeignKey(en => en.PartidaID);
        }
    }
}
