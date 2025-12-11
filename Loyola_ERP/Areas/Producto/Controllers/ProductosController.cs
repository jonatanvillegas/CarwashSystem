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
        public IActionResult Producto(int producto = 0)
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> GuardarProducto([FromBody] Productos modelo)
        {
                         
                await _context.Productos.AddAsync(modelo);
                await _context.SaveChangesAsync();

                return Json(new { mensaje = "Producto guardado exitosamente." });      
            
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

