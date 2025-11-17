using System.Collections.Generic;
using System.Linq;

namespace CapaEntidad
{
    public class Carrito
    {
        public List<CarritoItem> Items { get; private set; }
        public Carrito()
        {
            Items = new List<CarritoItem>();
        }
        public void AgregarItem(Producto producto, int cantidad)
        {
            CarritoItem itemExistente = Items
                .FirstOrDefault(i => i.objProducto.IdProducto == producto.IdProducto);

            if (itemExistente == null)
            {
                Items.Add(new CarritoItem
                {
                    objProducto = producto,
                    Cantidad = cantidad

                });
            }
            else
            {
                itemExistente.Cantidad += cantidad;
            }
        }
        public void EliminarItem(int idProducto)
        {
            Items.RemoveAll(i => i.objProducto.IdProducto == idProducto);
        }
        public decimal CalcularTotal()
        {
            return Items.Sum(i => i.Subtotal);
        }
        public void Limpiar()
        {
            Items.Clear();
        }
    }
}