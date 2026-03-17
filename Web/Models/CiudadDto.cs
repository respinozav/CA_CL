using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class CiudadDto
    {
        public Guid CiudadId { get; set; }

        [Required(ErrorMessage = "La ciudad es obligatoria")]
        public string Nombre { get; set; } = string.Empty;
    }
}
