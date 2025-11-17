using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Pedido
    {
        public int IdPedido { get; set; }
        public Cliente objCliente { get; set; }
        public int TotalProducto { get; set; }
        public decimal MontoTotal { get; set; }
        public string Contacto { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public string MetodoPago { get; set; }
        public string Estado { get; set; }
        public DateTime FechaPedido { get; set; }
        public List<DetallePedido> objDetallePedido { get; set; }
    }
}
