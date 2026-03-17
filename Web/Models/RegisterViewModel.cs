using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "El usuario es obligatorio")]
        [StringLength(50)]
        public string Usuario { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(150)]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [DataType(DataType.Password)]
        public string Clave { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
        [DataType(DataType.Date)]
        public DateTime FechaNacimiento { get; set; }

        [Required(ErrorMessage = "La ciudad es obligatoria")]
        public Guid? CiudadId { get; set; } 

        [Required(ErrorMessage = "Selecciona un género")]
        public string Genero { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Intereses { get; set; }

        public string? MensajeError { get; set; }
        public string? MensajeOk { get; set; }

        public List<CiudadDto> Ciudades { get; set; } = new List<CiudadDto>();

        // Validación automática +18
        public bool EsMayorDeEdad()
        {
            var hoy = DateTime.Today;
            var edad = hoy.Year - FechaNacimiento.Year;

            if (FechaNacimiento.Date > hoy.AddYears(-edad))
                edad--;

            return edad >= 18;
        }
    }
}