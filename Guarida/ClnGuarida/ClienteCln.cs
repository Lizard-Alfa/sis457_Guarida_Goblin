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
                var busacar = context.Cliente.Where(x => x.estado == (short)Estado.Activo);
                if (!string.IsNullOrEmpty(filtro))
                {
                    string f = filtro.Trim().ToLower();
                    busacar = busacar.Where(x => x.nombres.ToLower().Contains(f) || x.apellidos.ToLower().Contains(f))
                                 .OrderBy(x => x.nombres);
                }
                else
                {
                    busacar = busacar.OrderByDescending(x => x.fechaRegistro)
                                 .Take(10);
                }
                return busacar.Select(x => new
                {
                    id = x.id,
                    ApellidoNombre = x.nombres + " " + x.apellidos
                })
                            .ToList<object>();
            }
            }
        }
    }
