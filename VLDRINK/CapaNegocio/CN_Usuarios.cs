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

        public List<Usuario> Listar(int idUsuarioLogueado, bool esAdminLogueado)
        {
            return objCapaDato.Listar(idUsuarioLogueado, esAdminLogueado);
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

        public bool Modificar(Usuario obj, int idUsuarioLogueado, bool esAdminLogueado, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Nombres) || string.IsNullOrEmpty(obj.Apellidos) || string.IsNullOrEmpty(obj.Correo))
            {
                Mensaje = "No se permiten campos vacíos.";
                return false;
            }

            if (!esAdminLogueado && obj.IdUsuario != idUsuarioLogueado)
            {
                Mensaje = "Error de Permiso: Solo puedes modificar tu propia cuenta.";
                return false;
            }

            if (!esAdminLogueado && obj.esAdmin == true)
            {
                Mensaje = "Error de Permiso: No puedes asignarte el rol de Administrador.";
                return false;
            }

            if (obj.IdUsuario == 1 && !obj.esAdmin)
            {
                Mensaje = "Error de Permiso: No se puede quitar el rol de Admin al usuario principal.";
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

        public bool Eliminar(int idUsuarioAEliminar, bool esAdminActual, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (!esAdminActual)
            {
                Mensaje = "Error de Permiso: Solo un SuperUsuario puede eliminar cuentas.";
                return false;
            }

            if (idUsuarioAEliminar == 1)
            {
                Mensaje = "No se puede eliminar al administrador principal del sistema.";
                return false;
            }

            return objCapaDato.Eliminar(idUsuarioAEliminar, out Mensaje);
        }

        public bool CambiarPassword(int idUsuarioAfectado, string nuevaClave, int idUsuarioLogueado, bool esAdminLogueado, out string Mensaje)
        {
            Mensaje = string.Empty;

            // --- REGLA DE PERMISO ---
            if (!esAdminLogueado && idUsuarioAfectado != idUsuarioLogueado)
            {
                Mensaje = "Error de Permiso: Solo puedes cambiar tu propia contraseña.";
                return false;
            }

            if (string.IsNullOrEmpty(nuevaClave) || nuevaClave.Length < 6)
            {
                Mensaje = "La contraseña debe tener al menos 6 caracteres.";
                return false;
            }

            var (hash, salt) = PasswordHasher.HashPassword(nuevaClave);

            return objCapaDato.CambiarPassword(idUsuarioAfectado, hash, salt, out Mensaje);
        }
    }
}



