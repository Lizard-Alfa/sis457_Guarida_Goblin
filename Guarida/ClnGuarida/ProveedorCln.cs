using CadGuarida;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClnGuarida
{
    public class ProveedorCln
    {
        public static int crear(Proveedor proveedor)
        {
            using (var context = new GuaridaEntities())
            {
                var existe = context.Proveedor
                    .Where(x => x.razonSocial == proveedor.razonSocial && x.estado == (short)Estado.Activo)
                    .FirstOrDefault();
                if (existe != null)
                    throw new Exception($"El proveedor '{proveedor.razonSocial}' ya existe");

                context.Proveedor.Add(proveedor);
                context.SaveChanges();
                return proveedor.id;
            }
        }

        public static int actualizar(Proveedor proveedor)
        {
            using (var context = new GuaridaEntities())
            {
                var existe = context.Proveedor.Find(proveedor.id);
                if (existe == null)
                    throw new Exception($"El proveedor con id {proveedor.id} no existe");

                existe.tipo = proveedor.tipo;
                existe.razonSocial = proveedor.razonSocial;
                existe.nit = proveedor.nit;
                existe.contacto = proveedor.contacto;
                existe.email = proveedor.email;
                existe.telefono = proveedor.telefono;
                existe.pais = proveedor.pais;
                existe.usuarioRegistro = proveedor.usuarioRegistro;
                return context.SaveChanges();
            }
        }

        public static int eliminar(int id, string usuario)
        {
            using (var context = new GuaridaEntities())
            {
                var existe = context.Proveedor.Find(id);
                if (existe == null)
                    throw new Exception($"El proveedor con id {id} no existe");

                existe.estado = (short)Estado.Eliminado;
                existe.usuarioRegistro = usuario;
                return context.SaveChanges();
            }
        }

        public static Proveedor obtenerUno(int id)
        {
            using (var context = new GuaridaEntities())
            {
                var prov = context.Proveedor.Find(id);
                if (prov == null)
                    throw new Exception($"El proveedor con id {id} no existe");
                return prov;
            }
        }

        public static List<paProveedorListar_Result> listarPa(string tipo, string parametro)
        {
            using (var context = new GuaridaEntities())
            {
                return context.paProveedorListar(tipo, parametro).ToList();
            }
        }
    }
}