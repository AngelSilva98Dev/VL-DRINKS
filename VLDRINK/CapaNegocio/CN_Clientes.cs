using CapaDatos;
using CapaEntidad;
using System;
using System.Linq;
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
            Mensaje = string.Empty;


            Cliente clienteExistente = objCapaDato.ObtenerClientePorCorreo(obj.Correo);
            if (clienteExistente != null)
            {
                Mensaje = "Error: El correo electrónico ya está registrado.";
                return 0; 
            }

            string passwordEnTexto = Encoding.UTF8.GetString(obj.PasswordHash);

            var (hash, salt) = PasswordHasher.HashPassword(passwordEnTexto);

            obj.PasswordHash = hash;
            obj.PasswordSalt = salt;
            obj.Reestablecer = false; 

            return objCapaDato.Registrar(obj, out Mensaje);
        }
        public Cliente ObtenerClientePorId(int idCliente)
        {

            return objCapaDato.ObtenerClientePorId(idCliente);
        }

        public bool SolicitarReestablecimiento(string correo, out string Mensaje)
        {
            Mensaje = string.Empty;

            Cliente cliente = objCapaDato.ObtenerClientePorCorreo(correo);
            if (cliente == null)
            {
                Mensaje = "No se encontró ningún usuario con esa dirección de correo.";
                return false;
            }

            if (cliente.FechaUltimoReinicio.HasValue) 
            {
                TimeSpan diferencia = DateTime.Now - cliente.FechaUltimoReinicio.Value;

                if (diferencia.TotalMinutes < 2)
                {
                    int segundosRestantes = 120 - (int)diferencia.TotalSeconds;

                    Mensaje = $"TIEMPO_ESPERA|{segundosRestantes}";
                    return false; 
                }
            }

            string nuevaClave = GenerarClaveAleatoria(8);

            var (hash, salt) = PasswordHasher.HashPassword(nuevaClave);

            bool actualizado = objCapaDato.ActualizarPassword(cliente.IdCliente, hash, salt, out Mensaje);

            if (!actualizado)
            {
                Mensaje = "Error al actualizar la contraseña en la base de datos.";
                return false;
            }

            string cuerpoEmail = $"Hola {cliente.Nombres},<br><br>Se ha reestablecido tu contraseña para la tienda VL-DRINKS.<br><br>Tu nueva contraseña es: <b>{nuevaClave}</b>";
            bool emailEnviado = ServicioEmail.Enviar(cliente.Correo, "VL-DRINKS Tienda - Contraseña Reestablecida", cuerpoEmail);

            if (!emailEnviado)
            {
                Mensaje = "Contraseña actualizada en BBDD, pero falló el envío del email. (Revisa la configuración SMTP).";
                return true; 
            }

            Mensaje = "¡Éxito! Se ha enviado una nueva contraseña a tu correo.";
            return true;
        }

        private string GenerarClaveAleatoria(int longitud)
        {
            const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnpqrstuvwxyz23456789";
            Random random = new Random();

            return new string(Enumerable.Repeat(chars, longitud)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        private bool EnviarEmail(string correo, string asunto, string cuerpoHtml)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("========================================");
                System.Diagnostics.Debug.WriteLine("--- SIMULACIÓN DE ENVÍO DE EMAIL (TIENDA) ---");
                System.Diagnostics.Debug.WriteLine($"Para: {correo}");
                System.Diagnostics.Debug.WriteLine($"Asunto: {asunto}");
                System.Diagnostics.Debug.WriteLine($"Cuerpo: {cuerpoHtml}");
                System.Diagnostics.Debug.WriteLine("========================================");

                return true; 
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al simular email: {ex.Message}");
                return false;
            }
        }

    }
}