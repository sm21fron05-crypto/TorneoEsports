using System.ComponentModel.DataAnnotations;

namespace TorneoEsports.Models
{
    // =========================================================================================
    // MODELO DE DOMINIO: Equipo
    // Representa la tabla [Equipos] en la base de datos SQL Server mediante Entity Framework Core.
    // =========================================================================================
    public class Equipo
    {
        // Clave Primaria (PK) - Por convención de EF Core, una propiedad llamada 'ID' o 'EquipoID'
        // se configura automáticamente como IDENTITY(1,1) autoincremental.
        public int ID { get; set; }

        // [Display]: Especifica el texto visible de la etiqueta <label> en las vistas Razor.
        // [Required]: Obliga a que el campo no sea nulo ni vacío tanto en cliente como en servidor.
        // [StringLength]: Limita la longitud máxima y mínima en la validación y define NVARCHAR(60) en SQL.
        [Display(Name = "Nombre del equipo")]
        [Required(ErrorMessage = "El nombre del equipo es obligatorio.")]
        [StringLength(60, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 60 caracteres.")]
        public string NombreEquipo { get; set; } = string.Empty;

        [Display(Name = "País")]
        [Required(ErrorMessage = "El país es obligatorio.")]
        [StringLength(50, ErrorMessage = "El país no puede superar los 50 caracteres.")]
        public string Pais { get; set; } = string.Empty;

        // =========================================================================================
        // PROPIEDADES DE NAVEGACIÓN (RELACIONES ENTITY FRAMEWORK CORE)
        // =========================================================================================

        // Relación 1:N -> Un Equipo tiene muchos Jugadores asociados.
        public List<Jugador> Jugadores { get; set; } = new();

        // Relación N:N -> Un Equipo participa en muchos Torneos (tabla intermedia 'Participa').
        public List<Torneo> Torneos { get; set; } = new();

        // Relación N:N con atributo propio (Resultado) -> Resuelto con entidad intermedia explícita.
        public List<EnfrentamientoEquipoPartida> Enfrentamientos { get; set; } = new();
    }
}
