using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TorneoEsports.Migrations
{
    /// <inheritdoc />
    public partial class InicialTorneoEsports : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Arbitros",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreArbitro = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Arbitros", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Equipos",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreEquipo = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Pais = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipos", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Sedes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreSede = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ubicacion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sedes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Torneos",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreTorneo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaInicio = table.Column<DateOnly>(type: "date", nullable: false),
                    FechaFin = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Torneos", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Jugadores",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nickname = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    NombreReal = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    EquipoID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jugadores", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Jugadores_Equipos_EquipoID",
                        column: x => x.EquipoID,
                        principalTable: "Equipos",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Participa",
                columns: table => new
                {
                    EquiposID = table.Column<int>(type: "int", nullable: false),
                    TorneosID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participa", x => new { x.EquiposID, x.TorneosID });
                    table.ForeignKey(
                        name: "FK_Participa_Equipos_EquiposID",
                        column: x => x.EquiposID,
                        principalTable: "Equipos",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Participa_Torneos_TorneosID",
                        column: x => x.TorneosID,
                        principalTable: "Torneos",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Partidas",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaPartida = table.Column<DateOnly>(type: "date", nullable: false),
                    Hora = table.Column<TimeOnly>(type: "time", nullable: false),
                    TorneoID = table.Column<int>(type: "int", nullable: false),
                    SedeID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Partidas", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Partidas_Sedes_SedeID",
                        column: x => x.SedeID,
                        principalTable: "Sedes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Partidas_Torneos_TorneoID",
                        column: x => x.TorneoID,
                        principalTable: "Torneos",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Arbitraje",
                columns: table => new
                {
                    ArbitrosID = table.Column<int>(type: "int", nullable: false),
                    PartidasID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Arbitraje", x => new { x.ArbitrosID, x.PartidasID });
                    table.ForeignKey(
                        name: "FK_Arbitraje_Arbitros_ArbitrosID",
                        column: x => x.ArbitrosID,
                        principalTable: "Arbitros",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Arbitraje_Partidas_PartidasID",
                        column: x => x.PartidasID,
                        principalTable: "Partidas",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Enfrentamientos",
                columns: table => new
                {
                    EquipoID = table.Column<int>(type: "int", nullable: false),
                    PartidaID = table.Column<int>(type: "int", nullable: false),
                    Resultado = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enfrentamientos", x => new { x.EquipoID, x.PartidaID });
                    table.ForeignKey(
                        name: "FK_Enfrentamientos_Equipos_EquipoID",
                        column: x => x.EquipoID,
                        principalTable: "Equipos",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Enfrentamientos_Partidas_PartidaID",
                        column: x => x.PartidaID,
                        principalTable: "Partidas",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Arbitraje_PartidasID",
                table: "Arbitraje",
                column: "PartidasID");

            migrationBuilder.CreateIndex(
                name: "IX_Enfrentamientos_PartidaID",
                table: "Enfrentamientos",
                column: "PartidaID");

            migrationBuilder.CreateIndex(
                name: "IX_Jugadores_EquipoID",
                table: "Jugadores",
                column: "EquipoID");

            migrationBuilder.CreateIndex(
                name: "IX_Participa_TorneosID",
                table: "Participa",
                column: "TorneosID");

            migrationBuilder.CreateIndex(
                name: "IX_Partidas_SedeID",
                table: "Partidas",
                column: "SedeID");

            migrationBuilder.CreateIndex(
                name: "IX_Partidas_TorneoID",
                table: "Partidas",
                column: "TorneoID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Arbitraje");

            migrationBuilder.DropTable(
                name: "Enfrentamientos");

            migrationBuilder.DropTable(
                name: "Jugadores");

            migrationBuilder.DropTable(
                name: "Participa");

            migrationBuilder.DropTable(
                name: "Arbitros");

            migrationBuilder.DropTable(
                name: "Partidas");

            migrationBuilder.DropTable(
                name: "Equipos");

            migrationBuilder.DropTable(
                name: "Sedes");

            migrationBuilder.DropTable(
                name: "Torneos");
        }
    }
}
