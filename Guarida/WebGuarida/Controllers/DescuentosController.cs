
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebGuarida;
using WebGuarida.Models;

[Authorize]
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
        await ActualizarEstadosVencidosAsync();
        return View(await _context.Descuento
            .OrderBy(x => x.FechaRegistro)
            .Where(x => x.Estado == (int)Estado.Activo || x.Estado == (int)Estado.Inactivo)
            .ToListAsync());

    }

    // GET: DESCUENTOS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        await ActualizarEstadosVencidosAsync();
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
    public async Task<IActionResult> Create(Descuento descuento)
    {

        if (ValidarCampos(descuento))
        {
            try
            {
                descuento.UsuarioRegistro = User.Identity?.Name;
                descuento.FechaRegistro = DateTime.Now;
                descuento.Estado = (int)Estado.Activo;

                _context.Add(descuento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error al guardar: {ex.Message}");
            }
        }

        return View(descuento);
    }

    // GET: DESCUENTOS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        await ActualizarEstadosVencidosAsync();
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
    public async Task<IActionResult> Edit(int? id, Descuento descuento)
    {
        if (id != descuento.Id)
        {
            return NotFound();
        }

        if (ValidarCampos(descuento, descuento.Id))
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
            descuento.Estado = (int)Estado.Eliminado;
          
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool DescuentoExists(int? id)
    {
        return _context.Descuento.Any(e => e.Id == id);
    }
    private async Task ActualizarEstadosVencidosAsync()
    {
        var hoy = DateOnly.FromDateTime(DateTime.Now);

        var descuentosVencidos = await _context.Descuento
            .Where(d => d.Estado == (int)Estado.Activo && d.FechaFin < hoy)
            .ToListAsync();

        if (descuentosVencidos.Any())
        {
            foreach (var d in descuentosVencidos)
            {
                d.Estado = (int)Estado.Inactivo;
            }
            await _context.SaveChangesAsync();
        }
    }
    private void ResetearErrores()
    {
        ModelState.Clear();
    }

    private bool ValidarCampos(Descuento descuento, int? idActual = null)
    {
        bool esValido = true;
        ResetearErrores();

        if (string.IsNullOrWhiteSpace(descuento.Nombre))
        {
            ModelState.AddModelError(nameof(descuento.Nombre), "El nombre de la promoción es obligatorio.");
            esValido = false;
        }

        if (descuento.Porcentaje <= 0 || descuento.Porcentaje > 100)
        {
            ModelState.AddModelError(nameof(descuento.Porcentaje), "El porcentaje debe ser mayor a 0 y hasta 100.");
            esValido = false;
        }

        if (descuento.FechaInicio == default)
        {
            ModelState.AddModelError(nameof(descuento.FechaInicio), "La fecha de inicio es obligatoria.");
            esValido = false;
        }

        if (descuento.FechaFin == default)
        {
            ModelState.AddModelError(nameof(descuento.FechaFin), "La fecha de fin es obligatoria.");
            esValido = false;
        }
        else if (descuento.FechaInicio != default && descuento.FechaFin < descuento.FechaInicio)
        {
            ModelState.AddModelError(nameof(descuento.FechaFin), "La fecha fin no puede ser menor a la fecha inicio.");
            esValido = false;
        }

        if (descuento.FechaInicio != default && descuento.FechaFin != default)
        {
            bool haySolapamiento = _context.Descuento
                .Any(d => d.Estado == (int)Estado.Activo
                       && d.FechaInicio != default
                       && d.FechaFin != default
                       && d.FechaInicio <= descuento.FechaFin
                       && descuento.FechaInicio <= d.FechaFin
                       && (!idActual.HasValue || d.Id != idActual.Value));

            if (haySolapamiento)
            {
                ModelState.AddModelError(nameof(descuento.FechaFin),
                    "Ya existe un descuento activo que se cruza con ese rango de fechas.");
                esValido = false;
            }
        }

        return esValido;
    }
}

