using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using FrontApi.Models;
using System.Threading.Tasks;
using APIProductos.Models;

namespace FrontApi.Controllers
{
    public class ProductosController : Controller
    {
        private readonly HttpClient _httpClient;

        public ProductosController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("http://localhost:5145/api/Producto/");
        }

        // Método para listar los productos
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetFromJsonAsync<ProductoResponse>("Lista");

            // Verifica que la respuesta no sea nula y contiene productos
            if (response?.Response != null)
            {
                return View(response.Response); // Pasa la lista de productos a la vista
            }

            // Manejar el caso en que no haya productos o haya un error
            ModelState.AddModelError("", "No se pudieron obtener los productos.");
            return View(new List<Productos>()); // Devuelve una lista vacía en caso de error
        }

        // Método para crear un nuevo producto
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Productos producto)
        {
            if (!ModelState.IsValid) // Verifica la validez del modelo antes de enviar la petición
            {
                return View(producto);
            }

            var response = await _httpClient.PostAsJsonAsync("Guardar", producto);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index)); // Redirige a la lista de productos después de crear
            }

            ModelState.AddModelError("", "Error al guardar el producto");
            return View(producto); // Vuelve a mostrar la vista con el error
        }

        // Método para editar un producto
        public async Task<IActionResult> Edit(int id)
        {
            var producto = await _httpClient.GetFromJsonAsync<Productos>($"Obtener/{id}");
            if (producto == null)
            {
                ModelState.AddModelError("", "Producto no encontrado");
                return RedirectToAction(nameof(Index)); // Si no se encuentra el producto, redirige a la lista
            }
            Console.WriteLine($"Producto: {producto.nombre}, {producto.codigo_barra}, {producto.marca}, {producto.categoria}, {producto.precio}");
            return View(producto); // Pasa el producto a la vista de edición
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Productos producto)
        {
            if (!ModelState.IsValid) // Verifica que el modelo sea válido
            {
                return View(producto); // Si hay errores de validación, vuelve a mostrar el formulario
            }

            var response = await _httpClient.PutAsJsonAsync("Editar", producto);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index)); // Redirige a la lista de productos después de editar
            }

            ModelState.AddModelError("", "Error al editar el producto");
            return View(producto); // Vuelve a mostrar la vista con el error
        }

        // Método para eliminar un producto
        public async Task<IActionResult> Delete(int id)
        {
            var producto = await _httpClient.GetFromJsonAsync<Productos>($"Obtener/{id}");
            if (producto == null)
            {
                ModelState.AddModelError("", "Producto no encontrado");
                return RedirectToAction(nameof(Index)); // Si no se encuentra el producto, redirige a la lista
            }
            return View(producto); // Pasa el producto a la vista de confirmación de eliminación
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _httpClient.DeleteAsync($"Eliminar/{id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index)); // Redirige a la lista de productos después de eliminar
            }

            ModelState.AddModelError("", "Error al eliminar el producto");
            return RedirectToAction(nameof(Index)); // Si falla la eliminación, redirige a la lista
        }
    }
}
