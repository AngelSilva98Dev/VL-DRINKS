using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
namespace CapaNegocio
{
    public class CN_Usuarios
    {
        private CD_Usuarios objCapaDato = new CD_Usuarios();

        public List<Usuario> Listar()
        {
            return objCapaDato.Listar();
        }

        public int Registrar(Usuario objeto, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(objeto.Nombres) || string.IsNullOrWhiteSpace(objeto.Nombres))
            {
                Mensaje = "Debe ingresar un nombre";

            }else if (string.IsNullOrEmpty(objeto.Apellidos) || string.IsNullOrWhiteSpace(objeto.Apellidos))
            {
                Mensaje = "Debe ingresar un apellido";

            }else if (string.IsNullOrEmpty(objeto.Correo) || string.IsNullOrWhiteSpace(objeto.Correo))
            {
                Mensaje = "Debe ingresar un correo";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                string clave = CN_Recursos.GenerarPassword();
                string asunto = "Creacion de Cuenta USUARIO(VLDRINKS)";
                string mensajeCorreo = "<h3>Su cuenta fue creada correctamente</h3></br><p>Su contraseña para acceder es: !clave!</p>";
                mensajeCorreo = mensajeCorreo.Replace("!clave!",clave);

                bool respuesta = CN_Recursos.EnviarCorreo(objeto.Correo,asunto,mensajeCorreo);
                if (respuesta)
                {
                    objeto.Clave = CN_Recursos.ConvertirSha256(clave);
                    return objCapaDato.Registrar(objeto, out Mensaje);
                }
                else
                {
                    Mensaje = "No fue posible enviar el correo";
                    return  0;
                }
                
               

            }
            else
            {
                return 0;
            }

        }

        public bool Editar(Usuario objeto, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(objeto.Nombres) || string.IsNullOrWhiteSpace(objeto.Nombres))
            {
                Mensaje = "Debe ingresar un nombre";

            }
            else if (string.IsNullOrEmpty(objeto.Apellidos) || string.IsNullOrWhiteSpace(objeto.Apellidos))
            {
                Mensaje = "Debe ingresar un apellido";

            }
            else if (string.IsNullOrEmpty(objeto.Correo) || string.IsNullOrWhiteSpace(objeto.Correo))
            {
                Mensaje = "Debe ingresar un correo";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDato.Editar(objeto, out Mensaje);

            }else
            {
                return false;
            }

        }

        public bool Eliminar(int id, out string Mensaje)
        {
            return objCapaDato.Eliminar(id, out Mensaje);
        }


        public bool CambiarClave(int idusuario, string clave, out string mensaje)
        {
            return objCapaDato.CambiarClave( idusuario,  clave, out  mensaje);
        }



        public bool ReestablecerClave(int idusuario, string correo, out string mensaje)
        {
            mensaje = string.Empty;
            string nuevaclave = CN_Recursos.GenerarPassword();
            bool resultado = objCapaDato.ReestablecerClave(idusuario, CN_Recursos.ConvertirSha256(nuevaclave), out mensaje);

            if (resultado)
            {
                string asunto = "Contraseña reestablecida USUARIO(VLDRINKS)";
                string mensajeCorreo = "<h3>Su contraseña fue reestablecida correctamente</h3></br><p>Su clave para acceder es: !clave!</p>";
                mensajeCorreo = mensajeCorreo.Replace("!clave!", nuevaclave);
                bool respuesta = CN_Recursos.EnviarCorreo(correo, asunto, mensajeCorreo);
                if (respuesta)
                {
                    return true;
                }
                else
                {
                    mensaje = "No se pudo enviar el correo";
                    return false;
                }
            }
            else
            {
                mensaje = "No se pudo reestablecer la contraseña";
                return false;
            }

            

        }
    }
}
