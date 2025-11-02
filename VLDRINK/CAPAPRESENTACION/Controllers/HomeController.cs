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

            List<Usuario> listaCompleta = new CN_Usuarios().Listar();


            List<UsuarioDTO> listaParaElCliente = listaCompleta.Select(u => new UsuarioDTO
            {
                IdUsuario = u.IdUsuario,
                Nombres = u.Nombres,
                Apellidos = u.Apellidos,
                Correo = u.Correo,
                Activo = u.Activo,
                Reestablecer = u.Reestablecer
            }).ToList();


            return Json(new { data = listaParaElCliente }, JsonRequestBehavior.AllowGet);
        }
    }
}