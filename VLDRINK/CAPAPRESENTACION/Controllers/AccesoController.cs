using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaEntidad;
using CapaNegocio;
using System.Web.Security;

namespace CAPAPRESENTACION.Controllers
{
    public class AccesoController : Controller
    {
        // GET: Acceso
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CambiarClave()
        {
            return View();
        }

        public ActionResult Reestablecer()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string correo, string clave)
        {
            Usuario objUsuario = new Usuario();

            objUsuario = new CN_Usuarios().Listar().Where(u => u.Correo == correo && u.Clave == CN_Recursos.ConvertirSha256(clave)).FirstOrDefault();

            if(objUsuario == null)
            {
                ViewBag.Error = "Correo o contraseña incorrectos";
                return View();
            }
            else
            {
                if (objUsuario.Reestablecer)
                {
                    TempData["IdUsuario"] = objUsuario.IdUsuario;
                    return RedirectToAction("CambiarClave");
                }

                FormsAuthentication.SetAuthCookie(objUsuario.Correo, false);
                ViewBag.Error = null;
                return RedirectToAction("Index", "Home");
            }

        }

        [HttpPost]

        public ActionResult CambiarClave(string idusuario, string claveactual, string nuevaclave, string confirmarclave)
        {
            Usuario objUsuario = new Usuario();

            objUsuario = new CN_Usuarios().Listar().Where(u => u.IdUsuario == int.Parse(idusuario)).FirstOrDefault();

            if(objUsuario.Clave != CN_Recursos.ConvertirSha256(claveactual))
            {
                TempData["Idusuario"] = idusuario;
                ViewData["vclave"] = "";
                ViewBag.Error = "La contraseña actual no es correcta";
                return View();

            }
            else if(nuevaclave != confirmarclave)
            {
                TempData["Idusuario"] = idusuario;
                ViewData["vclave"] = claveactual;
                ViewBag.Error = "La contraseñas no coinciden";
                return View();
            }

            ViewData["vclave"] = "";
             nuevaclave = CN_Recursos.ConvertirSha256(nuevaclave);

            string mensaje = string.Empty;
            bool respuesta = new CN_Usuarios().CambiarClave(int.Parse(idusuario), nuevaclave, out mensaje);
            if (respuesta)
            {
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Idusuario"] = idusuario;
                ViewBag.Error = mensaje;
                return View();
            }

           
        }

        [HttpPost]

        public ActionResult Reestablecer(string correo)
        {
            Usuario objUsuario = new Usuario();

            objUsuario = new CN_Usuarios().Listar().Where(item => item.Correo == correo).FirstOrDefault();

            if (objUsuario == null)
            {
                ViewBag.Error = "No se encontró un usuario relacionado a ese mail";
                return View();
            }
            
            string mensaje = string.Empty;
            bool respuesta = new CN_Usuarios().ReestablecerClave(objUsuario.IdUsuario, correo, out mensaje);
            if (respuesta)
            {
                ViewBag.Error = null;
                return RedirectToAction("Index","Acceso");

            }
            else
            {
                ViewBag.Error = mensaje;
                return View();
            }
        }

        public ActionResult CerrarSesion()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Acceso");

        }
    }
}