using System;
using System.Collections.Generic;

namespace WebGuarida.Models;

public partial class Venta
{
    public int Id { get; set; }

    public int IdCliente { get; set; }

    public int IdUsuario { get; set; }

    public int? IdDescuento { get; set; }

    public int? Transaccion { get; set; }

    public DateTime Fecha { get; set; }

    public decimal Subtotal { get; set; }

    public decimal MontoDescuentoAplicado { get; set; }

    public decimal Total { get; set; }

    public string MetodoPago { get; set; } = null!;

    public string? Observaciones { get; set; }

    public string UsuarioRegistro { get; set; } = null!;

    public DateTime FechaRegistro { get; set; }

    public short Estado { get; set; }

    public virtual Cliente IdClienteNavigation { get; set; } = null!;

    public virtual Descuento? IdDescuentoNavigation { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;

    public virtual ICollection<VentaDetalle> VentaDetalle { get; set; } = new List<VentaDetalle>();
}
