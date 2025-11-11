using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using CapaDatos;

namespace CapaNegocio
{
    public  class CN_Categorias
    {
        private CD_Categorias objCapaDato = new CD_Categorias();
        
        public List<Categoria> Listar()
        {
            return objCapaDato.Listar();
        }

        public int Registrar(Categoria objeto, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(objeto.Descripcion) || string.IsNullOrWhiteSpace(objeto.Descripcion))
            {
                Mensaje = "Debe ingresar una categoria";

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

        public bool Editar(Categoria objeto, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(objeto.Descripcion) || string.IsNullOrWhiteSpace(objeto.Descripcion))
            {
                Mensaje = "Debe ingresar una categoria";

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


        public bool Eliminar(int id, out string Mensaje)
        {
            return objCapaDato.Eliminar(id, out Mensaje);
        }

    }
}
