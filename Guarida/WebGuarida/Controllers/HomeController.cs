using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebGuarida.Models;

namespace WebGuarida.Controllers
{
    public class HomeController : Controller
    {
        private readonly GuaridaContext _context;
        private const int ItemsPorPagina = 20;

        public HomeController(GuaridaContext context) => _context = context;

        // PÚBLICO: Inicio con novedades
        public async Task<IActionResult> Index()
        {
            var novedades = await _context.Producto
                .Where(p => p.Estado == (int)Estado.Activo)
                .OrderByDescending(p => p.FechaRegistro)
                .Take(8).ToListAsync();
            ViewData["Novedades"] = novedades;
            return View();
        }

        // PÚBLICO: Catálogo
        public async Task<IActionResult> Catalogo(int? categoria, int pagina = 1)
        {
            var categorias = await _context.Categoria
                .Where(c => c.Estado == (int)Estado.Activo)
                .OrderBy(c => c.Nombre).ToListAsync();

            var query = _context.Producto.Where(p => p.Estado == (int)Estado.Activo);
            if (categoria.HasValue) query = query.Where(p => p.IdCategoria == categoria);

            var total = await query.CountAsync();
            var totalPaginas = Math.Max(1, (int)Math.Ceiling(total / (double)ItemsPorPagina));
            pagina = Math.Clamp(pagina, 1, totalPaginas);

            var productos = await query.OrderBy(p => p.Nombre)
                .Skip((pagina - 1) * ItemsPorPagina).Take(ItemsPorPagina).ToListAsync();

            return View(new CatalogoViewModel
            {
                Productos = productos,
                Categorias = categorias,
                CategoriaSeleccionada = categoria,
                PaginaActual = pagina,
                TotalPaginas = totalPaginas,
                TotalProductos = total
            });
        }

        public IActionResult Privacy() => View();

        // PRIVADO (requiere login)
        [Authorize]
        public IActionResult Dashboard() => View();
    }
}