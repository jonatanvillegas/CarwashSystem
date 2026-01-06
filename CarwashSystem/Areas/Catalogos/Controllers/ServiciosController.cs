using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UI.Models;

namespace UI.Areas.Catalogos.Controllers
{
    [Area("Catalogos")]
    public class ServiciosController : Controller
    {
        private readonly CarwashSystemContext _context;

        public ServiciosController(CarwashSystemContext context)
        {
            _context = context;
        }

        // GET: Catalogos/Servicios
        public async Task<IActionResult> Index()
        {
            var servicios = await _context.Servicios
                .OrderByDescending(s => s.FechaCreacion)
                .ToListAsync();
            return View(servicios);
        }

        // GET: Catalogos/Servicios/Create
        public IActionResult Create()
        {
            return PartialView("Partial/_CreateEdit", new Servicios());
        }

        // POST: Catalogos/Servicios/Create
        [HttpPost]
        public async Task<IActionResult> Create(Servicios servicio)
        {
            if (ModelState.IsValid)
            {
                servicio.Activo = true;
                servicio.FechaCreacion = DateTime.Now;
                _context.Add(servicio);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            return PartialView("Partial/_CreateEdit", servicio);
        }

        // GET: Catalogos/Servicios/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var servicio = await _context.Servicios.FindAsync(id);
            if (servicio == null) return NotFound();
            return PartialView("Partial/_CreateEdit", servicio);
        }

        // POST: Catalogos/Servicios/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(Servicios servicio)
        {
            if (ModelState.IsValid)
            {
                var original = await _context.Servicios.FindAsync(servicio.Id);
                if (original == null) return NotFound();

                original.Nombre = servicio.Nombre;
                original.Descripcion = servicio.Descripcion;

                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            return PartialView("Partial/_CreateEdit", servicio);
        }

        // POST: Catalogos/Servicios/Desactivar/5
        [HttpPost]
        public async Task<IActionResult> Desactivar(int id)
        {
            var servicio = await _context.Servicios.FindAsync(id);
            if (servicio == null) return NotFound();
            servicio.Activo = false;
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }
    }
}

