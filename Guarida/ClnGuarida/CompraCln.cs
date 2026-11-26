using CadGuarida;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClnGuarida
{
    /// <summary>
    /// Fila resumida para listar las compras en una grilla.
    /// </summary>
    public class CompraListado
    {
        public int id { get; set; }
        public DateTime fecha { get; set; }
        public string proveedor { get; set; }
        public string moneda { get; set; }
        public decimal tipoCambio { get; set; }
        public decimal total { get; set; }
        public string estadoEnvio { get; set; }
        public string trackingNumber { get; set; }
        public DateTime? fechaEstimada { get; set; }
        public string observaciones { get; set; }
        public string usuarioRegistro { get; set; }
        public string situacion { get; set; }   // "Activa" o "Anulada"
    }

    /// <summary>
    /// Fila resumida del detalle de una compra.
    /// </summary>
    public class CompraDetalleListado
    {
        public int id { get; set; }
        public string codigo { get; set; }
        public string producto { get; set; }
        public int cantidad { get; set; }
        public decimal precioUnitario { get; set; }
        public decimal subtotal { get; set; }
    }

    public class CompraCln
    {
        public const string Pendiente = "Pendiente";
        public const string EnCamino = "En camino";
        public const string EnAduana = "En Aduana";
        public const string Recibido = "Recibido";
        public const string Cancelado = "Cancelado";

        public const string Usd = "USD";
        public const string Bob = "BOB";

        public static List<string> monedas()
        {
            return new List<string> { Usd, Bob };
        }

        /// <summary>
        /// Estados que se pueden elegir al registrar una compra nueva.
        /// </summary>
        public static List<string> estadosEnvio()
        {
            return new List<string> { Pendiente, EnCamino, EnAduana, Recibido };
        }

        /// <summary>
        /// Proveedores activos para el combo del formulario.
        /// </summary>
        public static List<Proveedor> listarProveedores()
        {
            using (var context = new GuaridaEntities())
            {
                return context.Proveedor
                    .Where(x => x.estado == (short)Estado.Activo)
                    .OrderBy(x => x.razonSocial)
                    .ToList();
            }
        }

        /// <summary>
        /// Guarda la cabecera (Compra) y todo el detalle (CompraDetalle) en una sola operación.
        /// Si la compra se registra directamente como "Recibido", el stock sube en el mismo momento.
        /// </summary>
        public static int crear(Compra compra, List<CompraDetalle> detalles)
        {
            if (detalles == null || detalles.Count == 0)
                throw new Exception("La compra debe tener al menos un producto.");
            if (compra.moneda != Usd && compra.moneda != Bob)
                throw new Exception("La moneda debe ser USD o BOB.");
            if (compra.moneda == Usd && compra.tipoCambio <= 0)
                throw new Exception("El tipo de cambio debe ser mayor a 0.");

            using (var context = new GuaridaEntities())
            {
                var proveedor = context.Proveedor.Find(compra.idProveedor);
                if (proveedor == null)
                    throw new Exception($"El proveedor con id {compra.idProveedor} no existe.");

                if (string.IsNullOrWhiteSpace(compra.estadoEnvio))
                    compra.estadoEnvio = Pendiente;
                if (compra.moneda == Bob)
                    compra.tipoCambio = 1;

                decimal subtotal = 0;
                foreach (var detalle in detalles)
                {
                    if (detalle.cantidad <= 0)
                        throw new Exception("Las cantidades deben ser mayores a 0.");
                    if (detalle.precioUnitario < 0)
                        throw new Exception("Los precios no pueden ser negativos.");

                    var producto = context.Producto.Find(detalle.idProducto);
                    if (producto == null)
                        throw new Exception($"El producto con id {detalle.idProducto} no existe.");

                    detalle.subtotal = Math.Round(detalle.cantidad * detalle.precioUnitario, 2);
                    detalle.usuarioRegistro = compra.usuarioRegistro;
                    detalle.fechaRegistro = DateTime.Now;
                    detalle.estado = (short)Estado.Activo;
                    subtotal += detalle.subtotal;

                    // Solo si la compra ya llegó, el stock sube ahora.
                    if (compra.estadoEnvio == Recibido)
                        producto.stockActual += detalle.cantidad;
                }

                compra.subtotal = subtotal;
                compra.total = subtotal;
                compra.fechaRegistro = DateTime.Now;
                compra.estado = (short)Estado.Activo;
                compra.CompraDetalle = detalles;

                context.Compra.Add(compra);
                context.SaveChanges();   // cabecera + detalle + stock en una sola transacción
                return compra.id;
            }
        }

        /// <summary>
        /// Cambia el estado de envío (Pendiente -> En camino -> En Aduana).
        /// Para marcar como recibida usar recibir(); para cancelar, anular().
        /// </summary>
        public static int cambiarEstadoEnvio(int id, string nuevoEstado, string usuario)
        {
            if (nuevoEstado == Recibido) return recibir(id, usuario);
            if (nuevoEstado == Cancelado) return anular(id, usuario);

            using (var context = new GuaridaEntities())
            {
                var compra = obtenerActiva(context, id);
                if (compra.estadoEnvio == Recibido)
                    throw new Exception("La compra ya fue recibida, no se puede cambiar su estado.");
                compra.estadoEnvio = nuevoEstado;
                compra.usuarioRegistro = usuario;
                return context.SaveChanges();
            }
        }

        /// <summary>
        /// Marca la compra como "Recibido" y SUBE el stock de cada producto del detalle.
        /// </summary>
        public static int recibir(int id, string usuario)
        {
            using (var context = new GuaridaEntities())
            {
                var compra = obtenerActiva(context, id);
                if (compra.estadoEnvio == Recibido)
                    throw new Exception("La compra ya fue recibida.");

                var detalles = context.CompraDetalle
                    .Where(x => x.idCompra == id && x.estado == (short)Estado.Activo)
                    .ToList();
                foreach (var detalle in detalles)
                {
                    var producto = context.Producto.Find(detalle.idProducto);
                    if (producto != null)
                        producto.stockActual += detalle.cantidad;
                }

                compra.estadoEnvio = Recibido;
                compra.usuarioRegistro = usuario;
                return context.SaveChanges();
            }
        }

        /// <summary>
        /// Anula la compra. Si ya había sido recibida, se revierte el stock que había sumado.
        /// </summary>
        public static int anular(int id, string usuario)
        {
            using (var context = new GuaridaEntities())
            {
                var compra = obtenerActiva(context, id);

                if (compra.estadoEnvio == Recibido)
                {
                    var detalles = context.CompraDetalle
                        .Where(x => x.idCompra == id && x.estado == (short)Estado.Activo)
                        .ToList();
                    foreach (var detalle in detalles)
                    {
                        var producto = context.Producto.Find(detalle.idProducto);
                        if (producto == null) continue;
                        if (producto.stockActual < detalle.cantidad)
                            throw new Exception($"No se puede anular: el stock actual de '{producto.nombre}' ({producto.stockActual}) " +
                                                $"es menor a lo que ingresó con esta compra ({detalle.cantidad}). Ya se vendió parte de esa mercadería.");
                        producto.stockActual -= detalle.cantidad;
                    }
                }

                compra.estado = (short)Estado.Inactivo;
                compra.estadoEnvio = Cancelado;
                compra.usuarioRegistro = usuario;
                return context.SaveChanges();
            }
        }

        public static List<CompraListado> listar(DateTime? fechaInicio, DateTime? fechaFin)
        {
            if (fechaInicio.HasValue && fechaFin.HasValue && fechaFin.Value.Date < fechaInicio.Value.Date)
                throw new Exception("La fecha final no puede ser menor a la fecha de inicio");

            using (var context = new GuaridaEntities())
            {
                var consulta = context.Compra.AsQueryable();
                if (fechaInicio.HasValue)
                {
                    var desde = fechaInicio.Value.Date;
                    consulta = consulta.Where(x => x.fecha >= desde);
                }
                if (fechaFin.HasValue)
                {
                    var hasta = fechaFin.Value.Date;
                    consulta = consulta.Where(x => x.fecha <= hasta);
                }

                return consulta
                    .OrderByDescending(x => x.id)
                    .Select(x => new CompraListado
                    {
                        id = x.id,
                        fecha = x.fecha,
                        proveedor = x.Proveedor.razonSocial,
                        moneda = x.moneda,
                        tipoCambio = x.tipoCambio,
                        total = x.total,
                        estadoEnvio = x.estadoEnvio,
                        trackingNumber = x.trackingNumber,
                        fechaEstimada = x.fechaEstimada,
                        observaciones = x.observaciones,
                        usuarioRegistro = x.usuarioRegistro,
                        situacion = x.estado == 1 ? "Activa" : "Anulada"
                    })
                    .ToList();
            }
        }

        public static List<CompraDetalleListado> listarDetalle(int idCompra)
        {
            using (var context = new GuaridaEntities())
            {
                return context.CompraDetalle
                    .Where(x => x.idCompra == idCompra)
                    .OrderBy(x => x.id)
                    .Select(x => new CompraDetalleListado
                    {
                        id = x.id,
                        codigo = x.Producto.codigo,
                        producto = x.Producto.nombre,
                        cantidad = x.cantidad,
                        precioUnitario = x.precioUnitario,
                        subtotal = x.subtotal
                    })
                    .ToList();
            }
        }

        private static Compra obtenerActiva(GuaridaEntities context, int id)
        {
            var compra = context.Compra.Find(id);
            if (compra == null) throw new Exception($"La compra con id {id} no existe");
            if (compra.estado == (short)Estado.Inactivo) throw new Exception("La compra está anulada");
            return compra;
        }
    }
}
