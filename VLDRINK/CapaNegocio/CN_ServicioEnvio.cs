using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VLDRINKS.CORE;

namespace CapaNegocio
{
    public class ServicioEnvio
    {
        public decimal ObtenerCostoEnvio()
        {
            var config = ConfigEnvioService.Leer();
            return config.CostoEnvio;
        }

        public void ActualizarCostoEnvio(decimal nuevoCosto)
        {
            var config = new ConfigEnvio()
            {
                CostoEnvio = nuevoCosto
            };

            ConfigEnvioService.Guardar(config);
        }
    }
}
