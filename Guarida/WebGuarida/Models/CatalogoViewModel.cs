using System.Collections.Generic;

namespace WebGuarida.Models
{
    public class CatalogoViewModel
    {
        public List<Producto> Productos { get; set; } = new List<Producto>();
        public List<Categoria> Categorias { get; set; } = new List<Categoria>();

        public int? CategoriaSeleccionada { get; set; }

        // Datos de paginación
        public int PaginaActual { get; set; } = 1;
        public int TotalPaginas { get; set; }
        public int TotalProductos { get; set; }
    }
}