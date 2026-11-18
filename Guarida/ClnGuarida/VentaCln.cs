using CadGuarida;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClnGuarida
{
    public class VentaCln
    {
       public static int crear(Venta venta, List<VentaDetalle> detalles)
       {
            using (var context = new GuaridaEntities())
            {
                int ultimaTransaccion = context.Venta.Max(v => (int?)v.transaccion) ?? 0;
                venta.transaccion = ultimaTransaccion + 1;
                foreach (var detalle in detalles)
                {
                    var producto = context.Producto.Find(detalle.idProducto);
                    if (producto == null)
                        throw new Exception($"El producto con ID {detalle.idProducto} no existe.");

                    if (producto.stockActual < detalle.cantidad)
                        throw new Exception($"No hay suficiente stock para {producto.nombre}. Stock actual: {producto.stockActual}");
                }
                foreach (var detalle in detalles)
                {
                    var producto = context.Producto.Find(detalle.idProducto);
                    if (producto != null)
                    {
                        producto.stockActual -= detalle.cantidad;
                    }

                    detalle.fechaRegistro = DateTime.Now;
                    detalle.estado = 1;
                }
                venta.VentaDetalle = detalles;
                context.Venta.Add(venta);
                context.SaveChanges();

                return venta.id;
            }
        }
       public static List<string> metodPago()
        {
            return new List<string> { "Efectivo", "Tarjeta", "QR" };
        }

        public static int anular(int id, string usuario)
        {
            using (var context = new GuaridaEntities())
            {
                var venta = context.Venta.Find(id);
                if (venta == null) throw new Exception($"La venta con id {id} no existe");
                if (venta.estado == (short)Estado.Inactivo) throw new Exception("La venta ya está anulada");
                var detalles = context.VentaDetalle.Where(x => x.idVenta == id).ToList();
                foreach (var detalle in detalles)
                {
                    var producto = context.Producto.Find(detalle.idProducto);
                    if (producto != null)
                    {
                        producto.stockActual += detalle.cantidad;
                    }
                }
                venta.estado = (short)Estado.Inactivo;
                venta.usuarioRegistro = usuario;
                return context.SaveChanges();
            }
        }
        public static Venta obtenerUno(int id)
        {
            using (var context = new GuaridaEntities())
            {
                var venta = context.Venta.Find(id);
                if (venta == null) throw new Exception($"La venta con id {id} no existe");
                return venta;
            }
        }
        public static List<Venta> listar()
        {
            using (var context = new GuaridaEntities())
            {
                return context.Venta.Where(x => x.estado == (short)Estado.Activo).OrderBy( x => x.fechaRegistro).ToList();
            }
        }
        public static List<paVentaListar_Result> listarPa(DateTime? fechaInicio, DateTime? fechaFin)
        {
            if (fechaInicio.HasValue && fechaFin.HasValue && fechaFin.Value.Date < fechaInicio.Value.Date) 
                throw new Exception("La fecha final no puede ser menor a la fecha de inicio");

            using (var context = new GuaridaEntities())
            {
                return context.paVentaListar(fechaInicio, fechaFin).ToList();
            }
        }
        public static string cargoUsuario(int idEmpleado)
        {
            using (var context = new GuaridaEntities())
            {
                return context.Empleado
                    .Where(e => e.id == idEmpleado)   // <-- aquí va "e.id"
                    .Select(e => e.cargo)
                    .FirstOrDefault() ?? "S/N";
            }
        }
    }

}
