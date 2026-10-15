using CadGuarida;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClnGuarida
{
    public class UsuarioCln
    {
        public static Usuario validar(string usuario, string clave)
        {
            using (var context = new GuaridaEntities())
            {
                return context.Usuario.Where(x=> x.usuario1 == usuario && x.clave == clave).FirstOrDefault();
            }
        }
    }
}
