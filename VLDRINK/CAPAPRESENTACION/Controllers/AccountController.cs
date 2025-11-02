using CapaEntidad;
using CapaNegocio;
using System;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;

namespace CAPAPRESENTACION.Controllers
{
    [AllowAnonymous]
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
        [ValidateAntiForgeryToken]
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
            FormsAuthentication.SignOut(); 
            Session.Clear(); 
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public ActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registrar(string nombres, string apellidos, string email, string password)
        {
            string mensajeError = string.Empty;
            CN_Usuarios objNegocio = new CN_Usuarios();


            Usuario nuevoUsuario = new Usuario()
            {
                Nombres = nombres,
                Apellidos = apellidos,
                Correo = email,

                PasswordHash = Encoding.UTF8.GetBytes(password)
            };


            int idGenerado = objNegocio.Registrar(nuevoUsuario, out mensajeError);

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
    }
}