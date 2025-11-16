using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public  class CN_ReportePanel
    {
        private CD_Reporte objCapaDato = new CD_Reporte();

        public PanelControl VerPanel()
        {
            return objCapaDato.VerPanel();
        }

        public List<Reporte> Ventas(string fechainicio, string fechafin, string idtransaccion)
        {
            return objCapaDato.Ventas(fechainicio, fechafin, idtransaccion);
        }


    }
}
