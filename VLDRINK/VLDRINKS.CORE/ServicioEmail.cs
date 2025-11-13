using System;
using System.Configuration; 
using System.Net;
using System.Net.Mail;

namespace VLDRINKS.CORE
{
    public static class ServicioEmail
    {
        public static bool Enviar(string correoDestino, string asunto, string cuerpoHtml)
        {
            // Lee la configuración desde el Web.config
            string servidor = ConfigurationManager.AppSettings["EmailServidor"];
            int puerto = Convert.ToInt32(ConfigurationManager.AppSettings["EmailPuerto"]);
            string usuario = ConfigurationManager.AppSettings["EmailUsuario"];
            string clave = ConfigurationManager.AppSettings["EmailClave"];

            try
            {
                // Crea el mensaje de correo
                MailMessage mail = new MailMessage();
                // Usamos el 'usuario' como remitente
                mail.From = new MailAddress(usuario, "VLDRINKS Admin");
                mail.To.Add(correoDestino);
                mail.Subject = asunto;
                mail.Body = cuerpoHtml;
                mail.IsBodyHtml = true;

                // Configura el cliente SMTP 
                SmtpClient smtp = new SmtpClient(servidor, puerto);
                // Se autentica con la "Contraseña de Aplicación"
                smtp.Credentials = new NetworkCredential(usuario, clave);
                smtp.EnableSsl = true; // Gmail requiere SSL
                smtp.Timeout = 20000; // 20 segundos


                smtp.Send(mail);

                return true; 
            }
            catch (Exception ex)
            {  
                System.Diagnostics.Debug.WriteLine("ERROR AL ENVIAR EMAIL:");
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false; 
            }
        }
    }
}