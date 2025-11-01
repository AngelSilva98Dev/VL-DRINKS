using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using CapaDatos;
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

            // 2. Validacion de mail
            if (usuario == null)
            {
                Mensaje = "Credenciales inválidas.";
                return null; //Mail incorrecto
            }

            // 3. Validacion de Contraseña encriptada
            // Usamos la clase PasswordHasher que creamos
            bool esValido = PasswordHasher.VerifyPassword(clave, usuario.PasswordHash, usuario.PasswordSalt);

            if (!esValido)
            {
                Mensaje = "Credenciales inválidas.";
                return null; // Contraseña incorrecta
            }

            // 4. Ingreso exitoso
            return usuario;
        }
    }
}

