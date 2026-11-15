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
        public static int crear(Cliente cliente)
        {
            using (var contex = new GuaridaEntities()) 
            {
                var existe = contex.Cliente.Where(x => x.cedulaIdentidad == cliente.cedulaIdentidad).FirstOrDefault(); 
                if (existe != null) throw new Exception($"El cliente con ci o nit {cliente.cedulaIdentidad} ya existe");
                contex.Cliente.Add(cliente);
                contex.SaveChanges();
                return cliente.id;
            }
        }
        public static Cliente buscar(string ci)
        {
            using (var context = new GuaridaEntities())
            {
                var cliente = context.Cliente.FirstOrDefault(x => x.cedulaIdentidad == ci && x.estado == (short)Estado.Activo);
                if (cliente == null) throw new Exception($"El cliente con ci o nit {ci} no existe");
                return cliente;
            }
        }
    } 
}
