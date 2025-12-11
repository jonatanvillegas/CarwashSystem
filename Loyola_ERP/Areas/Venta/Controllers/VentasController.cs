using Microsoft.AspNetCore.Mvc;
using UI.Models;

namespace UI.Areas.Venta.Controllers
{
    [Area("Venta")]
    public class VentasController : Controller
    {
        private readonly TiendaProductosContext _context;

        public VentasController(TiendaProductosContext context)
        {
            _context = context;
        }
        public IActionResult Ventas()
        {
            return View();
        }
    }
}
