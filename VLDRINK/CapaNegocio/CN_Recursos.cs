using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public  class CN_Recursos
    {
        //hasheo de contraseña

        public static string ConvertirSha256(string texto)
        {
            StringBuilder sb = new StringBuilder();
            
            using(SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] resultado = hash.ComputeHash(enc.GetBytes(texto));

                foreach (byte b in resultado)
                {
                    sb.Append(b.ToString("x2"));
                }
            }

            return sb.ToString();
        }
    }
}
