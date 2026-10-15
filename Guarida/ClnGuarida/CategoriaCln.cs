using CadGuarida;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClnGuarida
{
    public class CategoriaCln
    {
        public static List<Categoria> listar()
        {
            using (var context = new GuaridaEntities())
            {
                return context.Categoria
                    .Where(x => x.estado == (short)Estado.Activo)
                    .OrderBy(x => x.nombre).ToList();
            } 
        }
    }
}
