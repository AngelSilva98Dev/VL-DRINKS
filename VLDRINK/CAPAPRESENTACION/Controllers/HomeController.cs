using System;
using System.Collections.Generic;
using System.Linq; 
using System.Web;
using System.Web.Mvc;
using CapaEntidad;
using CapaNegocio;

namespace CAPAPRESENTACION.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Usuarios()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListarUsuarios()
        {
            int idUsuarioLogueado = (int)Session["UserId"];
            bool esAdminLogueado = (bool)Session["UserEsAdmin"];

            List<Usuario> listaCompleta = new CN_Usuarios().Listar(idUsuarioLogueado, esAdminLogueado);

            List<UsuarioDTO> listaParaElCliente = listaCompleta.Select(u => new UsuarioDTO
            {
                IdUsuario = u.IdUsuario,
                Nombres = u.Nombres,
                Apellidos = u.Apellidos,
                Correo = u.Correo,
                Activo = u.Activo,
                esAdmin = u.esAdmin
            }).ToList();

            return Json(new { data = listaParaElCliente }, JsonRequestBehavior.AllowGet);
        }


        //Crear Administradores

        [HttpPost]
        public JsonResult GuardarUsuario(string Nombres, string Apellidos, string Correo, bool Activo)
        {
            if (!(bool)Session["UserEsAdmin"])
            {
                return Json(new { operacionExitosa = false, mensaje = "Permiso denegado." });
            }

            string mensaje = string.Empty;
            int idGenerado = 0;

            Usuario objeto = new Usuario()
            {
                Nombres = Nombres,
                Apellidos = Apellidos,
                Correo = Correo,
                Activo = Activo
            };

            CN_Usuarios objNegocio = new CN_Usuarios();

            idGenerado = objNegocio.RegistrarAdmin(objeto, out mensaje);

            return Json(new { operacionExitosa = (idGenerado > 0), mensaje = mensaje });
        }

        [HttpPost]
        public JsonResult ModificarUsuario(Usuario objeto)
        {
            string mensaje = string.Empty;
            bool resultado = false;

            int idUsuarioLogueado = (int)Session["UserId"];
            bool esAdminLogueado = (bool)Session["UserEsAdmin"];

            CN_Usuarios objNegocio = new CN_Usuarios();

            resultado = objNegocio.Modificar(objeto, idUsuarioLogueado, esAdminLogueado, out mensaje);

            return Json(new { operacionExitosa = resultado, mensaje = mensaje });
        }

        [HttpPost]
        public JsonResult EliminarUsuario(int idUsuario) 
        {
            string mensaje = string.Empty;

            // Leemos el rol de la sesión
            bool esAdminActual = (bool)Session["UserEsAdmin"];

            CN_Usuarios objNegocio = new CN_Usuarios();

            // Pasamos el rol a la Capa de Negocio
            bool resultado = objNegocio.Eliminar(idUsuario, esAdminActual, out mensaje);

            return Json(new { operacionExitosa = resultado, mensaje = mensaje });
        }

        [HttpPost]
        public JsonResult CambiarPassword(int idUsuario, string nuevaClave)
        {
            string mensaje = string.Empty;


            int idUsuarioLogueado = (int)Session["UserId"];
            bool esAdminLogueado = (bool)Session["UserEsAdmin"];

            CN_Usuarios objNegocio = new CN_Usuarios();


            bool resultado = objNegocio.CambiarPassword(
                idUsuario, 
                nuevaClave,
                idUsuarioLogueado, 
                esAdminLogueado, 
                out mensaje);

            return Json(new { operacionExitosa = resultado, mensaje = mensaje });
        }
    }
}