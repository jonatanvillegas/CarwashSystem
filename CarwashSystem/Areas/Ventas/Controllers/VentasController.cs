using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UI.Models;

namespace UI.Areas.Ventas.Controllers
{
    [Area("Ventas")]
    public class VentasController : Controller
    {
        private readonly CarwashSystemContext _context;

        public VentasController(CarwashSystemContext context)
        {
            _context = context;
        }

        // Pantalla principal de venta rápida
        public async Task<IActionResult> Index()
        {
            var productos = await _context.Productos.Where(p => p.Activo).ToListAsync();
            var cajaAbierta = await _context.Caja.FirstOrDefaultAsync(c => c.Estado == "ABIERTA");
            ViewBag.CajaAbierta = cajaAbierta != null;
            return View(productos);
        }

        // Registrar venta
        [HttpPost]
        public async Task<IActionResult> RegistrarVenta([FromBody] VentaPOSModel venta)
        {
            var caja = await _context.Caja.FirstOrDefaultAsync(c => c.Estado == "ABIERTA");
            if (caja == null)
                return Json(new { success = false, message = "No hay caja abierta." });

            if (venta.Productos == null || !venta.Productos.Any())
                return Json(new { success = false, message = "No hay productos en la venta." });

            var nuevaVenta = new Venta
            {
                CajaId = caja.CajaId,
                Total = venta.Total,
                Pago = venta.Pago,
                Vuelto = venta.Vuelto,
                Fecha = DateTime.Now
            };
            _context.Venta.Add(nuevaVenta);
            await _context.SaveChangesAsync();

            foreach (var item in venta.Productos)
            {
                var producto = await _context.Productos.FindAsync(item.ProductoId);
                if (producto == null) continue;

                var subtotal = item.Cantidad * producto.Precio;
                _context.VentaDetalle.Add(new VentaDetalle
                {
                    VentaId = nuevaVenta.VentaId,
                    ProductoId = item.ProductoId,
                    Cantidad = item.Cantidad,
                    PrecioUnitario = producto.Precio,
                    SubTotal = subtotal
                });
            }
            await _context.SaveChangesAsync();

            // Ejecutar SP para movimiento de caja (asegúrate que el tipo sea VENTA_SNACK si lo requiere)
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC op.sp_RegistrarVentaCajaMovimiento @CajaId={0}, @VentaId={1}, @Monto={2}",
                caja.CajaId, nuevaVenta.VentaId, nuevaVenta.Total
            );

            return Json(new { success = true });
        }
    }

    // Modelo para recibir la venta desde AJAX
    public class VentaPOSModel
    {
        public List<VentaPOSProducto> Productos { get; set; }
        public decimal Total { get; set; }
        public decimal Pago { get; set; }
        public decimal Vuelto { get; set; }
    }

    public class VentaPOSProducto
    {
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
    }
}