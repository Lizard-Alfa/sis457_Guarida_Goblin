using CadGuarida;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClnGuarida
{
    public class DetalleCln
    {
        // Lanza error si la venta no existe o está anulada
        public static void validar(int idVenta)
        {
            using (var context = new GuaridaEntities())
            {
                var venta = context.Venta.Find(idVenta);
                if (venta == null || venta.estado != (short)Estado.Activo)
                    throw new Exception("No existe esa venta");
            }
        }

        public static List<VentaDetalle> listarPorVenta(int idVenta)
        {
            validar(idVenta);

            using (var context = new GuaridaEntities())
            {
                return context.VentaDetalle
                    .Include("Producto")
                    .Include("Venta.Cliente")
                    .Where(x => x.idVenta == idVenta)
                    .ToList();
            }
        }
    }
}
