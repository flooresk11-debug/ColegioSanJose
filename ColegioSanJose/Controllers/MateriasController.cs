using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ColegioSanJose.Data;
using ColegioSanJose.Models.DB;

namespace ColegioSanJose.Controllers
{
    public class MateriasController : Controller
    {
        private readonly ColegioContext _context;

        public MateriasController(ColegioContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            var materias = _context.Materias.AsQueryable();
            if (!string.IsNullOrEmpty(searchString))
                materias = materias.Where(m =>
                    m.NombreMateria.Contains(searchString) ||
                    m.Docente.Contains(searchString));
            ViewData["CurrentFilter"] = searchString;
            return View(await materias.OrderBy(m => m.NombreMateria).ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var materia = await _context.Materias
                .Include(m => m.Expedientes).ThenInclude(e => e.Alumno)
                .FirstOrDefaultAsync(m => m.MateriaId == id);
            if (materia == null) return NotFound();
            return View(materia);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NombreMateria,Docente")] Materia materia)
        {
            if (ModelState.IsValid)
            {
                _context.Add(materia);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Materia registrada exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            return View(materia);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var materia = await _context.Materias.FindAsync(id);
            if (materia == null) return NotFound();
            return View(materia);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MateriaId,NombreMateria,Docente")] Materia materia)
        {
            if (id != materia.MateriaId) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Update(materia);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Materia actualizada exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            return View(materia);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var materia = await _context.Materias
                .Include(m => m.Expedientes)
                .FirstOrDefaultAsync(m => m.MateriaId == id);
            if (materia == null) return NotFound();
            return View(materia);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var materia = await _context.Materias.FindAsync(id);
            if (materia != null) _context.Materias.Remove(materia);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Materia eliminada exitosamente.";
            return RedirectToAction(nameof(Index));
        }
    }
}