using CapaDatos;
using CapaEntidad;
using System;
using System.Text;
using VLDRINKS.CORE; 

namespace CapaNegocio
{
    public class CN_Clientes
    {

        private CD_Clientes objCapaDato = new CD_Clientes();

        public Cliente ValidarCliente(string correo, string clave, out string Mensaje)
        {
            Mensaje = string.Empty;
            Cliente cliente = objCapaDato.ObtenerClientePorCorreo(correo);

            if (cliente == null)
            {
                Mensaje = "Credenciales inválidas.";
                return null;
            }


            bool esValido = PasswordHasher.VerifyPassword(clave, cliente.PasswordHash, cliente.PasswordSalt);

            if (!esValido)
            {
                Mensaje = "Credenciales inválidas.";
                return null;
            }

            return cliente;
        }

        public int Registrar(Cliente obj, out string Mensaje)
        {
            // (Aquí puedes añadir validaciones: que el correo no exista, etc.)


            string passwordEnTexto = Encoding.UTF8.GetString(obj.PasswordHash);


            var (hash, salt) = PasswordHasher.HashPassword(passwordEnTexto);

            obj.PasswordHash = hash;
            obj.PasswordSalt = salt;
            obj.Reestablecer = false;

            return objCapaDato.Registrar(obj, out Mensaje);
        }
    }
}