using System.ComponentModel.DataAnnotations;

namespace ColegioSanJose.Models.DB
{
    public class Materia
    {
        public int MateriaId { get; set; }

        [Required(ErrorMessage = "El nombre de la materia es obligatorio.")]
        [StringLength(150)]
        [Display(Name = "Nombre de la Materia")]
        public string NombreMateria { get; set; } = string.Empty;

        [Required(ErrorMessage = "El docente es obligatorio.")]
        [StringLength(150)]
        [Display(Name = "Docente")]
        public string Docente { get; set; } = string.Empty;

        public ICollection<Expediente> Expedientes { get; set; } = new List<Expediente>();
    }
}