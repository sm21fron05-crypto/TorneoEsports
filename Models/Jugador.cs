using System.ComponentModel.DataAnnotations;

namespace TorneoEsports.Models
{
    // =========================================================================================
    // MODELO DE DOMINIO: Jugador
    // Representa la tabla [Jugadores] en la base de datos SQL Server.
    // =========================================================================================
    public class Jugador
    {
        // Clave Primaria (PK) autoincremental
        public int ID { get; set; }

        [Display(Name = "Nickname")]
        [Required(ErrorMessage = "El nickname es obligatorio.")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "El nickname debe tener entre 2 y 30 caracteres.")]
        public string Nickname { get; set; } = string.Empty;

        [Display(Name = "Nombre real")]
        [Required(ErrorMessage = "El nombre real es obligatorio.")]
        [StringLength(80, ErrorMessage = "El nombre real no puede superar los 80 caracteres.")]
        public string NombreReal { get; set; } = string.Empty;

        // =========================================================================================
        // CLAVE FORÁNEA (FK) Y NAVEGACIÓN
        // =========================================================================================
        // [Range(1, int.MaxValue)]: Valida que el ID seleccionado sea mayor a 0 (evita valor por defecto 0).
        [Display(Name = "Equipo")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar un equipo.")]
        public int EquipoID { get; set; }

        // Propiedad de navegación hacia la entidad padre (Equipo al que pertenece este jugador).
        // Se define como anulable (?) para evitar errores de validación previa al enlace de datos.
        public Equipo? Equipo { get; set; }
    }
}
