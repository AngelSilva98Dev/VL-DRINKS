using System;
using System.Collections.Generic;
using System.Linq; // <-- 1. ASEGÚRATE DE TENER ESTE 'USING'
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

        public ActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListarUsuarios()
        {
            // 1. Obtenemos la lista completa de la capa de negocio
            List<Usuario> listaCompleta = new CN_Usuarios().Listar();

            // 2. Mapeamos a una lista DTO (SEGURA) para quitar Hash y Salt
            List<UsuarioDTO> listaParaElCliente = listaCompleta.Select(u => new UsuarioDTO
            {
                IdUsuario = u.IdUsuario,
                Nombres = u.Nombres,
                Apellidos = u.Apellidos,
                Correo = u.Correo,
                Activo = u.Activo,
                Reestablecer = u.Reestablecer
                // Intencionalmente omitimos los campos sensibles
            }).ToList();

            // 3. Devolvemos la lista SEGURA en el formato que DataTables espera
            return Json(new { data = listaParaElCliente }, JsonRequestBehavior.AllowGet);
        }

    }
}