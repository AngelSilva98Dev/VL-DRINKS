using CapaEntidad;
using CapaNegocio;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using System;

namespace CapaPresentacionTienda.Controllers
{
    public class TiendaController : Controller
    {
        public ActionResult Index(int? pagina, string categoria = "all")
        {
            CN_Categorias objCN_Categorias = new CN_Categorias();
            CN_Productos objCN_Productos = new CN_Productos();
            TiendaViewModel viewModel = new TiendaViewModel();

            viewModel.Categorias = objCN_Categorias.Listar().Where(c => c.Activo).ToList();

            List<Producto> listaCompleta = objCN_Productos.Listar(true).Where(p => p.Activo).ToList();


            if (categoria != "all")
            {
                listaCompleta = listaCompleta
                    .Where(p => p.objCategoria.Descripcion == categoria)
                    .ToList();
            }

            int pageNumber = (pagina ?? 1);
            int pageSize = 10;
            viewModel.Productos = listaCompleta.ToPagedList(pageNumber, pageSize);

            ViewBag.CategoriaActual = categoria;

            return View(viewModel);
        }

        private Carrito GetCarrito()
        {
            Carrito carrito = (Carrito)Session["Carrito"];
            if (carrito == null)
            {
                carrito = new Carrito();
                Session["Carrito"] = carrito;
            }
            return carrito;
        }

        public ActionResult Carrito()
        {
            Carrito carrito = GetCarrito();

            return View(carrito);
        }

        [HttpPost]
        public JsonResult AgregarAlCarrito(int idProducto, int cantidad)
        {
            try
            {
                CN_Productos objCN_Productos = new CN_Productos();
                Producto producto = objCN_Productos.ObtenerProducto(idProducto);

                if (producto == null)
                    return Json(new { success = false, message = "Producto no encontrado." });

                if (producto.Stock < cantidad)
                    return Json(new { success = false, message = "No hay stock suficiente." });

                Carrito carrito = GetCarrito();
                carrito.AgregarItem(producto, cantidad);

                bool sinStock = producto.Stock - cantidad <= 0;

                return Json(new
                {
                    success = true,
                    totalItems = carrito.Items.Count,
                    sinStock = sinStock
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult EliminarDelCarrito(int idProducto)
        {
            try
            {
                Carrito carrito = GetCarrito();

                carrito.EliminarItem(idProducto);

                return Json(new
                {
                    success = true,
                    totalItems = carrito.Items.Count,
                    montoTotal = carrito.CalcularTotal() 
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}