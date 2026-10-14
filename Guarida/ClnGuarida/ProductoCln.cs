using CadGuarida;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ClnGuarida
{
    public class ProductoCln
    {
        public static int crear(Producto producto)
        {
            using (var context = new GuaridaEntities())
            {
                var existe = context.Producto.Where(x => x.codigo == producto.codigo).FirstOrDefault();
                if (existe != null) throw new Exception($"El producto {producto.codigo} ya existe");
                context.Producto.Add(producto);
                context.SaveChanges();
                return producto.id;
            }
        }

        public static int actualizar(Producto producto)
        {
            using (var context = new GuaridaEntities())
            {
                var existe = context.Producto.Find(producto.id);
                if (existe == null) throw new Exception($"El producto con id {producto.id} no exite");

                existe.idCategoria = producto.idCategoria;
                existe.codigo = producto.codigo;
                existe.nombre = producto.nombre;
                existe.descripcion = producto.descripcion;
                existe.unidad = producto.unidad;
                existe.pesoKg = producto.pesoKg;
                existe.precioCostoUsd = producto.precioCostoUsd;
                existe.precioVentaBob = producto.precioVentaBob;
                existe.stockActual = producto.stockActual;
                existe.stockMinimo = producto.stockMinimo;
                existe.usuarioRegistro = producto.usuarioRegistro;

                return context.SaveChanges();
            }
        }

        public static int eliminar(int id, string usuario)
        {
            using (var context = new GuaridaEntities())
            {
                var existe = context.Producto.Find(id);
                if (existe == null) throw new Exception($"El producto con id {id} no exite");

                existe.estado = -1;
                existe.usuarioRegistro = usuario;
                return context.SaveChanges();
            }
        }

        public static Producto obtenerUno(int id)
        {
            using (var context = new GuaridaEntities())
            {
                var producto = context.Producto.Find(id);
                if (producto == null) throw new Exception($"El producto con id {id} no exite");
                return producto;
            }
        }

        public static List<Producto> listar()
        {
            using (var context = new GuaridaEntities())
            {
                return context.Producto.Where(x=> x.estado == 1).OrderBy(x=> x.nombre).ToList();
            }
        }

        public static List<paProductoListar_Result> listarPa(string parametro)
        {
            using (var context = new GuaridaEntities())
            {
                return context.paProductoListar(parametro.Trim()).ToList();
            }
        }
    }
}
