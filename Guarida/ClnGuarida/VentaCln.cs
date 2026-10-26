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
        public static int registrar(Venta venta, List<VentaDetalle> detalles)
        {
            if (detalles == null || detalles.Count == 0)
                throw new Exception("La venta debe tener al menos un producto");

            using (var context = new GuaridaEntities())
            {
                using (var transaccion = context.Database.BeginTransaction())
                {
                    try
                    {
                        decimal subtotal = 0m;
                        foreach (var detalle in detalles)
                        {
                            var producto = context.Producto.Find(detalle.idProducto);
                            if (producto == null)
                                throw new Exception($"El producto con id {detalle.idProducto} no existe");
                            if (producto.stockActual < detalle.cantidad)
                                throw new Exception($"Stock insuficiente de {producto.nombre}. Disponible: {producto.stockActual}");

                            producto.stockActual = producto.stockActual - detalle.cantidad;
                            detalle.precioUnitario = producto.precioVentaBob;
                            detalle.subtotal = detalle.cantidad * detalle.precioUnitario;
                            detalle.usuarioRegistro = venta.usuarioRegistro;
                            detalle.fechaRegistro = DateTime.Now;
                            detalle.estado = (short)Estado.Activo;

                            subtotal += detalle.subtotal;
                            venta.VentaDetalle.Add(detalle);
                        }

                        venta.fecha = DateTime.Now;
                        venta.subtotal = subtotal;
                        venta.total = subtotal;
                        venta.fechaRegistro = DateTime.Now;
                        venta.estado = (short)Estado.Activo;

                        context.Venta.Add(venta);
                        context.SaveChanges();
                        transaccion.Commit();
                        return venta.id;
                    }
                    catch
                    {
                        transaccion.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}
