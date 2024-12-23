namespace FrontApi.Models
{
    public class Productos
    {
        public int id { get; set; }
        public int codigo_barra { get; set; }

        public string nombre { get; set; }
        public string marca { get; set; }
        public string categoria { get; set; }
        public decimal precio { get; set; }
    }
}
