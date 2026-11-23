using System;
using System.Collections.Generic;

namespace WebGuarida.Models;

public partial class Cliente
{
    public int Id { get; set; }

    public string? CedulaIdentidad { get; set; }

    public string Nombres { get; set; } = null!;

    public string? Apellidos { get; set; }

    public string? Ciudad { get; set; }

    public string UsuarioRegistro { get; set; } = null!;

    public DateTime FechaRegistro { get; set; }

    public short Estado { get; set; }

    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();
}
