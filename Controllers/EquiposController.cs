using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TorneoEsports.Data;
using TorneoEsports.Models;

namespace TorneoEsports.Controllers
{
    // =========================================================================================
    // CONTROLADOR: EquiposController
    // Implementa el CRUD completo, búsqueda, validación de unicidad e integridad referencial.
    // =========================================================================================
    public class EquiposController : Controller
    {
        // Inyección de dependencias del contexto de base de datos
        private readonly TorneoEsportsContext _context;

        public EquiposController(TorneoEsportsContext context)
        {
            _context = context;
        }

        // =========================================================================================
        // LISTADO Y BÚSQUEDA (INDEX)
        // GET: /Equipos o /Equipos?buscar=texto
        // =========================================================================================
        public async Task<IActionResult> Index(string? buscar)
        {
            // Include(e => e.Jugadores): Carga ansiosa (Eager Loading) para contar los jugadores de cada equipo.
            // AsQueryable(): Permite construir la consulta SQL dinámicamente antes de ejecutarla en el servidor.
            var consulta = _context.Equipos
                .Include(e => e.Jugadores)
                .AsQueryable();

            // Si el usuario envió un criterio de búsqueda, agregamos la cláusula WHERE.
            if (!string.IsNullOrWhiteSpace(buscar))
            {
                consulta = consulta.Where(e => e.NombreEquipo.Contains(buscar) || e.Pais.Contains(buscar));
            }

            // Guardamos el texto de búsqueda en ViewData para repoblar el input de la vista.
            ViewData["Buscar"] = buscar;

            // ToListAsync() ejecuta la consulta SQL en la BD de forma asíncrona.
            return View(await consulta.OrderBy(e => e.NombreEquipo).ToListAsync());
        }

        // =========================================================================================
        // DETALLES (DETAILS)
        // GET: /Equipos/Details/5
        // =========================================================================================
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            // Carga el equipo junto a sus jugadores y los torneos en los que participa.
            var equipo = await _context.Equipos
                .Include(e => e.Jugadores)
                .Include(e => e.Torneos)
                .FirstOrDefaultAsync(e => e.ID == id);

            if (equipo == null) return NotFound();

            return View(equipo);
        }

        // =========================================================================================
        // REGISTRO DE EQUIPO (CREATE)
        // =========================================================================================
        // GET: /Equipos/Create -> Muestra el formulario vacío.
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Equipos/Create -> Procesa los datos enviados desde el formulario.
        // [ValidateAntiForgeryToken]: Protege contra ataques CSRF (Cross-Site Request Forgery).
        // [Bind]: Evita sobreasignación de campos maliciosos (solo mapea NombreEquipo y Pais).
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NombreEquipo,Pais")] Equipo equipo)
        {
            // Validación personalizada: verificar que el nombre no esté duplicado.
            await ValidarNombreUnicoAsync(equipo);

            // Si las validaciones de DataAnnotations o la de unicidad fallaron, recargamos la vista.
            if (!ModelState.IsValid) return View(equipo);

            // Agrega la entidad al contexto y persiste los cambios en SQL Server.
            _context.Add(equipo);
            await _context.SaveChangesAsync();

            // Notificación vía TempData (patrón Post-Redirect-Get).
            TempData["Exito"] = $"El equipo \"{equipo.NombreEquipo}\" se registró correctamente.";
            return RedirectToAction(nameof(Index));
        }

        // =========================================================================================
        // EDICIÓN DE EQUIPO (EDIT)
        // =========================================================================================
        // GET: /Equipos/Edit/5 -> Carga los datos actuales del equipo en el formulario.
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var equipo = await _context.Equipos.FindAsync(id);
            if (equipo == null) return NotFound();

            return View(equipo);
        }

        // POST: /Equipos/Edit/5 -> Guarda los cambios editados.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,NombreEquipo,Pais")] Equipo equipo)
        {
            // Seguridad: verificar que el ID de la URL coincida con el ID del objeto enviado.
            if (id != equipo.ID) return NotFound();

            await ValidarNombreUnicoAsync(equipo);

            if (!ModelState.IsValid) return View(equipo);

            try
            {
                _context.Update(equipo);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Manejo de concurrencia: si otro usuario eliminó el registro mientras se editaba.
                if (!await _context.Equipos.AnyAsync(e => e.ID == id)) return NotFound();
                throw;
            }

            TempData["Exito"] = $"El equipo \"{equipo.NombreEquipo}\" se actualizó correctamente.";
            return RedirectToAction(nameof(Index));
        }

        // =========================================================================================
        // ELIMINACIÓN DE EQUIPO (DELETE)
        // =========================================================================================
        // GET: /Equipos/Delete/5 -> Muestra la pantalla de confirmación.
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var equipo = await _context.Equipos
                .Include(e => e.Jugadores)
                .FirstOrDefaultAsync(e => e.ID == id);

            if (equipo == null) return NotFound();

            return View(equipo);
        }

        // POST: /Equipos/Delete/5 -> Confirma la eliminación física (o la bloquea si tiene dependencias).
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var equipo = await _context.Equipos
                .Include(e => e.Jugadores)
                .Include(e => e.Enfrentamientos)
                .FirstOrDefaultAsync(e => e.ID == id);

            if (equipo == null) return NotFound();

            // REGLA DE INTEGRIDAD: No permitir eliminar si tiene jugadores asignados.
            if (equipo.Jugadores.Count > 0)
            {
                TempData["Error"] = $"No se puede eliminar \"{equipo.NombreEquipo}\" porque tiene {equipo.Jugadores.Count} jugador(es) registrado(s). Elimina o reasigna los jugadores primero.";
                return RedirectToAction(nameof(Index));
            }

            // REGLA DE INTEGRIDAD: No permitir eliminar si ya tiene historial de partidas.
            if (equipo.Enfrentamientos.Count > 0)
            {
                TempData["Error"] = $"No se puede eliminar \"{equipo.NombreEquipo}\" porque ya participa en partidas.";
                return RedirectToAction(nameof(Index));
            }

            _context.Equipos.Remove(equipo);
            await _context.SaveChangesAsync();

            TempData["Exito"] = $"El equipo \"{equipo.NombreEquipo}\" se eliminó correctamente.";
            return RedirectToAction(nameof(Index));
        }

        // =========================================================================================
        // MÉTODO AUXILIAR DE VALIDACIÓN DE REGLAS DE NEGOCIO
        // =========================================================================================
        // Evita dos equipos con el mismo nombre (compara excluyendo el propio ID en caso de edición).
        private async Task ValidarNombreUnicoAsync(Equipo equipo)
        {
            var existe = await _context.Equipos.AnyAsync(e =>
                e.ID != equipo.ID && e.NombreEquipo == equipo.NombreEquipo);

            if (existe)
            {
                // Inyecta el error directamente al ModelState asociado a la propiedad 'NombreEquipo'.
                ModelState.AddModelError(nameof(Equipo.NombreEquipo), "Ya existe un equipo con ese nombre.");
            }
        }
    }
}
