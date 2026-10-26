using CadGuarida;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClnGuarida
{
    public class ClienteCln
    {
        public static List<Cliente> listar()
        {
            using (var context = new GuaridaEntities())
            {
                return context.Cliente
                    .Where(x => x.estado == (short)Estado.Activo)
                    .OrderBy(x => x.nombres).ToList();
            }
        }
    }
}
