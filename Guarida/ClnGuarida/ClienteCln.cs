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
        public static List<object> listar(string filtro)
        {
            using (var context = new GuaridaEntities())
            {
                var query = context.Cliente.Where(x => x.estado == (short)Estado.Activo);
                bool tieneFiltro = !string.IsNullOrEmpty(filtro);
                if (tieneFiltro)
                {
                    query = query.Where(x => x.nombres.Contains(filtro) || x.apellidos.Contains(filtro));
                }
                var orden = tieneFiltro
                    ? query.OrderBy(x => x.nombres.StartsWith(filtro) ? 0 : 1).ThenBy(x => x.nombres)
                    : query.OrderByDescending(x => x.fechaRegistro);
                return orden
                    .Take(10)
                    .Select(x => new {
                        id = x.id,
                        ApellidoNombre = x.nombres + " " + x.apellidos
                    })
                    .ToList<object>();
            }
        }
    }
}
