
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebGuarida.Models;

public class DescuentosController : Controller
{
    private readonly GuaridaContext _context;

    public DescuentosController(GuaridaContext context)
    {
        _context = context;
    }

    // GET: DESCUENTOS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Descuento.ToListAsync());
    }

    // GET: DESCUENTOS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var descuento = await _context.Descuento
            .FirstOrDefaultAsync(m => m.Id == id);
        if (descuento == null)
        {
            return NotFound();
        }

        return View(descuento);
    }

    // GET: DESCUENTOS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: DESCUENTOS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Nombre,Porcentaje,FechaInicio,FechaFin,Descripcion,UsuarioRegistro,FechaRegistro,Estado,DescuentoDetalle,Venta")] Descuento descuento)
    {
        if (ModelState.IsValid)
        {
            _context.Add(descuento);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(descuento);
    }

    // GET: DESCUENTOS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var descuento = await _context.Descuento.FindAsync(id);
        if (descuento == null)
        {
            return NotFound();
        }
        return View(descuento);
    }

    // POST: DESCUENTOS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Nombre,Porcentaje,FechaInicio,FechaFin,Descripcion,UsuarioRegistro,FechaRegistro,Estado,DescuentoDetalle,Venta")] Descuento descuento)
    {
        if (id != descuento.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(descuento);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DescuentoExists(descuento.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(descuento);
    }

    // GET: DESCUENTOS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var descuento = await _context.Descuento
            .FirstOrDefaultAsync(m => m.Id == id);
        if (descuento == null)
        {
            return NotFound();
        }

        return View(descuento);
    }

    // POST: DESCUENTOS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var descuento = await _context.Descuento.FindAsync(id);
        if (descuento != null)
        {
            _context.Descuento.Remove(descuento);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool DescuentoExists(int? id)
    {
        return _context.Descuento.Any(e => e.Id == id);
    }
}
