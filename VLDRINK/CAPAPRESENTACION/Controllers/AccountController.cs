using CapaEntidad;
using CapaNegocio;
using System;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;

namespace CAPAPRESENTACION.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password)
        {
            string mensajeError;
            CN_Usuarios objNegocio = new CN_Usuarios();

            Usuario usuario = objNegocio.ValidarUsuario(email, password, out mensajeError);

            if (usuario == null)
            {
                ViewBag.Error = mensajeError; 
                return View(); 
            }

            FormsAuthentication.SetAuthCookie(usuario.Correo, false);

            Session["UserCorreo"] = usuario.Correo;
            Session["UserNombre"] = usuario.Nombres;
            Session["UserId"] = usuario.IdUsuario;   
            Session["UserEsAdmin"] = usuario.esAdmin;

            return RedirectToAction("Index", "Home");
        }



        [HttpGet]
        [AllowAnonymous]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut(); 
            Session.Clear();

            TempData["Notification"] = "Se cerró la sesión correctamente.";

            return RedirectToAction("Login", "Account");
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

        [AllowAnonymous] 
        [HttpPost]
        public JsonResult SolicitarReestablecimiento(string email)
        {
            string mensajeError = string.Empty;
            CN_Usuarios objNegocio = new CN_Usuarios();

            // --- PRÓXIMO PASO: ---
            // Llamaremos a la CapaNegocio. El método 'SolicitarReestablecimiento'
            // que está aquí abajo todavía no existe. Lo crearemos en el siguiente paso.
            bool resultado = objNegocio.SolicitarReestablecimiento(email, out mensajeError);

            // Devolvemos una respuesta JSON que el JavaScript pueda entender
            if (resultado)
            {
                return Json(new { operacionExitosa = true, mensaje = mensajeError });
            }
            else
            {
                return Json(new { operacionExitosa = false, mensaje = mensajeError });
            }
        }
    }
}