using System;
using System.Collections.Generic;

namespace WebGuarida.Models;

public partial class Proveedor
{
    public int Id { get; set; }

    public string Tipo { get; set; } = null!;

    public string RazonSocial { get; set; } = null!;

    public string? Nit { get; set; }

    public string? Contacto { get; set; }

    public string? Email { get; set; }

    public string? Telefono { get; set; }

    public string? Pais { get; set; }

    public string? Ciudad { get; set; }

    public string? Direccion { get; set; }

    public string UsuarioRegistro { get; set; } = null!;

    public DateTime FechaRegistro { get; set; }

    public short Estado { get; set; }

    public virtual ICollection<Compra> Compra { get; set; } = new List<Compra>();
}
