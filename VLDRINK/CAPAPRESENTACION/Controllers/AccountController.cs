using CapaEntidad;
using CapaNegocio;
using System.Web.Mvc;
using System.Web.Security;

namespace CAPAPRESENTACION.Controllers
{
    public class AccountController : Controller
    {
        // Metodo para ir a la view de Login
        [HttpGet]
        public ActionResult Login()
        {
            return View(); 
        }

        // Autenticacion de Usuario
        [HttpPost]
        [ValidateAntiForgeryToken] // <-- Verifica el token de seguridad
        public ActionResult Login(string email, string password)
        {
            string mensajeError;
            CN_Usuarios objNegocio = new CN_Usuarios();

            //  Llama a la Capa de Negocio para validar
            Usuario usuario = objNegocio.ValidarUsuario(email, password, out mensajeError);

            //  Si el usuario es NULO, hubo un error
            if (usuario == null)
            {
                ViewBag.Error = mensajeError; // "Credenciales inválidas."
                return View(); // Devuelve la vista Login, pero con el mensaje de error
            }


            //  Creamos la "cookie" de autenticación
            FormsAuthentication.SetAuthCookie(usuario.Correo, false);

            // (Opcional) Puedes guardar datos del usuario en la Sesión
            Session["UserCorreo"] = usuario.Correo;
            Session["UserNombre"] = usuario.Nombres;

            // Redirigimos al Inicio
            return RedirectToAction("Index", "Home");
        }


        // Metodo cerrar sesion
        [HttpGet]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut(); // <-- Borra la cookie de autenticación
            Session.Clear(); // Limpia la sesión
            return RedirectToAction("Index", "Home");
        }

    }
}
