using CapaEntidad;
using CapaNegocio;
using System;
using System.Linq;
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


        [HttpGet]
        public ActionResult TestearVerificacion()
        {
            // 1. Llama a la capa de negocio
            CN_Usuarios objNegocio = new CN_Usuarios();
            string resultado = objNegocio.TestearVerificacion();

            // 2. Devuelve el resultado como texto simple
            return Content(resultado, "text/plain");
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

            // Usamos el campo PasswordHash temporalmente para pasar la contraseña
            // en texto plano a la capa de negocio.
            Usuario nuevoUsuario = new Usuario()
            {
                Nombres = nombres,
                Apellidos = apellidos,
                Correo = email,
                PasswordHash = password.Select(c => (byte)c).ToArray() // "Truco" para pasar el string
            };

            // Llamamos a la capa de negocio
            int idGenerado = objNegocio.Registrar(nuevoUsuario, out mensajeError);

            if (idGenerado > 0)
            {
                // ¡Éxito! Redirigimos al Login para que inicie sesión
                return RedirectToAction("Login");
            }
            else
            {
                // Falló. Mostramos el error
                ViewBag.Error = mensajeError;
                return View();
            }
        }
    }
}
