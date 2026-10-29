using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TorneoEsports.Data;
using TorneoEsports.Models;
using TorneoEsports.ViewModels;

namespace TorneoEsports.Controllers
{
    // =========================================================================================
    // CONTROLADOR: JugadoresController
    // Implementa el CRUD de jugadores con ViewModel, desplegable de equipos y reglas de negocio.
    // =========================================================================================
    public class JugadoresController : Controller
    {
        // Inyección de dependencias del DbContext
        private readonly TorneoEsportsContext _context;

        public JugadoresController(TorneoEsportsContext context)
        {
            _context = context;
        }

        // =========================================================================================
        // LISTADO Y FILTROS (INDEX)
        // GET: /Jugadores?equipoId=1&buscar=texto
        // Permite filtrar por equipo y/o buscar por Nickname o Nombre Real.
        // =========================================================================================
        public async Task<IActionResult> Index(int? equipoId, string? buscar)
        {
            // Carga ansiosa (.Include) del equipo para mostrar el nombre del equipo en la tabla.
            var consulta = _context.Jugadores
                .Include(j => j.Equipo)
                .AsQueryable();

            // Filtro 1: Si se seleccionó un equipo en el desplegable.
            if (equipoId.HasValue)
            {
                consulta = consulta.Where(j => j.EquipoID == equipoId.Value);
            }

            // Filtro 2: Si se escribió texto en la caja de búsqueda.
            if (!string.IsNullOrWhiteSpace(buscar))
            {
                consulta = consulta.Where(j => j.Nickname.Contains(buscar) || j.NombreReal.Contains(buscar));
            }

            // Construcción del ViewModel que transporta la lista y el estado de los filtros a la vista.
            var modelo = new JugadorListaViewModel
            {
                Jugadores = await consulta.OrderBy(j => j.Nickname).ToListAsync(),
                EquipoID = equipoId,
                Buscar = buscar,
                Equipos = await ObtenerEquiposAsync(equipoId) // Llena el dropdown de equipos manteniendo la selección
            };

            return View(modelo);
        }

        // =========================================================================================
        // DETALLES (DETAILS)
        // GET: /Jugadores/Details/5
        // =========================================================================================
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var jugador = await _context.Jugadores
                .Include(j => j.Equipo)
                .FirstOrDefaultAsync(j => j.ID == id);

            if (jugador == null) return NotFound();

            return View(jugador);
        }

        // =========================================================================================
        // REGISTRO DE JUGADOR (CREATE)
        // =========================================================================================
        // GET: /Jugadores/Create -> Prepara el ViewModel con la lista de equipos para el desplegable.
        public async Task<IActionResult> Create()
        {
            var modelo = new JugadorFormViewModel
            {
                Equipos = await ObtenerEquiposAsync() // Carga los equipos disponibles en la BD
            };

            return View(modelo);
        }

        // POST: /Jugadores/Create -> Recibe el formulario enviado por el usuario.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(JugadorFormViewModel modelo)
        {
            // 1. Validar reglas de negocio personalizadas (existencia de equipo y nickname único).
            await ValidarReglasAsync(modelo);

            // 2. Si falla alguna validación (Data Annotations o de negocio):
            // ¡IMPORTANTE!: Hay que volver a cargar la lista de Equipos antes de retornar la vista,
            // de lo contrario el elemento <select> renderizaría vacío y causaría error.
            if (!ModelState.IsValid)
            {
                modelo.Equipos = await ObtenerEquiposAsync(modelo.EquipoID);
                return View(modelo);
            }

            // 3. Mapeo del ViewModel hacia la entidad de dominio 'Jugador'.
            var jugador = new Jugador
            {
                Nickname = modelo.Nickname.Trim(),
                NombreReal = modelo.NombreReal.Trim(),
                EquipoID = modelo.EquipoID
            };

            // 4. Inserción en base de datos.
            _context.Add(jugador);
            await _context.SaveChangesAsync();

            // 5. Mensaje de éxito en TempData y redirección al Index.
            TempData["Exito"] = $"El jugador \"{jugador.Nickname}\" se registró correctamente.";
            return RedirectToAction(nameof(Index));
        }

        // =========================================================================================
        // EDICIÓN DE JUGADOR (EDIT)
        // =========================================================================================
        // GET: /Jugadores/Edit/5 -> Carga los datos del jugador en el ViewModel para el formulario.
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var jugador = await _context.Jugadores.FindAsync(id);
            if (jugador == null) return NotFound();

            var modelo = new JugadorFormViewModel
            {
                ID = jugador.ID,
                Nickname = jugador.Nickname,
                NombreReal = jugador.NombreReal,
                EquipoID = jugador.EquipoID,
                Equipos = await ObtenerEquiposAsync(jugador.EquipoID)
            };

            return View(modelo);
        }

        // POST: /Jugadores/Edit/5 -> Procesa la actualización.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, JugadorFormViewModel modelo)
        {
            if (id != modelo.ID) return NotFound();

            await ValidarReglasAsync(modelo);

            // Si hay errores de validación, recargamos el select y volvemos a mostrar el formulario.
            if (!ModelState.IsValid)
            {
                modelo.Equipos = await ObtenerEquiposAsync(modelo.EquipoID);
                return View(modelo);
            }

            var jugador = await _context.Jugadores.FindAsync(id);
            if (jugador == null) return NotFound();

            // Actualizamos los campos de la entidad con los valores validados del ViewModel.
            jugador.Nickname = modelo.Nickname.Trim();
            jugador.NombreReal = modelo.NombreReal.Trim();
            jugador.EquipoID = modelo.EquipoID;

            await _context.SaveChangesAsync();

            TempData["Exito"] = $"El jugador \"{jugador.Nickname}\" se actualizó correctamente.";
            return RedirectToAction(nameof(Index));
        }

        // =========================================================================================
        // ELIMINACIÓN DE JUGADOR (DELETE)
        // =========================================================================================
        // GET: /Jugadores/Delete/5 -> Pantalla de confirmación con datos del jugador y su equipo.
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var jugador = await _context.Jugadores
                .Include(j => j.Equipo)
                .FirstOrDefaultAsync(j => j.ID == id);

            if (jugador == null) return NotFound();

            return View(jugador);
        }

        // POST: /Jugadores/Delete/5 -> Confirma la eliminación física del registro.
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jugador = await _context.Jugadores.FindAsync(id);
            if (jugador == null) return NotFound();

            _context.Jugadores.Remove(jugador);
            await _context.SaveChangesAsync();

            TempData["Exito"] = $"El jugador \"{jugador.Nickname}\" se eliminó correctamente.";
            return RedirectToAction(nameof(Index));
        }

        // =========================================================================================
        // REGLAS DE NEGOCIO Y MÉTODOS AUXILIARES
        // =========================================================================================
        // Valida que el equipo exista realmente en BD y que el Nickname no esté duplicado.
        private async Task ValidarReglasAsync(JugadorFormViewModel modelo)
        {
            // Regla 1: Validar que el equipo seleccionado exista en la base de datos.
            if (modelo.EquipoID > 0 && !await _context.Equipos.AnyAsync(e => e.ID == modelo.EquipoID))
            {
                ModelState.AddModelError(nameof(modelo.EquipoID), "El equipo seleccionado no existe.");
            }

            // Regla 2: Validar unicidad del Nickname (ignora el propio registro si estamos en Edit).
            var nickname = modelo.Nickname?.Trim();
            if (!string.IsNullOrEmpty(nickname) &&
                await _context.Jugadores.AnyAsync(j => j.ID != modelo.ID && j.Nickname == nickname))
            {
                ModelState.AddModelError(nameof(modelo.Nickname), "Ya existe un jugador con ese nickname.");
            }
        }

        // Consulta los equipos en la BD y los transforma en una colección de SelectListItem para el HTML <select>.
        private async Task<IEnumerable<SelectListItem>> ObtenerEquiposAsync(int? seleccionado = null)
        {
            var equipos = await _context.Equipos
                .OrderBy(e => e.NombreEquipo)
                .ToListAsync();

            return equipos.Select(e => new SelectListItem
            {
                Value = e.ID.ToString(),
                Text = e.NombreEquipo,
                Selected = e.ID == seleccionado
            });
        }
    }
}
