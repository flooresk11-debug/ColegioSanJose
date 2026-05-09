using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ColegioSanJose.Data;
using ColegioSanJose.Models.DB;

namespace ColegioSanJose.Controllers
{
    public class AlumnosController : Controller
    {
        private readonly ColegioContext _context;

        public AlumnosController(ColegioContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            var alumnos = _context.Alumnos.AsQueryable();
            if (!string.IsNullOrEmpty(searchString))
                alumnos = alumnos.Where(a =>
                    a.Nombre.Contains(searchString) ||
                    a.Apellido.Contains(searchString));
            ViewData["CurrentFilter"] = searchString;
            return View(await alumnos.OrderBy(a => a.Apellido).ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var alumno = await _context.Alumnos
                .Include(a => a.Expedientes).ThenInclude(e => e.Materia)
                .FirstOrDefaultAsync(a => a.AlumnoId == id);
            if (alumno == null) return NotFound();
            return View(alumno);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nombre,Apellido,FechaNacimiento,Grado")] Alumno alumno)
        {
            if (ModelState.IsValid)
            {
                _context.Add(alumno);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Alumno registrado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            return View(alumno);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var alumno = await _context.Alumnos.FindAsync(id);
            if (alumno == null) return NotFound();
            return View(alumno);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AlumnoId,Nombre,Apellido,FechaNacimiento,Grado")] Alumno alumno)
        {
            if (id != alumno.AlumnoId) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Update(alumno);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Alumno actualizado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            return View(alumno);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var alumno = await _context.Alumnos
                .Include(a => a.Expedientes)
                .FirstOrDefaultAsync(a => a.AlumnoId == id);
            if (alumno == null) return NotFound();
            return View(alumno);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var alumno = await _context.Alumnos.FindAsync(id);
            if (alumno != null) _context.Alumnos.Remove(alumno);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Alumno eliminado exitosamente.";
            return RedirectToAction(nameof(Index));
        }
    }
}