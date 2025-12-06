using Microsoft.AspNetCore.Mvc;

namespace UI.Areas.Producto.Controllers
{
    [Area("Producto")]
    public class ProductosController : Controller
    {
        public IActionResult ListadoProductos()
        {
            return View();
        }
        public IActionResult Producto(int producto = 0)
        {
            return View();
        }
    }
}
