using CapaEntidad;
using CapaNegocio;
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
            {
                return RedirectToAction("Index", "Tienda");
            }

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

            return View(carrito);
        }
    }
}