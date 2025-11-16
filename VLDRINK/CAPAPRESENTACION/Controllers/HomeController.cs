using CapaEntidad;
using CapaNegocio;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace CAPAPRESENTACION.Controllers
{
    [Authorize]
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






        [HttpGet]

        public JsonResult VistaReporte(string fechainicio,string fechafin, string idtransaccion)
        {
            List<Reporte> objlista = new List<Reporte>();

           objlista = new CN_ReportePanel().Ventas(fechainicio,fechafin,idtransaccion);
            return Json(new { data = objlista }, JsonRequestBehavior.AllowGet);

        }



        [HttpGet]

        public JsonResult VistaPanel()
        {
            PanelControl objeto = new CN_ReportePanel().VerPanel();
            return Json(new { resultado = objeto}, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]

        public FileResult ExportarVentas(string fechainicio, string fechafin, string idtransaccion)
        {
            List<Reporte> objlista = new List<Reporte>();
            objlista = new CN_ReportePanel().Ventas(fechainicio, fechafin, idtransaccion);

            DataTable dt = new DataTable();

            dt.Locale = new System.Globalization.CultureInfo("es-AR");

            dt.Columns.Add("Fecha Venta", typeof(string));
            dt.Columns.Add("Cliente", typeof(string));
            dt.Columns.Add("Producto", typeof(string));
            dt.Columns.Add("Precio", typeof(decimal));
            dt.Columns.Add("Cantidad", typeof(int));
            dt.Columns.Add("Total", typeof(decimal));
            dt.Columns.Add("IdTransaccion", typeof(string));

            foreach(Reporte rp in objlista)
            {
                dt.Rows.Add(new object[]
                {
                    rp.FechaVenta,
                    rp.Cliente,
                    rp.Producto,
                    rp.Precio,
                    rp.Cantidad,
                    rp.Total,
                    rp.IdTransaccion
                });
            }

            dt.TableName = "Datos";

            using(XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(),"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet","ReporteVenta" + DateTime.Now.ToString() + ".xlsx");
                }
            }

        }

    }
}