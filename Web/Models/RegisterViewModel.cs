using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Usuario { get; set; } = string.Empty;

        [Required]
        [StringLength(150)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Clave { get; set; } = string.Empty;

        public string? MensajeError { get; set; }
        public string? MensajeOk { get; set; }
    }
}
