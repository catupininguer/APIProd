using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Cors;
using FrontApi.Models;

namespace FrontApi.Controllers
{
    [EnableCors("ReglasCors")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly string cadenaSQL;

        public ProductoController(IConfiguration config)
        {
            cadenaSQL = config.GetConnectionString("CadenaSQL") ?? throw new ArgumentNullException(nameof(config));
        }

        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista()
        {
            List<Productos> lista = new List<Productos>();

            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    await conexion.OpenAsync();
                    var cmd = new SqlCommand("ListarProductos", conexion)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    using (var rd = await cmd.ExecuteReaderAsync())
                    {
                        while (await rd.ReadAsync())
                        {
                            lista.Add(new Productos()
                            {
                                id = Convert.ToInt32(rd["id"]),
                                codigo_barra = Convert.ToInt32(rd["codigo_barra"]),
                                nombre = rd["nombre"].ToString(),
                                marca = rd["marca"].ToString(),
                                categoria = rd["categoria"].ToString(),
                                precio = Convert.ToDecimal(rd["precio"])
                            });
                        }
                    }
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = lista });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { mensaje = "Ocurrió un error al obtener los productos", error = error.Message, response = lista });
            }
        }

        [HttpGet]
        [Route("Obtener/{id:int}")]
        public async Task<IActionResult> Obtener(int id)
        {
            List<Productos> lista = new List<Productos>();
            Productos productos = new Productos();

            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    await conexion.OpenAsync();
                    var cmd = new SqlCommand("ObtenerProducto", conexion)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("id", id); // Agregar el parámetro ID para obtener un solo producto
                    using (var rd = await cmd.ExecuteReaderAsync())
                    {
                        while (await rd.ReadAsync())
                        {
                            lista.Add(new Productos()
                            {
                                id = Convert.ToInt32(rd["id"]),
                                codigo_barra = Convert.ToInt32(rd["codigo_barra"]),
                                nombre = rd["nombre"].ToString(),
                                marca = rd["marca"].ToString(),
                                categoria = rd["categoria"].ToString(),
                                precio = Convert.ToDecimal(rd["precio"])
                            });
                        }
                    }

                }

                productos = lista.Where(item => item.id == id).FirstOrDefault();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = productos });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { mensaje = "Ocurrió un error al obtener los productos", error = error.Message, response = productos });
            }
        }

        [HttpPost]
        [Route("Guardar")]
        public IActionResult Guardar([FromBody] Productos objeto)
        {
            List<Productos> lista = new List<Productos>();
            Productos productos = new Productos();

            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("GuardarProducto", conexion);
                    cmd.Parameters.AddWithValue("codigo_barra", objeto.codigo_barra);
                    cmd.Parameters.AddWithValue("nombre", objeto.nombre);
                    cmd.Parameters.AddWithValue("marca", objeto.marca);
                    cmd.Parameters.AddWithValue("categoria", objeto.categoria);
                    cmd.Parameters.AddWithValue("precio", objeto.precio);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.ExecuteNonQuery();
                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok" });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { mensaje = "Ocurrió un error al obtener los productos", error = error.Message });
            }
        }

        [HttpPut]
        [Route("Editar")]
        public IActionResult Editar([FromBody] Productos objeto)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("EditarProducto", conexion);
                    cmd.Parameters.AddWithValue("id", objeto.id == 0 ? DBNull.Value : objeto.id);
                    cmd.Parameters.AddWithValue("codigo_barra", objeto.codigo_barra == 0 ? DBNull.Value : objeto.codigo_barra);
                    cmd.Parameters.AddWithValue("nombre", objeto.nombre is null ? DBNull.Value : objeto.nombre);
                    cmd.Parameters.AddWithValue("marca", objeto.marca is null ? DBNull.Value : objeto.marca);
                    cmd.Parameters.AddWithValue("categoria", objeto.categoria is null ? DBNull.Value : objeto.categoria);
                    cmd.Parameters.AddWithValue("precio", objeto.precio == 0 ? DBNull.Value : objeto.precio);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.ExecuteNonQuery();
                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Editado" });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { error = error.Message });
            }
        }
        [HttpDelete]
        [Route("Eliminar/{id:int}")]
        public IActionResult Eliminar(int id)
        {

            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("EliminarProducto", conexion);
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.ExecuteNonQuery();
                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Eliminado" });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { error = error.Message });
            }
        }
    }
}