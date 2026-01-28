namespace Web.Models
{
    public class LoginResultDto
    {
        public required string Codigo { get; set; }
        public required string Descripcion { get; set; }

        // Solo viene en LOGIN_OK
        public Guid? UsuarioId { get; set; }
        public string? Usuario { get; set; }
        public string? Nombre { get; set; }
        public string? Email { get; set; }
        public Guid? RolId { get; set; }
        public string? RolNombre { get; set; }
    }

}
