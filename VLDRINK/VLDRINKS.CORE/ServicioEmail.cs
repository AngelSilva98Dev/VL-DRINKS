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
            string servidor = ConfigurationManager.AppSettings["EmailServidor"];
            int puerto = Convert.ToInt32(ConfigurationManager.AppSettings["EmailPuerto"]);
            string usuario = ConfigurationManager.AppSettings["EmailUsuario"];
            string clave = ConfigurationManager.AppSettings["EmailClave"];

            try
            {
                MailMessage mail = new MailMessage();

                mail.From = new MailAddress(usuario, "VLDRINKS Admin");
                mail.To.Add(correoDestino);
                mail.Subject = asunto;
                mail.Body = cuerpoHtml;
                mail.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient(servidor, puerto);

                smtp.Credentials = new NetworkCredential(usuario, clave);
                smtp.EnableSsl = true; 
                smtp.Timeout = 20000; 


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