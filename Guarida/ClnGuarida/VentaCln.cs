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
                        // 1. Configuramos y guardamos PRIMERO la cabecera de la venta
                        venta.fecha = DateTime.Now;
                        venta.fechaRegistro = DateTime.Now;
                        venta.estado = (short)Estado.Activo;
                        venta.subtotal = 0m; // Se actualizará al final
                        venta.total = 0m;    // Se actualizará al final

                        context.Venta.Add(venta);
                        context.SaveChanges(); //¡Aquí SQL Server genera el id real de la venta!

                        decimal subtotalGeneral = 0m;

                        // 2. Procesamos cada detalle utilizando el ID real de la venta ya guardada
                        foreach (var detalle in detalles)
                        {
                            var producto = context.Producto.Find(detalle.idProducto);
                            if (producto == null)
                                throw new Exception($"El producto con id {detalle.idProducto} no existe");
                            if (producto.stockActual < detalle.cantidad)
                                throw new Exception($"Stock insuficiente de {producto.nombre}. Disponible: {producto.stockActual}");

                            // Descontamos stock del producto
                            producto.stockActual = producto.stockActual - detalle.cantidad;

                            // Asignamos explícitamente el idVenta para que queden vinculados correctamente
                            detalle.idVenta = venta.id;
                            detalle.precioUnitario = producto.precioVentaBob;
                            detalle.subtotal = detalle.cantidad * detalle.precioUnitario;
                            detalle.usuarioRegistro = venta.usuarioRegistro;
                            detalle.fechaRegistro = DateTime.Now;
                            detalle.estado = (short)Estado.Activo;

                            subtotalGeneral += detalle.subtotal;

                            // Agregamos el detalle al contexto de forma independiente pero atado al idVenta
                            context.VentaDetalle.Add(detalle);
                        }

                        // 3. Actualizamos los montos totales en la cabecera de la venta
                        venta.subtotal = subtotalGeneral;
                        venta.total = subtotalGeneral;

                        // Guardamos los cambios de los detalles y los nuevos totales de la venta
                        context.SaveChanges();

                        // Si todo salió bien, confirmamos la transacción en la base de datos
                        transaccion.Commit();
                        return venta.id;
                    }
                    catch
                    {
                        // Si ocurre cualquier error, se revierte todo (stock y registros)
                        transaccion.Rollback();
                        throw;
                    }
                }
            }
        }
        public static List<Venta> listar()
        {
            using (var context = new GuaridaEntities())
            {
                return context.Venta.Where(x => x.estado == (short)Estado.Activo).OrderBy(x => x.id).ToList();
            }
        }
        public static List<paVentaListar_Result> listarPa(DateTime? fechaInicio, DateTime? fechaFin)
        {
            using (var context = new GuaridaEntities())
            {
                return context.paVentaListar(fechaInicio,fechaFin).ToList();
            }
        }
    }

}
