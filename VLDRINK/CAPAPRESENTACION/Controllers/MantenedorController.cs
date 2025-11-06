using CapaEntidad;
using CapaNegocio;
using System.Collections.Generic;
using System.Linq; 
using System.Web.Mvc;

namespace CAPAPRESENTACION.Controllers
{
    [Authorize]
    public class MantenedorController : BaseController 
    {
        
        public ActionResult Categoria()
        {
            return View();
        }
        public ActionResult Marca()
        {
            return View();
        }
        public ActionResult Producto()
        {
            return View();
        }

        // --- CRUD DE CATEGORIAS ---

        // LEER 
        [HttpGet]
        public JsonResult ListarCategorias()
        {
            List<Categoria> lista = new CN_Categorias().Listar();

            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        // CREAR
        [HttpPost]
        public JsonResult GuardarCategoria(Categoria objeto)
        {
            string mensaje = string.Empty;
            int idGenerado = new CN_Categorias().Registrar(objeto, out mensaje);

            if (idGenerado > 0)
            {
                mensaje = "Categoría guardada exitosamente.";
            }

            return Json(new { operacionExitosa = (idGenerado > 0), mensaje = mensaje });
        }

        // MODIFICAR
        [HttpPost]
        public JsonResult ModificarCategoria(Categoria objeto)
        {
            string mensaje = string.Empty;
            bool resultado = new CN_Categorias().Modificar(objeto, out mensaje);

            if (resultado)
            {
                mensaje = "Categoría modificada exitosamente.";
            }

            return Json(new { operacionExitosa = resultado, mensaje = mensaje });
        }

        // ELIMINAR
        [HttpPost]
        public JsonResult EliminarCategoria(int idCategoria)
        {
            string mensaje = string.Empty;
            bool resultado = new CN_Categorias().Eliminar(idCategoria, out mensaje);

            return Json(new { operacionExitosa = resultado, mensaje = mensaje });
        }


        // --- CRUD DE MARCAS ---

        // LEER
        [HttpGet]
        public JsonResult ListarMarcas()
        {
            List<Marca> lista = new CN_Marcas().Listar();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        // CREAR
        [HttpPost]
        public JsonResult GuardarMarca(Marca objeto)
        {
            string mensaje = string.Empty;
            int idGenerado = new CN_Marcas().Registrar(objeto, out mensaje);

            if (idGenerado > 0)
            {
                mensaje = "Marca guardada exitosamente.";
            }

            return Json(new { operacionExitosa = (idGenerado > 0), mensaje = mensaje });
        }

        // MODIFICAR
        [HttpPost]
        public JsonResult ModificarMarca(Marca objeto)
        {
            string mensaje = string.Empty;
            bool resultado = new CN_Marcas().Modificar(objeto, out mensaje);

            if (resultado)
            {
                mensaje = "Marca modificada exitosamente.";
            }

            return Json(new { operacionExitosa = resultado, mensaje = mensaje });
        }

        // ELIMINAR
        [HttpPost]
        public JsonResult EliminarMarca(int idMarca)
        {
            string mensaje = string.Empty;
            bool resultado = new CN_Marcas().Eliminar(idMarca, out mensaje);

            return Json(new { operacionExitosa = resultado, mensaje = mensaje });
        }


        // --- CRUD DE PRODUCTOS ---

        // LEER
        [HttpGet]
        public JsonResult ListarProductos()
        {
            List<Producto> lista = new CN_Productos().Listar();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        // CREAR
        [HttpPost]
        public JsonResult GuardarProducto(Producto objeto)
        {
            string mensaje = string.Empty;
            int idGenerado = new CN_Productos().Registrar(objeto, out mensaje);
            if (idGenerado > 0)
            {
                mensaje = "Producto guardado exitosamente.";
            }
            return Json(new { operacionExitosa = (idGenerado > 0), mensaje = mensaje });
        }

        // MODIFICAR
        [HttpPost]
        public JsonResult ModificarProducto(Producto objeto)
        {
            string mensaje = string.Empty;
            bool resultado = new CN_Productos().Modificar(objeto, out mensaje);
            if (resultado)
            {
                mensaje = "Producto modificado exitosamente.";
            }
            return Json(new { operacionExitosa = resultado, mensaje = mensaje });
        }

        // ELIMINAR
        [HttpPost]
        public JsonResult EliminarProducto(int idProducto)
        {
            string mensaje = string.Empty;
            bool resultado = new CN_Productos().Eliminar(idProducto, out mensaje);
            return Json(new { operacionExitosa = resultado, mensaje = mensaje });
        }

        [HttpGet]
        public JsonResult ObtenerProducto(int idProducto)
        {
            Producto obj = new CN_Productos().ObtenerProducto(idProducto);

            return Json(obj, JsonRequestBehavior.AllowGet);
        }

    }
}