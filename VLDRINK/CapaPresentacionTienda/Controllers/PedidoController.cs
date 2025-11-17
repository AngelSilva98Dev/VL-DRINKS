using CapaEntidad;
using CapaNegocio;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;

namespace CapaPresentacionTienda.Controllers
{
    [Authorize]
    public class PedidoController : Controller
    {
        private Carrito GetCarrito()
        {
            Carrito carrito = (Carrito)Session["Carrito"];
            if (carrito == null)
            {
                carrito = new Carrito();
                Session["Carrito"] = carrito;
            }
            return carrito;
        }

        private Cliente GetClienteLogueado()
        {
            if (Session["ClienteId"] == null)
            {
                return null; 
            }

            int idCliente = (int)Session["ClienteId"];


            return new CN_Clientes().ObtenerClientePorId(idCliente);
        }

        [HttpGet]
        public ActionResult Checkout()
        {
            Carrito carrito = GetCarrito();

            if (carrito.Items.Count == 0)
                return RedirectToAction("Index", "Tienda");

            if (Session["ClienteId"] == null)
                return RedirectToAction("Login", "Account");

            int idCliente = (int)Session["ClienteId"];

            Cliente cliente = new CN_Clientes().ObtenerClientePorId(idCliente);

            if (cliente == null)
            {
                ViewBag.Error = "Error al obtener los datos del cliente. Intente iniciar sesión de nuevo.";
                ViewBag.Cliente = new Cliente();
            }
            else
            {
                ViewBag.Cliente = cliente;
            }

            var servicioEnvio = new ServicioEnvio();
            decimal costoEnvio = servicioEnvio.ObtenerCostoEnvio();
            decimal subtotal = carrito.CalcularTotal();

            ViewBag.Subtotal = subtotal;
            ViewBag.CostoEnvio = costoEnvio;
            ViewBag.Total = subtotal + costoEnvio;

            return View(carrito);
        }
        public ActionResult Confirmacion(int id, string metodo)
        {
            ViewBag.IdPedido = id;
            ViewBag.MetodoPago = metodo;

            ViewBag.Alias = ConfigurationManager.AppSettings["DatosBanco_Alias"];
            ViewBag.CBU = ConfigurationManager.AppSettings["DatosBanco_CBU"];
            ViewBag.WhatsApp = ConfigurationManager.AppSettings["DatosContacto_WhatsApp"];

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Checkout(string Contacto, string Telefono, string Direccion, string MetodoPago)
        {
            Carrito carrito = GetCarrito(); 
            int idCliente = (int)Session["ClienteId"];
            var servicioEnvio = new ServicioEnvio();
            decimal costoEnvio = servicioEnvio.ObtenerCostoEnvio();
            decimal subtotal = carrito.CalcularTotal();

            Cliente clienteLogueado = new CN_Clientes().ObtenerClientePorId(idCliente);



            Pedido objPedido = new Pedido
            {
                objCliente = new Cliente() { IdCliente = idCliente },
                Contacto = Contacto,
                Telefono = Telefono,
                Direccion = Direccion,
                MetodoPago = MetodoPago,

                Subtotal = subtotal,
                CostoEnvio = costoEnvio,
                MontoTotal = subtotal + costoEnvio
            };


            List<DetallePedido> listaDetallePedido = new List<DetallePedido>();
            foreach (CarritoItem item in carrito.Items)
            {
                listaDetallePedido.Add(new DetallePedido()
                {
                    objProducto = item.objProducto,
                    Cantidad = item.Cantidad,
                    PrecioUnitario = item.objProducto.Precio,
                    Total = item.Subtotal, 
                    NombreProducto = item.objProducto.Nombre
                });
            }

            CN_Pedidos objCN_Pedidos = new CN_Pedidos();
            string mensajeError = string.Empty;
            int idPedidoGenerado = objCN_Pedidos.Registrar(objPedido, listaDetallePedido, out mensajeError);

            if (idPedidoGenerado > 0)
            {
                carrito.Limpiar();
                Session["Carrito"] = carrito;

                return RedirectToAction("Confirmacion", new { id = idPedidoGenerado, metodo = MetodoPago });
            }
            else
            {
                ViewBag.Error = mensajeError;
                ViewBag.Cliente = clienteLogueado;
                return View(carrito);
            }
        }
    }
}