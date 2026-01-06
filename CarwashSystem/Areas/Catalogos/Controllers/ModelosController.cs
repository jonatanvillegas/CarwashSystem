using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UI.Models;

namespace UI.Areas.Catalogos.Controllers
{
    [Area("Catalogos")]
    public class ModelosController : Controller
    {
        private readonly CarwashSystemContext _context;

        public ModelosController(CarwashSystemContext context)
        {
            _context = context;
        }

        // GET: Catalogos/Modelos
        public async Task<IActionResult> Index()
        {
            var modelos = await _context.Modelos
                .Include(m => m.Marca)
                .OrderByDescending(m => m.Id)
                .ToListAsync();
            return View(modelos);
        }

        // GET: Catalogos/Modelos/Create
        public IActionResult Create()
        {
            ViewBag.Marcas = _context.Marcas.Where(m => m.Activo).ToList();
            return PartialView("Partial/_CreateEdit", new Modelos());
        }

        // POST: Catalogos/Modelos/Create
        [HttpPost]
        public async Task<IActionResult> Create(Modelos modelo)
        {
            if (ModelState.IsValid)
            {
                modelo.Activo = true;
                _context.Add(modelo);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            ViewBag.Marcas = _context.Marcas.Where(m => m.Activo).ToList();
            return PartialView("Partial/_CreateEdit", modelo);
        }

        // GET: Catalogos/Modelos/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var modelo = await _context.Modelos.FindAsync(id);
            if (modelo == null) return NotFound();
            ViewBag.Marcas = _context.Marcas.Where(m => m.Activo).ToList();
            return PartialView("Partial/_CreateEdit", modelo);
        }

        // POST: Catalogos/Modelos/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(Modelos modelo)
        {
            if (ModelState.IsValid)
            {
                var original = await _context.Modelos.FindAsync(modelo.Id);
                if (original == null) return NotFound();

                original.Nombre = modelo.Nombre;
                original.MarcaId = modelo.MarcaId;
                // No modificar Activo aquí

                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            ViewBag.Marcas = _context.Marcas.Where(m => m.Activo).ToList();
            return PartialView("Partial/_CreateEdit", modelo);
        }

        // POST: Catalogos/Modelos/Desactivar/5
        [HttpPost]
        public async Task<IActionResult> Desactivar(int id)
        {
            var modelo = await _context.Modelos.FindAsync(id);
            if (modelo == null) return NotFound();
            modelo.Activo = false;
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> GetByMarca(int marcaId)
        {
            var modelos = await _context.Modelos
                .Where(m => m.Activo && m.MarcaId == marcaId)
                .Select(m => new { id = m.Id, nombre = m.Nombre })
                .ToListAsync();
            return Json(modelos);
        }
    }
}
