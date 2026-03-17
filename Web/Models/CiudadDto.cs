using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class CiudadDto
    {
        public Guid CiudadId { get; set; }

        public string Nombre { get; set; } = string.Empty;
    }
}
