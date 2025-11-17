using CapaEntidad;
using CapaNegocio; 
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Security; 

namespace CapaPresentacionTienda.Controllers
{
    public class AccountController : Controller
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

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password)
        {
            Carrito carritoTemporal = GetCarrito();

            string mensajeError;
            CN_Clientes objNegocio = new CN_Clientes();

            Cliente cliente = objNegocio.ValidarCliente(email, password, out mensajeError);

            if (cliente == null)
            {
                ViewBag.Error = mensajeError;
                return View(); 
            }

            FormsAuthentication.SetAuthCookie(cliente.Correo, false);

            Session["ClienteCorreo"] = cliente.Correo;
            Session["ClienteId"] = cliente.IdCliente;

            
            if (carritoTemporal != null && carritoTemporal.Items.Count > 0)
            {
                Session["Carrito"] = carritoTemporal;
            }

            return RedirectToAction("Index", "Tienda");
        }

        public ActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registrar(string nombres, string apellidos, string email, string password, bool esMayor)
        {
            string mensajeError = string.Empty;

            if (!esMayor)
            {
                ViewBag.Error = "Debe confirmar que es mayor de 18 años para registrarse.";
                return View();
            }

            CN_Clientes objNegocio = new CN_Clientes();

            Cliente nuevoCliente = new Cliente()
            {
                Nombres = nombres,
                Apellidos = apellidos,
                Correo = email,
                PasswordHash = Encoding.UTF8.GetBytes(password)
            };

            int idGenerado = objNegocio.Registrar(nuevoCliente, out mensajeError);

            if (idGenerado > 0)
            {
                return RedirectToAction("Login");
            }
            else
            {
                ViewBag.Error = mensajeError;
                return View();
            }
        }

        public ActionResult LogOut()
        {
            Session.Remove("ClienteCorreo");

            return RedirectToAction("Index", "Home");
        }
    }
}