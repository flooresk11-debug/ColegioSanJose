using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ColegioSanJose.Data;

namespace ColegioSanJose.Controllers
{
    public class EstadisticasController : Controller
    {
        private readonly ColegioContext _context;

        public EstadisticasController(ColegioContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Promedios()
        {
            var promedios = await _context.Expedientes
                .Include(e => e.Alumno)
                .Where(e => e.NotaFinal.HasValue)
                .GroupBy(e => new { e.AlumnoId, e.Alumno!.Nombre, e.Alumno.Apellido, e.Alumno.Grado })
                .Select(g => new PromedioAlumnoVM
                {
                    AlumnoId = g.Key.AlumnoId,
                    NombreCompleto = g.Key.Nombre + " " + g.Key.Apellido,
                    Grado = g.Key.Grado,
                    Promedio = Math.Round((double)g.Average(e => e.NotaFinal!.Value), 2),
                    TotalMaterias = g.Count(),
                    NotaMaxima = (double)g.Max(e => e.NotaFinal!.Value),
                    NotaMinima = (double)g.Min(e => e.NotaFinal!.Value)
                })
                .OrderByDescending(p => p.Promedio)
                .ToListAsync();

            var distribucion = await _context.Expedientes
                .Where(e => e.NotaFinal.HasValue)
                .Select(e => e.NotaFinal!.Value)
                .ToListAsync();

            ViewBag.Sobresaliente = distribucion.Count(n => n >= 9);
            ViewBag.Bueno = distribucion.Count(n => n >= 7 && n < 9);
            ViewBag.Regular = distribucion.Count(n => n >= 5 && n < 7);
            ViewBag.Deficiente = distribucion.Count(n => n < 5);
            ViewBag.Total = distribucion.Count;

            var promedioMaterias = await _context.Expedientes
                .Include(e => e.Materia)
                .Where(e => e.NotaFinal.HasValue)
                .GroupBy(e => new { e.MateriaId, e.Materia!.NombreMateria })
                .Select(g => new PromedioMateriaVM
                {
                    NombreMateria = g.Key.NombreMateria,
                    Promedio = Math.Round((double)g.Average(e => e.NotaFinal!.Value), 2),
                    TotalAlumnos = g.Count()
                })
                .OrderByDescending(p => p.Promedio)
                .ToListAsync();

            ViewBag.PromedioMaterias = promedioMaterias;

            return View(promedios);
        }
    }

    public class PromedioAlumnoVM
    {
        public int AlumnoId { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string Grado { get; set; } = string.Empty;
        public double Promedio { get; set; }
        public int TotalMaterias { get; set; }
        public double NotaMaxima { get; set; }
        public double NotaMinima { get; set; }
    }

    public class PromedioMateriaVM
    {
        public string NombreMateria { get; set; } = string.Empty;
        public double Promedio { get; set; }
        public int TotalAlumnos { get; set; }
    }
}