using CapaDatos;
using CapaEntidad;
using System.Collections.Generic;

namespace CapaNegocio
{
    public class CN_Categorias
    {
        // Recuerda que tu nombre de variable aquí podría ser 'objCapaDato'
        private CD_Categorias objCapaDato = new CD_Categorias();

        public List<Categoria> Listar()
        {
            return objCapaDato.Listar();
        }

        public int Registrar(Categoria obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            // Regla de Negocio: No permitir descripciones vacías
            if (string.IsNullOrEmpty(obj.Descripcion))
            {
                Mensaje = "La descripción de la categoría no puede estar vacía.";
                return 0;
            }

            // (Aquí podrías añadir una validación de 'Descripción duplicada' si quisieras)

            return objCapaDato.Registrar(obj, out Mensaje);
        }

        public bool Modificar(Categoria obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            // Regla de Negocio: No permitir descripciones vacías
            if (string.IsNullOrEmpty(obj.Descripcion))
            {
                Mensaje = "La descripción de la categoría no puede estar vacía.";
                return false;
            }

            // (Aquí podrías añadir la validación de duplicados al modificar)

            return objCapaDato.Modificar(obj, out Mensaje);
        }

        public bool Eliminar(int id, out string Mensaje)
        {
            Mensaje = string.Empty;

            // (Aquí podrías añadir reglas, ej: "No eliminar categoría si tiene productos asociados")

            return objCapaDato.Eliminar(id, out Mensaje);
        }
    }
}