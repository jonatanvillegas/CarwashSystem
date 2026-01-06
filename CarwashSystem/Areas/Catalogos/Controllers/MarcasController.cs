using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UI.Models;

namespace UI.Areas.Catalogos.Controllers
{
    [Area("Catalogos")]
    public class MarcasController : Controller
    {
        private readonly CarwashSystemContext _context;

        public MarcasController(CarwashSystemContext context)
        {
            _context = context;
        }

        // GET: Catalogos/Marcas
        public async Task<IActionResult> Index()
        {
            var marcas = await _context.Marcas
                .OrderByDescending(m => m.FechaCreacion)
                .ToListAsync();
            return View(marcas);
        }

        // GET: Catalogos/Marcas/Create
        public IActionResult Create()
        {
            ViewBag.TiposVehiculo = _context.TiposVehiculo.ToList();
            return PartialView("Partial/_CreateEdit", new Marcas());
        }

        // POST: Catalogos/Marcas/Create
        [HttpPost]
        public async Task<IActionResult> Create(Marcas marca)
        {
            if (ModelState.IsValid)
            {
                marca.Activo = true;
                marca.FechaCreacion = DateTime.Now;
                _context.Add(marca);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            ViewBag.TiposVehiculo = _context.TiposVehiculo.ToList();
            return PartialView("Partial/_CreateEdit", marca);
        }

        // GET: Catalogos/Marcas/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var marca = await _context.Marcas.FindAsync(id);
            if (marca == null) return NotFound();
            ViewBag.TiposVehiculo = _context.TiposVehiculo.ToList();
            return PartialView("Partial/_CreateEdit", marca);
        }

        // POST: Catalogos/Marcas/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(Marcas marca)
        {
            if (ModelState.IsValid)
            {
                var original = await _context.Marcas.FindAsync(marca.Id);
                if (original == null) return NotFound();

                original.Nombre = marca.Nombre;
                original.TipoVehiculoId = marca.TipoVehiculoId;
                // No modificar FechaCreacion ni Activo

                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            ViewBag.TiposVehiculo = _context.TiposVehiculo.ToList();
            return PartialView("Partial/_CreateEdit", marca);
        }

        // POST: Catalogos/Marcas/Desactivar/5
        [HttpPost]
        public async Task<IActionResult> Desactivar(int id)
        {
            var marca = await _context.Marcas.FindAsync(id);
            if (marca == null) return NotFound();
            marca.Activo = false;
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> GetByTipoVehiculo(int tipoVehiculoId)
        {
            var marcas = await _context.Marcas
                .Where(m => m.Activo && m.TipoVehiculoId == tipoVehiculoId)
                .Select(m => new { id = m.Id, nombre = m.Nombre })
                .ToListAsync();
            return Json(marcas);
        }
    }
}