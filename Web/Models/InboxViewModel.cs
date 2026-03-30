namespace Web.Models
{
    public class InboxViewModel
    {
        public Guid UsuarioId { get; set; }
        public string Nombre { get; set; }
        public string UltimoMensaje { get; set; }
        public DateTime Fecha { get; set; }

        // 🔥 OPCIONAL (te recomiendo agregar)
        public int NoLeidos { get; set; }
    }
}