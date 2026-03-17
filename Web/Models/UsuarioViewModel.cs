namespace Web.Models
{
    public class UsuarioViewModel
    {
        public string Usuario { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Ciudad { get; set; }
        public string Genero { get; set; }
        public string Intereses { get; set; }

        public string Email { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
