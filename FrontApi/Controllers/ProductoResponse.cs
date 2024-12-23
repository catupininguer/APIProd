using FrontApi.Models;

namespace APIProductos.Models
{
    public class ProductoResponse
    {
        public required string Mensaje { get; set; }
        public required List<Productos> Response { get; set; }
    }
}