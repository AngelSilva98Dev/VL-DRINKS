using CapaDatos;
using CapaEntidad;
using System.Collections.Generic;

namespace CapaNegocio
{
    public class CN_Marcas
    {
        // Usando el nombre de variable 'objCapaDato'
        private CD_Marcas objCapaDato = new CD_Marcas();

        public List<Marca> Listar()
        {
            return objCapaDato.Listar();
        }

        public int Registrar(Marca obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Descripcion))
            {
                Mensaje = "La descripción de la marca no puede estar vacía.";
                return 0;
            }

            return objCapaDato.Registrar(obj, out Mensaje);
        }

        public bool Modificar(Marca obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Descripcion))
            {
                Mensaje = "La descripción de la marca no puede estar vacía.";
                return false;
            }

            return objCapaDato.Modificar(obj, out Mensaje);
        }

        public bool Eliminar(int id, out string Mensaje)
        {
            Mensaje = string.Empty;

            // (Aquí puedes añadir reglas, ej: "No eliminar marca si tiene productos asociados")

            return objCapaDato.Eliminar(id, out Mensaje);
        }
    }
}