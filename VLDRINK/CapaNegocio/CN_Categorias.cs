using CapaDatos;
using CapaEntidad;
using System.Collections.Generic;

namespace CapaNegocio
{
    public class CN_Categorias
    {
        private CD_Categorias objCapaDato = new CD_Categorias();
        public List<Categoria> Listar()
        {
            return objCapaDato.Listar();
        }

        public int Registrar(Categoria obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Descripcion))
            {
                Mensaje = "La descripción de la categoría no puede estar vacía.";
                return 0;
            }

            return objCapaDato.Registrar(obj, out Mensaje);
        }

        public bool Modificar(Categoria obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Descripcion))
            {
                Mensaje = "La descripción de la categoría no puede estar vacía.";
                return false;
            }


            return objCapaDato.Modificar(obj, out Mensaje);
        }

        public bool Eliminar(int id, out string Mensaje)
        {
            Mensaje = string.Empty;


            return objCapaDato.Eliminar(id, out Mensaje);
        }
    }
}