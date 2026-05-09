using System.ComponentModel.DataAnnotations;

namespace ColegioSanJose.Models.DB
{
    public class Alumno
    {
        public int AlumnoId { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100)]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        [StringLength(100)]
        [Display(Name = "Apellido")]
        public string Apellido { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Nacimiento")]
        public DateTime? FechaNacimiento { get; set; }

        [Required(ErrorMessage = "El grado es obligatorio.")]
        [StringLength(50)]
        [Display(Name = "Grado")]
        public string Grado { get; set; } = string.Empty;

        public ICollection<Expediente> Expedientes { get; set; } = new List<Expediente>();

        public string NombreCompleto => $"{Nombre} {Apellido}";
    }
}