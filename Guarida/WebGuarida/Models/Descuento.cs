using System;
using System.Collections.Generic;

namespace WebGuarida.Models;

public partial class Descuento
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public decimal Porcentaje { get; set; }

    public DateOnly? FechaInicio { get; set; }

    public DateOnly? FechaFin { get; set; }

    public string? Descripcion { get; set; }

    public string UsuarioRegistro { get; set; } = null!;

    public DateTime FechaRegistro { get; set; }

    public short Estado { get; set; }

    public virtual ICollection<DescuentoDetalle> DescuentoDetalle { get; set; } = new List<DescuentoDetalle>();

    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();
}
