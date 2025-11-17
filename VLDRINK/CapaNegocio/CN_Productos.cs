using CapaDatos;
using CapaEntidad;
using System.Collections.Generic;

namespace CapaNegocio
{
    public class CN_Productos
    {
        private CD_Productos objCapaDato = new CD_Productos();

        public List<Producto> Listar(bool incluirImagen = false)
        {
            return objCapaDato.Listar(incluirImagen);
        }

        public int Registrar(Producto obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Nombre))
            {
                Mensaje = "El nombre del producto no puede ser vacío.";
                return 0;
            }
            if (obj.objMarca.IdMarca == 0)
            {
                Mensaje = "Debe seleccionar una marca.";
                return 0;
            }
            if (obj.objCategoria.IdCategoria == 0)
            {
                Mensaje = "Debe seleccionar una categoría.";
                return 0;
            }
            if (string.IsNullOrEmpty(obj.Imagen) || string.IsNullOrEmpty(obj.NombreImagen))
            {
                Mensaje = "Debe seleccionar una imagen para el producto.";
                return 0;
            }

            return objCapaDato.Registrar(obj, out Mensaje);
        }

        public bool Modificar(Producto obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Nombre))
            {
                Mensaje = "El nombre del producto no puede ser vacío.";
                return false;
            }
            if (obj.objMarca.IdMarca == 0)
            {
                Mensaje = "Debe seleccionar una marca.";
                return false;
            }
            if (obj.objCategoria.IdCategoria == 0)
            {
                Mensaje = "Debe seleccionar una categoría.";
                return false;
            }

            return objCapaDato.Modificar(obj, out Mensaje);
        }

        public bool Eliminar(int id, out string Mensaje)
        {
            Mensaje = string.Empty;
            return objCapaDato.Eliminar(id, out Mensaje);
        }

        public Producto ObtenerProducto(int id)
        {
            return objCapaDato.ObtenerProducto(id);
        }

    }
}