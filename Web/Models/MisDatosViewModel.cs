namespace Web.Models
{
    public class MisDatosViewModel
    {
        public UsuarioViewModel Usuario { get; set; }
        public List<CiudadDto> Ciudades { get; set; }
        public List<GeneroDto> Generos { get; set; }
    }
}
