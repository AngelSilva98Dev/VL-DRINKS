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
        // GET: /Account/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password)
        {
            string mensajeError;
            // Usamos la nueva capa de negocio
            CN_Clientes objNegocio = new CN_Clientes();

            // Validamos al Cliente
            Cliente cliente = objNegocio.ValidarCliente(email, password, out mensajeError);

            if (cliente == null)
            {
                ViewBag.Error = mensajeError;
                return View();
            }

            // --- Autenticación para el Cliente ---
            // Puedes usar una variable de Sesión diferente
            // para no mezclarla con la del admin
            Session["ClienteCorreo"] = cliente.Correo;

            // (O si usas FormsAuthentication, puedes darle un rol)
            // FormsAuthentication.SetAuthCookie(cliente.Correo, false);

            return RedirectToAction("Index", "Home"); // Redirige a la Home de la Tienda
        }

        // GET: /Account/Registrar
        public ActionResult Registrar()
        {
            return View();
        }

        // POST: /Account/Registrar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registrar(string nombres, string apellidos, string email, string password)
        {
            string mensajeError = string.Empty;
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

        // GET: /Account/LogOut
        public ActionResult LogOut()
        {
            // Limpia la sesión del cliente
            Session.Remove("ClienteCorreo");
            // Opcional: Limpia toda la sesión
            // Session.Clear(); 

            // FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}