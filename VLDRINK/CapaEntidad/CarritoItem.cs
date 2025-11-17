using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class CarritoItem
    {
        public Producto objProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal Subtotal
        {
            get { return objProducto.Precio * Cantidad; }
        }
    }
}
