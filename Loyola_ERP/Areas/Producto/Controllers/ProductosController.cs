using Loyola_ERP.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;
using System.Linq;
using UI.Models;
using UI.Services;
using static System.Net.Mime.MediaTypeNames;
using Loyola_ERP.Models;
using System.IO;

namespace UI.Areas.Producto.Controllers
{
    [Area("Producto")]
    public class ProductosController : Controller
    {
        private readonly TiendaProductosContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IProductoService _productoService;

        public ProductosController(TiendaProductosContext context, IWebHostEnvironment hostEnvironment, IProductoService productoService)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _productoService = productoService;
        }
        public IActionResult ListadoProductos()
        {
            return View();
        }
        public async Task<IActionResult> Producto(int productoId = 0)
        {
            var producto = await _context.Productos.FirstOrDefaultAsync(c => c.Id == productoId);
            if (producto == null)
            {
                producto = new Productos();
            }

            // Si tenemos un producto, buscar la última imagen asociada y pasar la URL para servirla
            if (producto.Id != 0)
            {
                var imagen = await _context.ProductoImagen
                    .Where(i => i.ProductoId == producto.Id)
                    .OrderByDescending(i => i.CreadoEn)
                    .FirstOrDefaultAsync();

                if (imagen != null)
                {
                    ViewBag.ExistingImageUrl = Url.Action("ObtenerImagenProducto", "Productos", new { area = "Producto", productoId = producto.Id });
                    ViewBag.ExistingImageName = imagen.NombreArchivo ?? "imagen.jpg";
                    ViewBag.ExistingImageSize = imagen.Tamano;
                }
            }

            return View(producto);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerImagenProducto(int productoId)
        {
            var imagen = await _context.ProductoImagen
                .Where(i => i.ProductoId == productoId)
                .OrderByDescending(i => i.CreadoEn)
                .FirstOrDefaultAsync();

            if (imagen == null || imagen.Contenido == null)
                return NotFound();

            return File(imagen.Contenido, imagen.ContentType ?? "image/jpeg");
        }

        [HttpPost]
        public async Task<IActionResult> GuardarProducto(Productos model, IFormFile imagenFile)
        {
            try
            {
                bool esNuevo = model.Id == 0;

                // Si es edición, preservar la referencia de imagen si el formulario no la envió.
                if (!esNuevo)
                {
                    var productoExistente = await _context.Productos
                        .AsNoTracking()
                        .FirstOrDefaultAsync(p => p.Id == model.Id);

                    if (productoExistente == null)
                    {
                        return NotFound(new { mensaje = "Producto no encontrado" });
                    }

                    // El binder no envía el campo Imagen desde el formulario, por eso lo preservamos.
                    if (string.IsNullOrEmpty(model.Imagen))
                    {
                        model.Imagen = productoExistente.Imagen;
                    }
                }

                // 1️⃣ Guardar producto (crea o edita)
                await _productoService.Guardar(model);

                // 2️⃣ Procesar imagen (si existe)
                if (imagenFile != null && imagenFile.Length > 0)
                {
                    using var image = SixLabors.ImageSharp.Image.Load(imagenFile.OpenReadStream());

                    image.Mutate(x => x.Resize(new ResizeOptions
                    {
                        Mode = ResizeMode.Max,
                        Size = new Size(800, 800)
                    }));

                    var encoder = new JpegEncoder { Quality = 75 };

                    using var ms = new MemoryStream();
                    await image.SaveAsync(ms, encoder);
                    var imageBytes = ms.ToArray();

                    // ❗ Si es edición → eliminar la imagen anterior (la más reciente)
                    if (!esNuevo)
                    {
                        var imagenAnterior = await _context.ProductoImagen
                            .Where(x => x.ProductoId == model.Id)
                            .OrderByDescending(x => x.CreadoEn)
                            .FirstOrDefaultAsync();

                        if (imagenAnterior != null)
                            _context.ProductoImagen.Remove(imagenAnterior);
                    }

                    var imagenEntity = new ProductoImagen
                    {
                        ProductoId = model.Id,
                        NombreArchivo = Path.GetFileName(imagenFile.FileName),
                        ContentType = "image/jpeg",
                        Tamano = imageBytes.LongLength,
                        Ancho = image.Width,
                        Alto = image.Height,
                        Contenido = imageBytes,
                        CreadoEn = DateTime.UtcNow
                    };

                    _context.ProductoImagen.Add(imagenEntity);

                    // Actualizar referencia en el producto (la entidad model está siendo rastreada por el mismo contexto)
                    model.Imagen = imagenEntity.NombreArchivo;
                }

                // 3️⃣ Guardar todo de una sola vez (producto ya fue guardado por el servicio, aquí se guardan cambios de imagen / referencia)
                await _context.SaveChangesAsync();

                return Json(new
                {
                    ok = true,
                    mensaje = esNuevo ? "Producto creado correctamente" : "Producto actualizado correctamente"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    ok = false,
                    mensaje = "Error al guardar el producto",
                    detalle = ex.Message
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ListarProductos()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ObtenerProductos(string nombreBusqueda, string estadoBusqueda)
        {
            try
            {
                var query = _context.ProductoView.AsQueryable();

                if (!string.IsNullOrWhiteSpace(nombreBusqueda))
                {
                    query = query.Where(p => p.nombre.Contains(nombreBusqueda));
                }

                if (!string.IsNullOrWhiteSpace(estadoBusqueda))
                {
                    query = query.Where(p => p.estadoNombre == estadoBusqueda);
                }

                return Json(query.ToList());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Ocurrió un error al obtener los productos: " + ex.Message });
            }
        }

        [HttpGet]
        public IActionResult ObtenerEstados()
        {
            try
            {
                int[] estadosId = { 1, 2 }; // IDs permitidos

                var estados = _context.Estados
                    .Where(e => estadosId.Contains(e.Id))
                    .OrderBy(e => e.Nombre)
                    .Select(e => new {
                        id = e.Id,
                        nombre = e.Nombre
                    })
                    .ToList();

                return Json(estados);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

    }
}