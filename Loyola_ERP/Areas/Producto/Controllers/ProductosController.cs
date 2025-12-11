using UI.Models; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Loyola_ERP.Data;
using System.Linq;


namespace UI.Areas.Producto.Controllers
{
    [Area("Producto")]
    public class ProductosController : Controller
    {
        private readonly TiendaProductosContext _context;

        public ProductosController(TiendaProductosContext context)
        {
            _context = context;
        }
        public IActionResult ListadoProductos()
        {
            return View();
        }
        public async Task<IActionResult> Producto(int productoId = 0)
        {
            var producto = await _context.Productos.FirstOrDefaultAsync(c => c.Id == productoId);
            if(producto == null)
            {
                producto = new Productos();
            }
            return View(producto);
        }
        [HttpPost]
        public async Task<IActionResult> GuardarProducto([FromBody] Productos modelo)
        {
            if (modelo.Id > 0)
            {
                var productoExistente = await _context.Productos.FindAsync(modelo.Id);
                if (productoExistente != null)
                {
                    productoExistente.Nombre = modelo.Nombre;
                    productoExistente.Codigo = modelo.Codigo;
                    productoExistente.Stock = modelo.Stock;
                    productoExistente.Precio = modelo.Precio;
                    productoExistente.EstadoId = modelo.EstadoId;

                    _context.Productos.Update(productoExistente);
                    await _context.SaveChangesAsync();

                    return Json(new { mensaje = "Producto actualizado exitosamente." });
                }
                else
                {
                    return Json(new { mensaje = "Producto no encontrado." });
                }
            }
            else
            {
                await _context.Productos.AddAsync(modelo);
                await _context.SaveChangesAsync();

                return Json(new { mensaje = "Producto guardado exitosamente." });
            }
        }
        [HttpGet]
        public async Task<IActionResult> ListarProductos()
        {
            return View();

        }

        [HttpGet]
        public IActionResult ObtenerProductos(string nombreBusqueda, string estadoBusqueda)
        {
            try
            {
                var query = _context.ProductoView.AsQueryable();

                if (!string.IsNullOrWhiteSpace(nombreBusqueda))
                {
                    query = query.Where(p => p.nombre.Contains(nombreBusqueda));
                }

                if (!string.IsNullOrWhiteSpace(estadoBusqueda))
                {
                    query = query.Where(p => p.estadoNombre == estadoBusqueda);
                }

                return Json(query.ToList());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Ocurrió un error al obtener los productos: " + ex.Message });
            }
        }


    }
}

