using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public  class CD_Reporte
    {
        public List<Reporte> Ventas(string fechainicio, string fechafin, string idtransaccion)
        {
            List<Reporte> lista = new List<Reporte>();

            try
            {
                using (SqlConnection objConexion = new SqlConnection(Conexion.conex))
                {
                    

                    SqlCommand comando = new SqlCommand("sp_ReporteVentas", objConexion);
                    comando.Parameters.AddWithValue("fechainicio", fechainicio);
                    comando.Parameters.AddWithValue("fechafin", fechafin);
                    comando.Parameters.AddWithValue("idtransaccion", idtransaccion);
                    comando.CommandType = CommandType.StoredProcedure;

                    objConexion.Open();

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            lista.Add(new Reporte
                            {
                                FechaVenta = lector["FechaVenta"].ToString(),
                                Cliente = lector["Cliente"].ToString(),
                                Producto = lector["Producto"].ToString(),
                                Precio = Convert.ToDecimal(lector["Precio"], new CultureInfo("es-AR")),
                                Cantidad = Convert.ToInt32(lector["Cantidad"].ToString()),
                                Total= Convert.ToDecimal(lector["Total"], new CultureInfo("es-AR")),
                                IdTransaccion = lector["IdTransaccion"].ToString()



                            });
                        }
                    }

                }
            }
            catch
            {
                lista = new List<Reporte>();
            }

            return lista;
        }



        public PanelControl VerPanel()
        {
            PanelControl objPanel = new PanelControl();

            try
            {
                using (SqlConnection objConexion = new SqlConnection(Conexion.conex))
                {


                    SqlCommand comando = new SqlCommand("sp_ReportePanel", objConexion);
                    comando.CommandType = CommandType.StoredProcedure;

                    objConexion.Open();

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            objPanel = new PanelControl()
                            {
                                TotalCliente= Convert.ToInt32(lector["TotalCliente"]),
                                TotalVenta = Convert.ToInt32(lector["TotalVenta"]),
                                TotalProducto = Convert.ToInt32(lector["TotalProducto"]),

                            };
                        }
                    }

                }
            }
            catch
            {
                objPanel = new PanelControl();
            }

            return objPanel;
        }
    }
}
