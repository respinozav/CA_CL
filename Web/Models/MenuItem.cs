namespace Web.Models
{
    public class MenuItem
    {
        public Guid MenuId { get; set; }
        public Guid? MenuPadreId { get; set; }
        public string Nombre { get; set; }
        public string Icono { get; set; }
        public string Ruta { get; set; }
        public int Orden { get; set; }
    }

}
