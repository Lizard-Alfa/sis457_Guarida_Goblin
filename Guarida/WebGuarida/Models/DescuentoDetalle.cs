using System;
using System.Collections.Generic;

namespace WebGuarida.Models;

public partial class DescuentoDetalle
{
    public int Id { get; set; }

    public int IdDescuento { get; set; }

    public int IdProducto { get; set; }

    public string UsuarioRegistro { get; set; } = null!;

    public DateTime FechaRegistro { get; set; }

    public short Estado { get; set; }

    public virtual Descuento IdDescuentoNavigation { get; set; } = null!;

    public virtual Producto IdProductoNavigation { get; set; } = null!;
}
