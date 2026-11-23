using System;
using System.Collections.Generic;

namespace WebGuarida.Models;

public partial class Compra
{
    public int Id { get; set; }

    public int IdProveedor { get; set; }

    public DateOnly Fecha { get; set; }

    public decimal Subtotal { get; set; }

    public decimal Total { get; set; }

    public string? TrackingNumber { get; set; }

    public string? EstadoEnvio { get; set; }

    public DateOnly? FechaEstimada { get; set; }

    public string? Observaciones { get; set; }

    public string UsuarioRegistro { get; set; } = null!;

    public DateTime FechaRegistro { get; set; }

    public short Estado { get; set; }

    public virtual ICollection<CompraDetalle> CompraDetalle { get; set; } = new List<CompraDetalle>();

    public virtual Proveedor IdProveedorNavigation { get; set; } = null!;
}
