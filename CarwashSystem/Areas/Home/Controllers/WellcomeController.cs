using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UI.Models;

namespace UI.Areas.Home.Controllers
{
    [Area("Home")]
    public class WellcomeController : BaseController
    {
        private readonly CarwashSystemContext _context;
        public WellcomeController(CarwashSystemContext context):base(context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var dashboardData = await _context.Procedures.sp_DashboardAsync();
            var model = dashboardData.FirstOrDefault();
            return View(model);
        }
        public async Task<IActionResult> UltimosLavados()
        {
            var caja = await _context.Caja
                .Where(c => c.Estado == "ABIERTA")
                .Select(c => new { c.FechaApertura })
                .FirstOrDefaultAsync();

            if (caja == null)
            {
                return PartialView("Partial/_Lavados", new List<Lavados_View>());
            }

            var lavados = await _context.Lavados_View
                .Where(l => l.FechaCierre >= caja.FechaApertura)
                .OrderByDescending(l => l.FechaCierre)
                .Take(5)
                .ToListAsync();

            return PartialView("Partial/_Lavados", lavados);
        }
        public async Task<IActionResult> UltimasVentas()
        {
            var caja = await _context.Caja
                .Where(c => c.Estado == "ABIERTA")
                .Select(c => new { c.FechaApertura })
                .FirstOrDefaultAsync();

            if (caja == null)
            {
                return PartialView("Partial/_Ventas", new List<sp_ProductosMasVendidosResult>());
            }

            var ventas = await _context.Procedures.sp_ProductosMasVendidosAsync();

            return PartialView("Partial/_Ventas", ventas);
        }
    }
}
