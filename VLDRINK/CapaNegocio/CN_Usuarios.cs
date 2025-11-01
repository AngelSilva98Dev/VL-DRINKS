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

        public string TestearVerificacion()
        {
            string passwordDePrueba = "pass123";

            // Los datos EXACTOS que están en tu BBDD para 'admin@vldrinks.com'
            byte[] hashGuardado = new byte[] {
        0xCF, 0x13, 0x48, 0xA8, 0xF1, 0x76, 0x00, 0x88, 0x2D, 0x92, 0x1A, 0x22, 0x12, 0x3F, 0x85, 0x96,
        0x61, 0xF4, 0x98, 0xC8, 0x73, 0x43, 0x16, 0xD3, 0x11, 0xC6, 0x2F, 0x23, 0x16, 0xE6, 0xF9, 0x8C,
        0x25, 0x36, 0x76, 0xC3, 0x18, 0x28, 0xF7, 0x31, 0x67, 0x15, 0xB5, 0x00, 0x3C, 0x26, 0x0C, 0x6D,
        0x32, 0xA9, 0xA3, 0xB6, 0x87, 0x1E, 0x89, 0x32, 0xC1, 0xC6, 0x98, 0xF2, 0x4C, 0x31, 0x1C, 0x8D
    };
            byte[] saltGuardado = new byte[] {
        0x81, 0xB7, 0xE5, 0x10, 0x86, 0xA0, 0x10, 0xD8, 0x13, 0x73, 0x97, 0x75, 0x96, 0xB6, 0x13, 0x89,
        0x81, 0x69, 0xF4, 0x17, 0xF7, 0x63, 0x6E, 0x05, 0x3A, 0x26, 0x18, 0x99, 0x91, 0x20, 0x66, 0xD0
    };

            // Llama a tu PasswordHasher estático
            bool esValido = VLDRINKS.CORE.PasswordHasher.VerifyPassword(passwordDePrueba, hashGuardado, saltGuardado);

            if (esValido)
            {
                return "VERIFICACIÓN EXITOSA. El PasswordHasher.cs es correcto (SHA-512).";
            }
            else
            {
                return "FALLO DE VERIFICACIÓN. El PasswordHasher.cs es incorrecto (probablemente usa SHA-1).";
            }
        }

        public Usuario ValidarUsuario(string correo, string clave, out string Mensaje)
        {
            Mensaje = string.Empty;

            // 1. Obtener el usuario desde la Capa de Datos
            Usuario usuario = objCapaDato.ObtenerUsuarioPorCorreo(correo);

            // --- PRUEBA DE DIAGNÓSTICO 1 ---
            if (usuario == null)
            {
                Mensaje = "Error de Depuración: El usuario '" + correo + "' NO FUE ENCONTRADO en la BBDD.";
                return null;
            }

            // --- PRUEBA DE DIAGNÓSTICO 2 ---
            // El usuario sí existe, ahora verificamos la contraseña
            try
            {
                bool esValido = PasswordHasher.VerifyPassword(clave, usuario.PasswordHash, usuario.PasswordSalt);

                if (!esValido)
                {
                    Mensaje = "Error de Depuración: El usuario fue encontrado, pero la contraseña no coincide (VerifyPassword devolvió 'false').";
                    return null; // Contraseña incorrecta
                }
            }
            catch (Exception ex)
            {
                Mensaje = "Error de Depuración: VerifyPassword 'crasheó'. Error: " + ex.Message;
                return null;
            }


            // 4. Devolver el usuario si todo está bien
            Mensaje = "¡ÉXITO!";
            return usuario;
        }

        public int Registrar(Usuario obj, out string Mensaje)
        {
            // 1. Validar que el correo no exista ya
            // (Usamos el método que ya tenías)
            Usuario usuarioExistente = objCapaDato.ObtenerUsuarioPorCorreo(obj.Correo);
            if (usuarioExistente != null)
            {
                Mensaje = "Error: El correo electrónico ya está registrado.";
                return 0; // 0 significa que falló
            }

            // 2. Hashear la contraseña
            // (Aquí usamos 'obj.PasswordHash' como contraseña temporal en texto plano)
            // El controlador nos la pasará así.
            string passwordEnTexto = System.Text.Encoding.UTF8.GetString(obj.PasswordHash);// Esto es un truco temporal
            var (hash, salt) = PasswordHasher.HashPassword(passwordEnTexto);

            // 3. Sobrescribir los datos del objeto 'obj'
            obj.PasswordHash = hash;
            obj.PasswordSalt = salt;
            obj.Activo = true;        // Lo ponemos activo por defecto
            obj.Reestablecer = false; // No necesita reestablecer

            // 4. Llamar a la Capa de Datos para registrar
            return objCapaDato.Registrar(obj, out Mensaje);
        }
    }
}



