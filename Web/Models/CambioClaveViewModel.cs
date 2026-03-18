using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class CambioClaveViewModel
    {
        [Required(ErrorMessage = "Debes ingresar una clave.")]
        public string NuevaClave { get; set; }

        [Required(ErrorMessage = "Debes confirmar la clave.")]
        public string ConfirmarClave { get; set; }
    }
}