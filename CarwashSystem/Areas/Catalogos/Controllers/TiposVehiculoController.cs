using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UI.Models;

namespace UI.Areas.Catalogos.Controllers
{
    [Area("Catalogos")]
    public class TiposVehiculoController : Controller
    {
        private readonly CarwashSystemContext _context;

        public TiposVehiculoController(CarwashSystemContext context)
        {
            _context = context;
        }

        // GET: Catalogos/TiposVehiculo
        public async Task<IActionResult> Index()
        {
            var tipos = await _context.TiposVehiculo
                .OrderByDescending(t => t.Id)
                .ToListAsync();
            return View(tipos);
        }

        // GET: Catalogos/TiposVehiculo/Create
        public IActionResult Create()
        {
            return PartialView("Partial/_CreateEdit", new TiposVehiculo());
        }

        // POST: Catalogos/TiposVehiculo/Create
        [HttpPost]
        public async Task<IActionResult> Create(TiposVehiculo tipo)
        {
            if (ModelState.IsValid)
            {
                tipo.Activo = true;
                _context.Add(tipo);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            return PartialView("Partial/_CreateEdit", tipo);
        }

        // GET: Catalogos/TiposVehiculo/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var tipo = await _context.TiposVehiculo.FindAsync(id);
            if (tipo == null) return NotFound();
            return PartialView("Partial/_CreateEdit", tipo);
        }

        // POST: Catalogos/TiposVehiculo/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(TiposVehiculo tipo)
        {
            if (ModelState.IsValid)
            {
                var original = await _context.TiposVehiculo.FindAsync(tipo.Id);
                if (original == null) return NotFound();

                original.Nombre = tipo.Nombre;
                // No modificar Activo

                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            return PartialView("Partial/_CreateEdit", tipo);
        }

        // POST: Catalogos/TiposVehiculo/Desactivar/5
        [HttpPost]
        public async Task<IActionResult> Desactivar(int id)
        {
            var tipo = await _context.TiposVehiculo.FindAsync(id);
            if (tipo == null) return NotFound();
            tipo.Activo = false;
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }
    }
}