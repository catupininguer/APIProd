using FrontApi.Models;

namespace APIProductos.Models
{
    public class ProductoResponse
    {
        public string Mensaje { get; set; }
        public List<Productos> Response { get; set; }
    }
}
