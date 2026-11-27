using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebGuarida.Models;
namespace WebGuarida.Controllers;

[Authorize]
public class VentasController : Controller
    {
        private readonly GuaridaContext _context;

        public VentasController(GuaridaContext context)
        {
            _context = context;
        }

      //busca en la base al usuario si esta activo y devuelve el usuario caso contrario 0
        private async Task<int> ObtenerIdUsuarioAsync()
        {
            var nombre = User.Identity?.Name;
            if (string.IsNullOrWhiteSpace(nombre)) return 0;

            var id = await _context.Usuario
                .Where(u => u.Usuario1 == nombre && u.Estado == (int)Estado.Activo)
                .Select(u => u.Id)
                .FirstOrDefaultAsync();

            return id;
        }

        private string ObtenerNombreUsuario()
        {
            return User.Identity?.Name ?? "sistema";
        }

        //  DESCUENTO ACTIVO: solo se valida Estado.Activo (global al subtotal)

        private async Task<Descuento?> ObtenerDescuentoActivo()
        {

           var hoy = DateOnly.FromDateTime(DateTime.Now);

            return await _context.Descuento
            .Where(d =>d.Estado == (int)Estado.Activo && d.FechaInicio <= hoy && d.FechaFin >= hoy)
            .FirstOrDefaultAsync();
    }


        //  LISTADO

        public async Task<IActionResult> Index()
        {
            var ventas = await _context.Venta
                .Include(v => v.IdClienteNavigation)
                .Include(v => v.IdUsuarioNavigation)
                .Where(x => x.Estado == (int)Estado.Activo || x.Estado == (int)Estado.Inactivo)
                .OrderByDescending(v => v.Fecha)
                .ToListAsync();
            return View(ventas);
        }

        //  PANTALLA DE VENTA
        public async Task<IActionResult> Create()
        {
            var descuento = await ObtenerDescuentoActivo();
            ViewBag.DescuentoId = descuento?.Id;
            ViewBag.DescuentoNombre = descuento?.Nombre;
            ViewBag.DescuentoPorcentaje = descuento?.Porcentaje ?? 0m;
            ViewBag.Usuario = ObtenerNombreUsuario();
            return View();
        }


        //  BUSCAR PRODUCTOS

        [HttpGet]
        public async Task<IActionResult> BuscarProductos(string? q)
        {
            var query = _context.Producto
                .AsNoTracking()
                .Where(p => p.Estado == (int)Estado.Activo);

            if (!string.IsNullOrWhiteSpace(q))
            {
                q = q.Trim();
                query = query.Where(p => p.Codigo.Contains(q)
                                      || p.Nombre.Contains(q)
                                      || (p.Descripcion != null && p.Descripcion.Contains(q))
                                      || p.IdCategoriaNavigation.Nombre.Contains(q));
            }

            var lista = await query
                .OrderBy(p => p.Nombre)
                .Take(60)
                .Select(p => new
                {
                    id = p.Id,
                    codigo = p.Codigo,
                    nombre = p.Nombre,
                    categoria = p.IdCategoriaNavigation.Nombre,
                    precio = p.PrecioVentaBob,
                    stock = p.StockActual,
                    imagenUrl = p.ImagenUrl
                })
                .ToListAsync();

            return Json(lista);
        }

        //  STOCK ACTUAL DE LOS PRODUCTOS DEL CARRITO
        //  GET /Ventas/StockProductos?ids=1&ids=2
        [HttpGet]
        public async Task<IActionResult> StockProductos([FromQuery] int[] ids)
        {
            if (ids == null || ids.Length == 0)
                return Json(new object[0]);

            var lista = await _context.Producto
                .AsNoTracking()
                .Where(p => ids.Contains(p.Id))
                .Select(p => new
                {
                    id = p.Id,
                    stock = p.StockActual,
                    precio = p.PrecioVentaBob,
                    activo = p.Estado == (int)Estado.Activo
                })
                .ToListAsync();

            return Json(lista);
        }

        //  BUSCAR CLIENTE POR CI/NIT 
        [HttpGet]
        public async Task<IActionResult> BuscarCliente(string ci)
        {
            if (string.IsNullOrWhiteSpace(ci))
                return Json(new { encontrado = false });

            ci = ci.Trim();
            var c = await _context.Cliente
                .FirstOrDefaultAsync(x => x.CedulaIdentidad == ci && x.Estado == (int)Estado.Activo);

            if (c == null)
                return Json(new { encontrado = false });

            return Json(new
            {
                encontrado = true,
                id = c.Id,
                ci = c.CedulaIdentidad,
                nombre = ((c.Nombres ?? "") + " " + (c.Apellidos ?? "")).Trim()
            });
        }

        //  CREAR CLIENTE RAPIDO 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearClienteRapido([FromBody] Cliente req)
        {
            if (req == null || string.IsNullOrWhiteSpace(req.CedulaIdentidad) || string.IsNullOrWhiteSpace(req.Nombres))
                return Json(new { ok = false, mensaje = "CI/NIT y nombres son obligatorios." });

            var ci = req.CedulaIdentidad.Trim();

            var existente = await _context.Cliente.FirstOrDefaultAsync(c => c.CedulaIdentidad == ci);
            if (existente != null)
            {
                return Json(new
                {
                    ok = false,
                    existe = true,
                    mensaje = "Ya existe un cliente con ese CI/NIT.",
                    id = existente.Id,
                    ci = existente.CedulaIdentidad,
                    nombre = ((existente.Nombres ?? "") + " " + (existente.Apellidos ?? "")).Trim()
                });
            }

            var cliente = new Cliente
            {
                CedulaIdentidad = ci,
                Nombres = req.Nombres.Trim(),
                Apellidos = string.IsNullOrWhiteSpace(req.Apellidos) ? null : req.Apellidos.Trim(),
                UsuarioRegistro = User.Identity?.Name,
                FechaRegistro = DateTime.Now,
                Estado = (int)Estado.Activo
            };

            _context.Cliente.Add(cliente);
            await _context.SaveChangesAsync();

            return Json(new
            {
                ok = true,
                id = cliente.Id,
                ci = cliente.CedulaIdentidad,
                nombre = ((cliente.Nombres ?? "") + " " + (cliente.Apellidos ?? "")).Trim()
            });
        }

        //  REGISTRAR VENTA  POST /Ventas/Registrar?recibido=0.00
        //  Todo se recalcula en el servidor (precios, descuento, total)

        [HttpPost]
        public async Task<IActionResult> Registrar([FromBody] Venta datos, decimal recibido = 0)
        {
            if (datos == null || datos.IdCliente <= 0)
                return Json(new { ok = false, mensaje = "Debe seleccionar un cliente." });

            if (datos.VentaDetalle == null || datos.VentaDetalle.Count == 0)
                return Json(new { ok = false, mensaje = "El carrito esta vacio." });

            if (datos.VentaDetalle.Any(i => i.Cantidad <= 0))
                return Json(new { ok = false, mensaje = "Hay cantidades invalidas." });

            var idUsuario = await ObtenerIdUsuarioAsync();
            if (idUsuario <= 0)
                return Json(new { ok = false, mensaje = "No se pudo identificar al usuario logueado." });

            var cliente = await _context.Cliente.FindAsync(datos.IdCliente);
            if (cliente == null || cliente.Estado != (int)Estado.Activo)
                return Json(new { ok = false, mensaje = "Cliente no valido." });

            await using var tx = await _context.Database.BeginTransactionAsync();
            try
            {
                // Unir items repetidos del mismo producto
                var agrupado = datos.VentaDetalle
                    .GroupBy(i => i.IdProducto)
                    .Select(g => new { IdProducto = g.Key, Cantidad = g.Sum(x => x.Cantidad) })
                    .ToList();

                var ids = agrupado.Select(a => a.IdProducto).ToList();
                var productos = await _context.Producto
                    .Where(p => ids.Contains(p.Id) && p.Estado == (int)Estado.Activo)
                    .ToDictionaryAsync(p => p.Id);

                var usuario = User.Identity?.Name;
                var detalles = new List<VentaDetalle>();
                decimal subtotal = 0;

                foreach (var a in agrupado)
                {
                    if (!productos.TryGetValue(a.IdProducto, out var prod))
                        return Json(new { ok = false, mensaje = "Un producto del carrito ya no esta disponible." });

                    if (a.Cantidad > prod.StockActual)
                        return Json(new { ok = false, mensaje = $"Stock insuficiente para '{prod.Nombre}'. Disponible: {prod.StockActual}." });

                    var sub = prod.PrecioVentaBob * a.Cantidad;
                    subtotal += sub;

                    prod.StockActual -= a.Cantidad;

                    detalles.Add(new VentaDetalle
                    {
                        IdProducto = prod.Id,
                        Cantidad = a.Cantidad,
                        PrecioUnitario = prod.PrecioVentaBob,
                        Subtotal = sub,
                        UsuarioRegistro = User.Identity?.Name,
                        FechaRegistro = DateTime.Now,
                        Estado = (int)Estado.Activo
                    });
                }

                // Descuento automatico
                var descuento = await ObtenerDescuentoActivo();
                decimal montoDescuento = 0;
                if (descuento != null)
                    montoDescuento = Math.Round(subtotal * descuento.Porcentaje / 100m, 2);

                var total = subtotal - montoDescuento;

                // Validacion de pago
                var metodo = string.IsNullOrWhiteSpace(datos.MetodoPago) ? "Efectivo" : datos.MetodoPago.Trim();
                if (metodo == "Efectivo" && recibido < total)
                    return Json(new { ok = false, mensaje = "El monto recibido es menor al total." });

                var venta = new Venta
                {
                    IdCliente = datos.IdCliente,
                    IdUsuario = idUsuario,
                    IdDescuento = descuento?.Id,
                    Fecha = DateTime.Now,
                    Subtotal = subtotal,
                    MontoDescuentoAplicado = montoDescuento,
                    Total = total,
                    MetodoPago = metodo,
                    Observaciones = string.IsNullOrWhiteSpace(datos.Observaciones) ? "sin observacion" : datos.Observaciones.Trim(),
                    UsuarioRegistro = usuario,
                    FechaRegistro = DateTime.Now,
                    Estado = (int)Estado.Activo,
                    VentaDetalle = detalles
                };

                _context.Venta.Add(venta);
                await _context.SaveChangesAsync();
                await tx.CommitAsync();

                return Json(new { ok = true, id = venta.Id });
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync();
                return Json(new { ok = false, mensaje = "Error al registrar la venta: " + ex.Message });
            }
        }


        //  DETALLE
        /*public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var venta = await _context.Venta
                .Include(v => v.IdClienteNavigation)
                .Include(v => v.IdUsuarioNavigation)
                .Include(v => v.IdDescuentoNavigation)
                .Include(v => v.VentaDetalle)
                    .ThenInclude(d => d.IdProductoNavigation)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (venta == null) return NotFound();

            ViewBag.PuedeAnular = venta.Estado == (int)Estado.Activo && (DateTime.Now - venta.Fecha).TotalHours < 24;
            return View(venta);
        }*/


        //  ANULAR (solo dentro de las primeras 24 horas) + devuelve stock
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Anular(int id)
        {
            var venta = await _context.Venta
                .Include(v => v.VentaDetalle)
                    .ThenInclude(d => d.IdProductoNavigation)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (venta == null) return NotFound();

            if (venta.Estado == (int)Estado.Inactivo)
            {
                TempData["Error"] = "La venta ya esta anulada.";
                return RedirectToAction(nameof(Index));
            }

            if (DateTime.Now >= venta.Fecha.AddHours(24))
        {
                TempData["Error"] = "Solo se puede anular una venta dentro de las primeras 24 horas.";
                return RedirectToAction(nameof(Index));
            }

            await using var tx = await _context.Database.BeginTransactionAsync();
            try
            {
                foreach (var d in venta.VentaDetalle)
                {
                    d.IdProductoNavigation.StockActual += d.Cantidad;
                    d.Estado = (int)Estado.Inactivo;
                }

                venta.Estado = (int)Estado.Inactivo;

                await _context.SaveChangesAsync();
                await tx.CommitAsync();

                TempData["Ok"] = "Venta anulada y stock devuelto.";
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync();
                TempData["Error"] = "No se pudo anular: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }