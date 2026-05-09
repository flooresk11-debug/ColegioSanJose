using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ColegioSanJose.Data;
using ColegioSanJose.Models.DB;

namespace ColegioSanJose.Controllers
{
    public class ExpedientesController : Controller
    {
        private readonly ColegioContext _context;

        public ExpedientesController(ColegioContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString, int? alumnoFiltro)
        {
            var expedientes = _context.Expedientes
                .Include(e => e.Alumno)
                .Include(e => e.Materia)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
                expedientes = expedientes.Where(e =>
                    e.Alumno!.Nombre.Contains(searchString) ||
                    e.Alumno.Apellido.Contains(searchString) ||
                    e.Materia!.NombreMateria.Contains(searchString));

            if (alumnoFiltro.HasValue)
                expedientes = expedientes.Where(e => e.AlumnoId == alumnoFiltro);

            ViewData["CurrentFilter"] = searchString;
            ViewBag.Alumnos = new SelectList(
                await _context.Alumnos.OrderBy(a => a.Apellido).ToListAsync(),
                "AlumnoId", "NombreCompleto", alumnoFiltro);

            return View(await expedientes
                .OrderBy(e => e.Alumno!.Apellido)
                .ThenBy(e => e.Materia!.NombreMateria)
                .ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var expediente = await _context.Expedientes
                .Include(e => e.Alumno)
                .Include(e => e.Materia)
                .FirstOrDefaultAsync(e => e.ExpedienteId == id);
            if (expediente == null) return NotFound();
            return View(expediente);
        }

        public IActionResult Create()
        {
            CargarSelectLists();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AlumnoId,MateriaId,NotaFinal,Observaciones")] Expediente expediente)
        {
            if (ModelState.IsValid)
            {
                _context.Add(expediente);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Expediente registrado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            CargarSelectLists(expediente.AlumnoId, expediente.MateriaId);
            return View(expediente);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var expediente = await _context.Expedientes.FindAsync(id);
            if (expediente == null) return NotFound();
            CargarSelectLists(expediente.AlumnoId, expediente.MateriaId);
            return View(expediente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ExpedienteId,AlumnoId,MateriaId,NotaFinal,Observaciones")] Expediente expediente)
        {
            if (id != expediente.ExpedienteId) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Update(expediente);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Expediente actualizado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            CargarSelectLists(expediente.AlumnoId, expediente.MateriaId);
            return View(expediente);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var expediente = await _context.Expedientes
                .Include(e => e.Alumno)
                .Include(e => e.Materia)
                .FirstOrDefaultAsync(e => e.ExpedienteId == id);
            if (expediente == null) return NotFound();
            return View(expediente);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var expediente = await _context.Expedientes.FindAsync(id);
            if (expediente != null) _context.Expedientes.Remove(expediente);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Expediente eliminado exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        private void CargarSelectLists(int alumnoId = 0, int materiaId = 0)
        {
            ViewBag.AlumnoId = new SelectList(
                _context.Alumnos.OrderBy(a => a.Apellido).ToList(),
                "AlumnoId", "NombreCompleto", alumnoId);
            ViewBag.MateriaId = new SelectList(
                _context.Materias.OrderBy(m => m.NombreMateria).ToList(),
                "MateriaId", "NombreMateria", materiaId);
        }
    }
}