using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UI.Models;
using Microsoft.AspNetCore.Identity;

namespace UI.Areas.Operaciones.Controllers
{
    [Area("Operaciones")]
    public class CajaController : Controller
    {
        private readonly CarwashSystemContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public CajaController(CarwashSystemContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Estado actual de caja
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var caja = await _context.Caja
                .Include(c => c.CajaMovimiento)
                .Where(c => c.UsuarioId == userId && c.Estado == "ABIERTA")
                .OrderByDescending(c => c.CajaId)
                .FirstOrDefaultAsync();

            return View(caja);
        }

        // GET: Apertura de caja
        public IActionResult Apertura()
        {
            return PartialView("Partial/_Apertura");
        }

        // POST: Apertura de caja
        [HttpPost]
        public async Task<IActionResult> Apertura(decimal montoInicial)
        {
            var userId = _userManager.GetUserId(User);
            var caja = await _context.Caja.FirstOrDefaultAsync(c => c.UsuarioId == userId);

            if (caja == null)
            {
                caja = new Caja
                {
                    FechaApertura = DateTime.Now,
                    MontoInicial = montoInicial,
                    UsuarioId = userId,
                    Estado = "ABIERTA",
                    TotalLavados = 0,
                    TotalVentas = 0
                };
                _context.Caja.Add(caja);
            }
            else if (caja.Estado == "CERRADA")
            {
                caja.FechaApertura = DateTime.Now;
                caja.MontoInicial = montoInicial;
                caja.Estado = "ABIERTA";
                caja.FechaCierre = null;
                caja.MontoCierre = null;
                caja.TotalLavados = 0;
                caja.TotalVentas = 0;
            }
            else
            {
                return Json(new { success = false, message = "Ya existe una caja abierta." });
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }

        // POST: Cierre de caja
        [HttpPost]
        public async Task<IActionResult> Cierre(decimal montoCierre)
        {
            var userId = _userManager.GetUserId(User);
            var caja = await _context.Caja.FirstOrDefaultAsync(c => c.UsuarioId == userId && c.Estado == "ABIERTA");

            if (caja == null)
                return Json(new { success = false, message = "No hay caja abierta." });

            caja.MontoCierre = montoCierre;
            caja.FechaCierre = DateTime.Now;
            caja.Estado = "CERRADA";
            await _context.SaveChangesAsync();

            // Guardar historial de cierre
            var historial = new HistorialCierre
            {
                CajaId = caja.CajaId,
                FechaCierre = caja.FechaCierre.Value,
                MontoCierre = montoCierre,
                TotalLavados = caja.TotalLavados,
                TotalVentas = caja.TotalVentas,
                TotalIngresos = (caja.TotalLavados + caja.TotalVentas),
                UsuarioId = userId
            };
            _context.HistorialCierre.Add(historial);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

        // GET: Historial de movimientos
        public async Task<IActionResult> Movimientos()
        {
            var userId = _userManager.GetUserId(User);
            var caja = await _context.Caja
                .Where(c => c.UsuarioId == userId)
                .OrderByDescending(c => c.CajaId)
                .FirstOrDefaultAsync();

            var movimientos = await _context.CajaMovimiento
                .Where(m => m.CajaId == caja.CajaId && m.Fecha.Date >= caja.FechaApertura.Date)
                .OrderBy(m => m.Fecha)
                .ToListAsync();

            return PartialView("Partial/_Movimientos", movimientos);
        }
    }
}