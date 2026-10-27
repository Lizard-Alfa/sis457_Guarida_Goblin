using CadGuarida;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClnGuarida
{
    public class VentaDetalleCln
    {                                                                                                                                                                                                                       
        public static List<VentaDetalle> listar()
        {
            using (var context = new GuaridaEntities())
            {
                return context.VentaDetalle.Where(x => x.estado == (short)Estado.Activo).OrderBy(x => x.id).ToList();
            }
        }
    }
}
