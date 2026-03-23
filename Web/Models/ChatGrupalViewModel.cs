namespace Web.Models
{
    public class ChatGrupalViewModel
    {
        public Guid HistoriaChatId { get; set; }
        public Guid UsuarioId { get; set; }
        public string Nombre { get; set; }
        public string Mensaje { get; set; }
        public DateTime FechaEnvio { get; set; }
    }
}
