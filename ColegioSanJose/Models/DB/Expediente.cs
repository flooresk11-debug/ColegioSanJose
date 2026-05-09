using System.ComponentModel.DataAnnotations;

namespace ColegioSanJose.Models.DB
{
    public class Expediente
    {
        public int ExpedienteId { get; set; }

        [Required(ErrorMessage = "El alumno es obligatorio.")]
        [Display(Name = "Alumno")]
        public int AlumnoId { get; set; }

        [Required(ErrorMessage = "La materia es obligatoria.")]
        [Display(Name = "Materia")]
        public int MateriaId { get; set; }

        [Range(0, 10, ErrorMessage = "La nota debe estar entre 0 y 10.")]
        [Display(Name = "Nota Final")]
        public decimal? NotaFinal { get; set; }

        [StringLength(500)]
        [Display(Name = "Observaciones")]
        public string? Observaciones { get; set; }

        public Alumno? Alumno { get; set; }
        public Materia? Materia { get; set; }
    }
}