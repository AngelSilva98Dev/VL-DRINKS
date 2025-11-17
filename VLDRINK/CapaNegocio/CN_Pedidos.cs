using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Pedidos
    {
        private CD_Pedidos objCapaDato = new CD_Pedidos();
        public bool CambiarEstado(int idPedido, string nuevoEstado, out string Mensaje)
        {
            Mensaje = string.Empty;

            return objCapaDato.CambiarEstado(idPedido, nuevoEstado, out Mensaje);
        }
        public List<Pedido> Listar(string estado)
        {
            return objCapaDato.Listar(estado);
        }
        public int Registrar(Pedido objPedido, List<DetallePedido> listaDetallePedido, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(objPedido.Contacto))
            {
                Mensaje = "Debe ingresar un nombre de contacto.";
                return 0;
            }
            if (string.IsNullOrEmpty(objPedido.Telefono))
            {
                Mensaje = "Debe ingresar un teléfono de contacto.";
                return 0;
            }
            if (string.IsNullOrEmpty(objPedido.Direccion))
            {
                Mensaje = "Debe ingresar una dirección de envío.";
                return 0;
            }

            if (listaDetallePedido.Count == 0)
            {
                Mensaje = "Su carrito está vacío.";
                return 0;
            }

            objPedido.TotalProducto = listaDetallePedido.Count;

            objPedido.Subtotal = listaDetallePedido.Sum(i => i.Total);

            objPedido.MontoTotal = objPedido.Subtotal + objPedido.CostoEnvio;

            if (objPedido.MetodoPago == "Transferencia")
                objPedido.Estado = "Esperando Comprobante";
            else
                objPedido.Estado = "En Preparacion";

            return objCapaDato.Registrar(objPedido, listaDetallePedido, out Mensaje);
        }
    }
}
