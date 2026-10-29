using TorneoEsports.Models;

namespace TorneoEsports.Data
{
    public static class DbInitializer
    {
        public static void Sembrar(TorneoEsportsContext db)
        {
            if (db.Equipos.Any())
            {
                return;
            }

            var equipos = new List<Equipo>
            {
                new Equipo
                {
                    NombreEquipo = "Cóndores Gaming",
                    Pais = "Bolivia",
                    Jugadores = new List<Jugador>
                    {
                        new Jugador { Nickname = "CondorX", NombreReal = "Marco Quispe" },
                        new Jugador { Nickname = "AndesKid", NombreReal = "Luis Mamani" },
                        new Jugador { Nickname = "Altiplano", NombreReal = "Diego Rojas" },
                        new Jugador { Nickname = "SigloXXI", NombreReal = "Carla Flores" }
                    }
                },
                new Equipo
                {
                    NombreEquipo = "Titanes del Sur",
                    Pais = "Argentina",
                    Jugadores = new List<Jugador>
                    {
                        new Jugador { Nickname = "TitanZero", NombreReal = "Matías Gómez" },
                        new Jugador { Nickname = "PampaFox", NombreReal = "Julián Pérez" },
                        new Jugador { Nickname = "Gaucho99", NombreReal = "Nicolás Díaz" },
                        new Jugador { Nickname = "Surena", NombreReal = "Camila Torres" }
                    }
                },
                new Equipo
                {
                    NombreEquipo = "Dragones Rojos",
                    Pais = "Chile",
                    Jugadores = new List<Jugador>
                    {
                        new Jugador { Nickname = "RedDragon", NombreReal = "Felipe Soto" },
                        new Jugador { Nickname = "Cordillera", NombreReal = "Benjamín Rivas" },
                        new Jugador { Nickname = "FuegoAustral", NombreReal = "Javiera Muñoz" },
                        new Jugador { Nickname = "Pehuen", NombreReal = "Tomás Herrera" }
                    }
                },
                new Equipo
                {
                    NombreEquipo = "Lobos Digitales",
                    Pais = "Perú",
                    Jugadores = new List<Jugador>
                    {
                        new Jugador { Nickname = "LoboAlfa", NombreReal = "Renzo Castillo" },
                        new Jugador { Nickname = "Inti", NombreReal = "Sofía Vargas" },
                        new Jugador { Nickname = "Chasqui", NombreReal = "Gabriel Ramos" },
                        new Jugador { Nickname = "Machu", NombreReal = "Andrea Salazar" }
                    }
                }
            };

            var sedes = new List<Sede>
            {
                new Sede { NombreSede = "Arena Gamer La Paz", Ubicacion = "La Paz, Zona Sur" },
                new Sede { NombreSede = "Coliseo Digital", Ubicacion = "Cochabamba, Centro" },
                new Sede { NombreSede = "Estadio eSports", Ubicacion = "Santa Cruz, Equipetrol" }
            };

            var arbitros = new List<Arbitro>
            {
                new Arbitro { NombreArbitro = "Roberto Aguilar" },
                new Arbitro { NombreArbitro = "Patricia Vega" },
                new Arbitro { NombreArbitro = "Hernán Cárdenas" },
                new Arbitro { NombreArbitro = "Lucía Mendoza" }
            };

            var torneo = new Torneo
            {
                NombreTorneo = "Copa Andina de E-Sports 2026",
                FechaInicio = new DateOnly(2026, 11, 2),
                FechaFin = new DateOnly(2026, 11, 30),
                Equipos = equipos,
                Partidas = new List<Partida>
                {
                    new Partida
                    {
                        FechaPartida = new DateOnly(2026, 11, 5),
                        Hora = new TimeOnly(15, 0),
                        Sede = sedes[0],
                        Arbitros = new List<Arbitro> { arbitros[0], arbitros[1] },
                        Enfrentamientos = new List<EnfrentamientoEquipoPartida>
                        {
                            new EnfrentamientoEquipoPartida { Equipo = equipos[0] },
                            new EnfrentamientoEquipoPartida { Equipo = equipos[1] }
                        }
                    },
                    new Partida
                    {
                        FechaPartida = new DateOnly(2026, 11, 5),
                        Hora = new TimeOnly(18, 0),
                        Sede = sedes[1],
                        Arbitros = new List<Arbitro> { arbitros[2] },
                        Enfrentamientos = new List<EnfrentamientoEquipoPartida>
                        {
                            new EnfrentamientoEquipoPartida { Equipo = equipos[2] },
                            new EnfrentamientoEquipoPartida { Equipo = equipos[3] }
                        }
                    },
                    new Partida
                    {
                        FechaPartida = new DateOnly(2026, 11, 12),
                        Hora = new TimeOnly(16, 30),
                        Sede = sedes[2],
                        Arbitros = new List<Arbitro> { arbitros[1], arbitros[3] },
                        Enfrentamientos = new List<EnfrentamientoEquipoPartida>
                        {
                            new EnfrentamientoEquipoPartida { Equipo = equipos[0] },
                            new EnfrentamientoEquipoPartida { Equipo = equipos[2] }
                        }
                    },
                    new Partida
                    {
                        FechaPartida = new DateOnly(2026, 11, 19),
                        Hora = new TimeOnly(17, 0),
                        Sede = sedes[0],
                        Arbitros = new List<Arbitro> { arbitros[0], arbitros[3] },
                        Enfrentamientos = new List<EnfrentamientoEquipoPartida>
                        {
                            new EnfrentamientoEquipoPartida { Equipo = equipos[1] },
                            new EnfrentamientoEquipoPartida { Equipo = equipos[3] }
                        }
                    }
                }
            };

            db.Equipos.AddRange(equipos);
            db.Sedes.AddRange(sedes);
            db.Arbitros.AddRange(arbitros);
            db.Torneos.Add(torneo);
            db.SaveChanges();
        }
    }
}
