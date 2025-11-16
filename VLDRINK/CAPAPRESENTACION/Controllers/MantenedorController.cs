using CapaEntidad;
using CapaNegocio;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CAPAPRESENTACION.Controllers
{
    [Authorize]
    public class MantenedorController : Controller
    {
        // GET: Mantenedor
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

        //Categoria!!!!!!!!!!!!!
        #region CATEGORIA

        [HttpGet]
        public JsonResult ListarCategorias()
        {
            List<Categoria> listaCategoria = new List<Categoria>();
            listaCategoria = new CN_Categorias().Listar();
            return Json(new { data = listaCategoria }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarCategoria(Categoria objCategoria)
        {
            object resultado;
            string mensaje = string.Empty;
            if (objCategoria.IdCategoria == 0)
            {
                resultado = new CN_Categorias().Registrar(objCategoria, out mensaje);
            }
            else
            {
                resultado = new CN_Categorias().Editar(objCategoria, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult EliminarCategoria(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CN_Categorias().Eliminar(id, out mensaje);

            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }
        #endregion

        //Marca!!!!!!!!!!!!!!!
        #region MARCA

        [HttpGet]
        public JsonResult ListarMarca()
        {
            List<Marca> listaMarca = new List<Marca>();
            listaMarca = new CN_Marcas().Listar();
            return Json(new { data = listaMarca }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarMarca(Marca objMarca)
        {
            object resultado;
            string mensaje = string.Empty;
            if (objMarca.IdMarca == 0)
            {
                resultado = new CN_Marcas().Registrar(objMarca, out mensaje);
            }
            else
            {
                resultado = new CN_Marcas().Editar(objMarca, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult EliminarMarca(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CN_Marcas().Eliminar(id, out mensaje);

            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }
        #endregion

        //Producto!!!!!!!!!!!!
        #region PRODUCTO

        [HttpGet]
        public JsonResult ListarProducto()
        {
            List<Producto> listaProducto = new List<Producto>();
            listaProducto = new CN_Productos().Listar();
            return Json(new { data = listaProducto }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GuardarProducto(string objeto, HttpPostedFileBase archivoImagen)
        {
            
            string mensaje = string.Empty;
            bool operacionExitosa = true;
            bool guardarImagenExitosa = true;
            Producto objProducto = new Producto();

            objProducto = JsonConvert.DeserializeObject<Producto>(objeto);
            decimal precio;
            if(decimal.TryParse(objProducto.PrecioTexto,NumberStyles.AllowDecimalPoint,new CultureInfo("es-AR"), out precio))
            {
                objProducto.Precio = precio;
            }else
            {
                return Json(new { operacionExitosa = false, mensaje = "El formato del precio debe ser ##,##" },JsonRequestBehavior.AllowGet);
            }

            if (objProducto.IdProducto == 0)
            {
                int idProductoGenerado = new CN_Productos().Registrar(objProducto, out mensaje);
                if (idProductoGenerado != 0)
                {
                    objProducto.IdProducto = idProductoGenerado;
                }
                else
                {
                    operacionExitosa = false;
                }
            }
            else
            {
                operacionExitosa = new CN_Productos().Editar(objProducto, out mensaje);
            }

            if (operacionExitosa)
            {
                if(archivoImagen != null)
                {
                    string rutaImagen = ConfigurationManager.AppSettings["ServidorFotos"];
                    string extension = Path.GetExtension(archivoImagen.FileName);
                    string nombreImagen = string.Concat(objProducto.IdProducto.ToString(), extension);

                    try
                    {
                        archivoImagen.SaveAs(Path.Combine(rutaImagen, nombreImagen));
                    }
                    catch (Exception ex) {
                        string msj = ex.Message;
                        guardarImagenExitosa = false;
                    }

                    if (guardarImagenExitosa)
                    {
                        objProducto.Imagen = rutaImagen;
                        objProducto.NombreImagen = nombreImagen;
                        bool respt = new CN_Productos().GuardarInfoImagen(objProducto, out mensaje);
                    }
                    else
                    {
                        mensaje = "Hubo un inconveniente con la imagen";
                    }
                }
            }

            return Json(new { operacionExitosa = operacionExitosa, idGenerado = objProducto.IdProducto, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]

        public JsonResult ImagenProducto(int id)
        {
            bool conversion;

            Producto objProducto = new CN_Productos().Listar().Where(p => p.IdProducto == id).FirstOrDefault();

            string textoBase64 = CN_Recursos.ConvertirBase64(Path.Combine(objProducto.Imagen,objProducto.NombreImagen), out conversion);

            return Json(new
            {  
                conversion = conversion,
                textoBase64 = textoBase64,
                extension =Path.GetExtension(objProducto.NombreImagen)
            },
                
                JsonRequestBehavior.AllowGet

            );
        }

        [HttpPost]
        public JsonResult EliminarProducto(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CN_Productos().Eliminar(id, out mensaje);

            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }

        #endregion
    }
}