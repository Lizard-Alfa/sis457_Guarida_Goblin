
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebGuarida;
using WebGuarida.Models;

public class VentaDetallesController : Controller
{
    private readonly GuaridaContext _context;

    public VentaDetallesController(GuaridaContext context)
    {
        _context = context;
    }

    // GET: VENTADETALLES
    public async Task<IActionResult> Index(int? id)    
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
        return View(venta);

    }

    
}
