using System;
using System.Collections.Generic;

namespace WebGuarida.Models;

public partial class Producto
{
    public int Id { get; set; }
    public int IdCategoria { get; set; }
    public string Codigo { get; set; } = null!;
    public string? ImagenUrl { get; set; }
    public string Nombre { get; set; } = null!;
    public string? Descripcion { get; set; }
    public decimal? PesoKg { get; set; }
    public decimal? PrecioCostoUsd { get; set; }
    public decimal PrecioVentaBob { get; set; }
    public int StockActual { get; set; }
    public int StockMinimo { get; set; }
    public string UsuarioRegistro { get; set; } = null!;
    public DateTime FechaRegistro { get; set; }
    public short Estado { get; set; }

    public virtual ICollection<CompraDetalle> CompraDetalle { get; set; } = new List<CompraDetalle>();
    public virtual ICollection<DescuentoDetalle> DescuentoDetalle { get; set; } = new List<DescuentoDetalle>();

    public virtual Categoria IdCategoriaNavigation { get; set; } = null!;

    public virtual ICollection<VentaDetalle> VentaDetalle { get; set; } = new List<VentaDetalle>();
}
