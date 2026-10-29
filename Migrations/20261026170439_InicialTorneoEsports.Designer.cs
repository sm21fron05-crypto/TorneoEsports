using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TorneoEsports.Data;

#nullable disable

namespace TorneoEsports.Migrations
{
    [DbContext(typeof(TorneoEsportsContext))]
    [Migration("20261026170439_InicialTorneoEsports")]
    partial class InicialTorneoEsports
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "10.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Arbitraje", b =>
                {
                    b.Property<int>("ArbitrosID")
                        .HasColumnType("int");

                    b.Property<int>("PartidasID")
                        .HasColumnType("int");

                    b.HasKey("ArbitrosID", "PartidasID");

                    b.HasIndex("PartidasID");

                    b.ToTable("Arbitraje");
                });

            modelBuilder.Entity("Participa", b =>
                {
                    b.Property<int>("EquiposID")
                        .HasColumnType("int");

                    b.Property<int>("TorneosID")
                        .HasColumnType("int");

                    b.HasKey("EquiposID", "TorneosID");

                    b.HasIndex("TorneosID");

                    b.ToTable("Participa");
                });

            modelBuilder.Entity("TorneoEsports.Models.Arbitro", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("NombreArbitro")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Arbitros");
                });

            modelBuilder.Entity("TorneoEsports.Models.EnfrentamientoEquipoPartida", b =>
                {
                    b.Property<int>("EquipoID")
                        .HasColumnType("int");

                    b.Property<int>("PartidaID")
                        .HasColumnType("int");

                    b.Property<string>("Resultado")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("EquipoID", "PartidaID");

                    b.HasIndex("PartidaID");

                    b.ToTable("Enfrentamientos");
                });

            modelBuilder.Entity("TorneoEsports.Models.Equipo", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("NombreEquipo")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<string>("Pais")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("ID");

                    b.ToTable("Equipos");
                });

            modelBuilder.Entity("TorneoEsports.Models.Jugador", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<int>("EquipoID")
                        .HasColumnType("int");

                    b.Property<string>("Nickname")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("NombreReal")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)");

                    b.HasKey("ID");

                    b.HasIndex("EquipoID");

                    b.ToTable("Jugadores");
                });

            modelBuilder.Entity("TorneoEsports.Models.Partida", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<DateOnly>("FechaPartida")
                        .HasColumnType("date");

                    b.Property<TimeOnly>("Hora")
                        .HasColumnType("time");

                    b.Property<int>("SedeID")
                        .HasColumnType("int");

                    b.Property<int>("TorneoID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("SedeID");

                    b.HasIndex("TorneoID");

                    b.ToTable("Partidas");
                });

            modelBuilder.Entity("TorneoEsports.Models.Sede", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("NombreSede")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Ubicacion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Sedes");
                });

            modelBuilder.Entity("TorneoEsports.Models.Torneo", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<DateOnly>("FechaFin")
                        .HasColumnType("date");

                    b.Property<DateOnly>("FechaInicio")
                        .HasColumnType("date");

                    b.Property<string>("NombreTorneo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Torneos");
                });

            modelBuilder.Entity("Arbitraje", b =>
                {
                    b.HasOne("TorneoEsports.Models.Arbitro", null)
                        .WithMany()
                        .HasForeignKey("ArbitrosID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TorneoEsports.Models.Partida", null)
                        .WithMany()
                        .HasForeignKey("PartidasID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Participa", b =>
                {
                    b.HasOne("TorneoEsports.Models.Equipo", null)
                        .WithMany()
                        .HasForeignKey("EquiposID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TorneoEsports.Models.Torneo", null)
                        .WithMany()
                        .HasForeignKey("TorneosID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TorneoEsports.Models.EnfrentamientoEquipoPartida", b =>
                {
                    b.HasOne("TorneoEsports.Models.Equipo", "Equipo")
                        .WithMany("Enfrentamientos")
                        .HasForeignKey("EquipoID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TorneoEsports.Models.Partida", "Partida")
                        .WithMany("Enfrentamientos")
                        .HasForeignKey("PartidaID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Equipo");

                    b.Navigation("Partida");
                });

            modelBuilder.Entity("TorneoEsports.Models.Jugador", b =>
                {
                    b.HasOne("TorneoEsports.Models.Equipo", "Equipo")
                        .WithMany("Jugadores")
                        .HasForeignKey("EquipoID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Equipo");
                });

            modelBuilder.Entity("TorneoEsports.Models.Partida", b =>
                {
                    b.HasOne("TorneoEsports.Models.Sede", "Sede")
                        .WithMany("Partidas")
                        .HasForeignKey("SedeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TorneoEsports.Models.Torneo", "Torneo")
                        .WithMany("Partidas")
                        .HasForeignKey("TorneoID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Sede");

                    b.Navigation("Torneo");
                });

            modelBuilder.Entity("TorneoEsports.Models.Equipo", b =>
                {
                    b.Navigation("Enfrentamientos");

                    b.Navigation("Jugadores");
                });

            modelBuilder.Entity("TorneoEsports.Models.Partida", b =>
                {
                    b.Navigation("Enfrentamientos");
                });

            modelBuilder.Entity("TorneoEsports.Models.Sede", b =>
                {
                    b.Navigation("Partidas");
                });

            modelBuilder.Entity("TorneoEsports.Models.Torneo", b =>
                {
                    b.Navigation("Partidas");
                });
#pragma warning restore 612, 618
        }
    }
}
