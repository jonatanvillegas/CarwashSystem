using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UI.Models;

namespace UI.Areas.Operaciones.Controllers
{
    [Area("Operaciones")]
    public class LavadosController : BaseController
    {

        public LavadosController(CarwashSystemContext context): base(context)
        {

        }

        // Listar lavados pendientes usando la vista
        public async Task<IActionResult> Index()
        {
            var lavados = await _context.Lavados_View
                .Where(l => l.Estado == "PENDIENTE")
                .OrderByDescending(l => l.FechaRegistro)
                .ToListAsync();
            ViewBag.CajaAbierta = CajaAbierta();
            return View(lavados);
        }

        // GET: Crear lavado (con selección de servicios)
        public IActionResult Create()
        {
            ViewBag.TiposVehiculo = _context.TiposVehiculo.Where(t => t.Activo).ToList();
            ViewBag.Marcas = _context.Marcas.Where(m => m.Activo).ToList();
            ViewBag.Servicios = _context.Servicios.Where(s => s.Activo).ToList();
            return PartialView("Partial/_CreateEdit", new Lavados());
        }

        // POST: Crear lavado con servicios seleccionados
        [HttpPost]
        public async Task<IActionResult> Create(Lavados lavado, int[] serviciosSeleccionados)
        {
            if (ModelState.IsValid && serviciosSeleccionados != null && serviciosSeleccionados.Length > 0)
            {
                lavado.Estado = "PENDIENTE";
                lavado.FechaRegistro = DateTime.Now;
                _context.Add(lavado);
                await _context.SaveChangesAsync();

                // Agregar los servicios seleccionados al lavado
                foreach (var servicioId in serviciosSeleccionados)
                {
                    _context.LavadoServicios.Add(new LavadoServicios
                    {
                        LavadoId = lavado.Id,
                        ServicioId = servicioId,
                        Precio = null // o 0, según tu lógica
                    });
                }
                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }
            ViewBag.TiposVehiculo = _context.TiposVehiculo.Where(t => t.Activo).ToList();
            ViewBag.Marcas = _context.Marcas.Where(m => m.Activo).ToList();
            ViewBag.Servicios = _context.Servicios.Where(s => s.Activo).ToList();
            ViewBag.Error = "Debes seleccionar al menos un servicio.";
            return PartialView("Partial/_CreateEdit", lavado);
        }

        // Detalle de lavado usando la vista
        public async Task<IActionResult> Detalle(int id)
        {
            var lavado = await _context.Lavados_View.FirstOrDefaultAsync(l => l.Id == id);
            if (lavado == null) return NotFound();

            var servicios = await _context.LavadoServicios
                .Include(ls => ls.Servicio)
                .Where(ls => ls.LavadoId == id)
                .ToListAsync();

            ViewBag.Servicios = servicios;
            ViewBag.ServiciosCatalogo = _context.Servicios.Where(s => s.Activo).ToList();
            return View(lavado);
        }

        // Agregar servicio al lavado
        [HttpPost]
        public async Task<IActionResult> AgregarServicio(LavadoServicios model)
        {
            if (model.LavadoId == 0 || model.ServicioId == 0)
                return Json(new { success = false, message = "Datos incompletos" });

            // Validar que no exista el servicio en el lavado
            var existe = await _context.LavadoServicios
                .AnyAsync(ls => ls.LavadoId == model.LavadoId && ls.ServicioId == model.ServicioId);
            if (existe)
                return Json(new { success = false, message = "El servicio ya está agregado" });

            _context.LavadoServicios.Add(model);
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }

        // Editar precio de servicio
        [HttpPost]
        public async Task<IActionResult> EditarPrecio(int id, decimal precio)
        {
            var detalle = await _context.LavadoServicios.FindAsync(id);
            if (detalle == null) return Json(new { success = false, message = "No encontrado" });
            detalle.Precio = precio;
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }

        // Completar lavado
        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> Completar(int id)
        {
            var cajaAbierta = await _context.Caja
                .Where(c => c.Estado == "ABIERTA")
                .OrderByDescending(c => c.FechaApertura)
                .FirstOrDefaultAsync();

            if (cajaAbierta == null)
                return Json(new { success = false, message = "No hay caja abierta" });

            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC op.sp_CompletarLavado @LavadoId={0}, @CajaId={1}",
                    id, cajaAbierta.CajaId
                );

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // Listar lavados completados del día usando la vista
        public async Task<IActionResult> Completados()
        {
            var hoy = DateTime.Today;
            var lavados = await _context.Lavados_View
                .Where(l => l.Estado == "COMPLETADO" && l.FechaCierre != null && l.FechaCierre.Value.Date == hoy)
                .OrderByDescending(l => l.FechaCierre)
                .ToListAsync();
            return View(lavados);
        }
    }
}