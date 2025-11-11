using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaEntidad;
using CapaNegocio;

namespace CAPAPRESENTACION.Controllers
{
    public class HomeController : Controller
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
            List<Usuario> listaUsuario = new List<Usuario>();
            listaUsuario = new CN_Usuarios().Listar();
            return Json(new { data = listaUsuario },JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GuardarUsuario(Usuario objUsuario)
        {
            object resultado;
            string mensaje = string.Empty;
            if(objUsuario.IdUsuario == 0)
            {
                resultado = new CN_Usuarios().Registrar(objUsuario, out mensaje);
            }
            else
            {
                resultado = new CN_Usuarios().Editar(objUsuario, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult EliminarUsuario (int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CN_Usuarios().Eliminar(id, out mensaje);

            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }
    }
}