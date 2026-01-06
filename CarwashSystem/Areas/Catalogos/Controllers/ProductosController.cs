using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UI.Models;

namespace UI.Areas.Catalogos.Controllers
{
    [Area("Catalogos")]
    public class ProductosController : Controller
    {
        private readonly CarwashSystemContext _context;

        public ProductosController(CarwashSystemContext context)
        {
            _context = context;
        }

        // GET: Catalogos/Productos
        public async Task<IActionResult> Index()
        {
            var productos = await _context.Productos
                .OrderByDescending(p => p.FechaCreacion)
                .ToListAsync();
            return View(productos);
        }

        // GET: Catalogos/Productos/Create
        public IActionResult Create()
        {
            return PartialView("Partial/_CreateEdit", new Productos());
        }

        // POST: Catalogos/Productos/Create
        [HttpPost]
        public async Task<IActionResult> Create(Productos producto)
        {
            if (ModelState.IsValid)
            {
                producto.Activo = true;
                producto.FechaCreacion = DateTime.Now;
                _context.Add(producto);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            return PartialView("Partial/_CreateEdit", producto);
        }

        // GET: Catalogos/Productos/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null) return NotFound();
            return PartialView("Partial/_CreateEdit", producto);
        }

        // POST: Catalogos/Productos/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(Productos producto)
        {
            if (ModelState.IsValid)
            {
                var original = await _context.Productos.FindAsync(producto.Id);
                if (original == null) return NotFound();

                original.Nombre = producto.Nombre;
                original.Precio = producto.Precio;
                // No modificar FechaCreacion ni Activo

                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            return PartialView("Partial/_CreateEdit", producto);
        }

        // POST: Catalogos/Productos/Desactivar/5
        [HttpPost]
        public async Task<IActionResult> Desactivar(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null) return NotFound();
            producto.Activo = false;
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }
    }
}