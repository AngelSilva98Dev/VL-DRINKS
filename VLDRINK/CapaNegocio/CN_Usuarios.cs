using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using VLDRINKS.CORE;

namespace CapaNegocio
{
    public class CN_Usuarios
    {
        private CD_Usuarios objCapaDato = new CD_Usuarios();

        public List<Usuario> Listar()
        {
            return objCapaDato.Listar();
        }

       

        public Usuario ValidarUsuario(string correo, string clave, out string Mensaje)
        {
            Mensaje = string.Empty;

            Usuario usuario = objCapaDato.ObtenerUsuarioPorCorreo(correo);

            if (usuario == null)
            {
                Mensaje = "Credenciales inválidas";
                return null;
            }
            try
            {
                bool esValido = PasswordHasher.VerifyPassword(clave, usuario.PasswordHash, usuario.PasswordSalt);

                if (!esValido)
                {
                    Mensaje = "Credenciales inválidas";
                    return null; 
                }
            }
            catch (Exception ex)
            {
                Mensaje = "Credenciales inválidas" + ex.Message;
                return null;
            }



            Mensaje = "Bienvenido" + usuario.Nombres;
            return usuario;
        }

        public int Registrar(Usuario obj, out string Mensaje)
        {

            Usuario usuarioExistente = objCapaDato.ObtenerUsuarioPorCorreo(obj.Correo);
            if (usuarioExistente != null)
            {
                Mensaje = "Error: El correo electrónico ya está registrado.";
                return 0; 
            }


            string passwordEnTexto = System.Text.Encoding.UTF8.GetString(obj.PasswordHash);
            var (hash, salt) = PasswordHasher.HashPassword(passwordEnTexto);


            obj.PasswordHash = hash;
            obj.PasswordSalt = salt;
            obj.Activo = true;       
            obj.Reestablecer = false; 


            return objCapaDato.Registrar(obj, out Mensaje);
        }

        public int RegistrarAdmin(Usuario obj, out string Mensaje)
        {
            Mensaje = string.Empty;


            if (objCapaDato.ObtenerUsuarioPorCorreo(obj.Correo) != null)
            {
                Mensaje = "Error: El correo electrónico ya está registrado.";
                return 0; 
            }


            string passwordEnTexto = "contraseña";
            var (hash, salt) = PasswordHasher.HashPassword(passwordEnTexto);


            obj.PasswordHash = hash;
            obj.PasswordSalt = salt;
            obj.Reestablecer = false; 


            return objCapaDato.Registrar(obj, out Mensaje);
        }

        public bool Modificar(Usuario obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Nombres) || string.IsNullOrEmpty(obj.Apellidos) || string.IsNullOrEmpty(obj.Correo))
            {
                Mensaje = "No se permiten campos vacíos.";
                return false;
            }

            Usuario usuarioExistente = objCapaDato.ObtenerUsuarioPorCorreo(obj.Correo);

            if (usuarioExistente != null && usuarioExistente.IdUsuario != obj.IdUsuario)
            {
                Mensaje = "El correo electrónico ya está en uso por otro usuario.";
                return false;
            }

            return objCapaDato.Modificar(obj, out Mensaje);
        }

        public bool Eliminar(int id, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (id == 0)
            {
                Mensaje = "No se ha proporcionado un ID de usuario.";
                return false;
            }

            if (id == 1)
            {
                Mensaje = "No se puede eliminar al administrador principal del sistema.";
                return false;
            }

            return objCapaDato.Eliminar(id, out Mensaje);
        }
    }
}



