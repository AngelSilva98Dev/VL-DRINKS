using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public  class CN_Productos
    {
        private CD_Productos objCapaDato = new CD_Productos();

        public List<Producto> Listar()
        {
            return objCapaDato.Listar();
        }


        public int Registrar(Producto objeto, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(objeto.Nombre) || string.IsNullOrWhiteSpace(objeto.Nombre))
            {
                Mensaje = "Debe ingresar un nombre";

            }
            else if (string.IsNullOrEmpty(objeto.Descripcion) || string.IsNullOrWhiteSpace(objeto.Descripcion))
            {
                Mensaje = "Debe ingresar una descripcion";

            }
            else if (objeto.objMarca.IdMarca == 0)
            {
                Mensaje = "Debe seleccionar una marca";

            }
            else if (objeto.objCategoria.IdCategoria == 0)
            {
                Mensaje = "Debe seleccionar una categoria";

            }
            else if (objeto.Precio == 0)
            {
                Mensaje = "Debe ingresar el precio del producto";

            }
            if (objeto.Stock == 0)
            {
                Mensaje = "Debe ingresar una cantidad correcta";

            }

            if (string.IsNullOrEmpty(Mensaje))
            {

                return objCapaDato.Registrar(objeto, out Mensaje);


            }
            else
            {
                return 0;
            }

        }

        public bool Editar(Producto objeto, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(objeto.Nombre) || string.IsNullOrWhiteSpace(objeto.Nombre))
            {
                Mensaje = "Debe ingresar un nombre";

            }
            else if (string.IsNullOrEmpty(objeto.Descripcion) || string.IsNullOrWhiteSpace(objeto.Descripcion))
            {
                Mensaje = "Debe ingresar una descripcion";

            }
            else if (objeto.objMarca.IdMarca == 0)
            {
                Mensaje = "Debe seleccionar una marca";

            }
            else if (objeto.objCategoria.IdCategoria == 0)
            {
                Mensaje = "Debe seleccionar una categoria";

            }
            else if (objeto.Precio == 0)
            {
                Mensaje = "Debe ingresar el precio del producto";

            }
            if (objeto.Stock == 0)
            {
                Mensaje = "Debe ingresar una cantidad correcta";

            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDato.Editar(objeto, out Mensaje);

            }
            else
            {
                return false;
            }

        }

        public bool GuardarInfoImagen(Producto objProducto, out string Mensaje)
        {
            return objCapaDato.GuardarInfoImagen(objProducto, out Mensaje);
        }

        public bool Eliminar(int id, out string Mensaje)
        {
            return objCapaDato.Eliminar(id, out Mensaje);
        }
    }
}
