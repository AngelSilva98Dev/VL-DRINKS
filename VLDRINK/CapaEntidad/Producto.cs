using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Producto
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        public  Marca objMarca { get; set; }
        public Categoria objCategoria { get; set; }
        public decimal Precio { get; set; }

        public int Stock { get; set; }
        public string Imagen { get; set; }
        public string NombreImagen { get; set; }
        public bool Activo { get; set; }

    }
}
