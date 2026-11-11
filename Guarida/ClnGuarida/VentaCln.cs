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
                var existe = context.Venta.Where(x => x.transaccion == venta.transaccion).FirstOrDefault();
                if (existe != null) throw new Exception($"La venta {venta.transaccion} ya existe");
                context.Venta.Add(venta);
                context.SaveChanges();
                foreach (var detalle in detalles)
                {
                    var producto = context.Producto.Find(detalle.idProducto);

                    if (producto != null)
                    {
                        if (producto.stockActual < detalle.cantidad)
                        {
                            throw new Exception($"No hay suficiente stock para el producto {producto.nombre}. Stock actual: {producto.stockActual}");
                        }
                        producto.stockActual = producto.stockActual - detalle.cantidad;
                    }
                    detalle.idVenta = venta.id;
                    context.VentaDetalle.Add(detalle);
                }
                return context.SaveChanges();
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
                if (venta.estado != (short)Estado.Activo) throw new Exception("La venta ya está anulada");

                foreach (var d in context.VentaDetalle.Where(x => x.idVenta == id).ToList())
                    context.Producto.Find(d.idProducto).stockActual += d.cantidad;

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

        // Todas las ventas activas y anuladas (las eliminadas no se muestran)
        public static List<paVentaListar_Result> listarPa(DateTime? fechaInicio, DateTime? fechaFin)
        {
            if (fechaInicio.HasValue && fechaFin.HasValue && fechaFin.Value.Date < fechaInicio.Value.Date)
                throw new Exception("La fecha final no puede ser menor a la fecha de inicio");

            using (var context = new GuaridaEntities())
            {
                return context.paVentaListar(fechaInicio, fechaFin).ToList();
            }
        }
    }

}
