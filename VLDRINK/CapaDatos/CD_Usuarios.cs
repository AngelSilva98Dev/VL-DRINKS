using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using System.Data.SqlClient;
using System.Data;

namespace CapaDatos
{
    public  class CD_Usuarios
    {
        public List<Usuario> Listar()
        {
            List<Usuario> lista = new List<Usuario>();

            try
            {
                using (SqlConnection objConexion = new SqlConnection(Conexion.conex))
                {
                    string consulta = "select IdUsuario, Nombres, Apellidos, Correo, Clave, Activo, Reestablecer from USUARIO";

                    SqlCommand comando = new SqlCommand(consulta, objConexion);
                    comando.CommandType = CommandType.Text;

                    objConexion.Open();

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            lista.Add(new Usuario
                            {
                                IdUsuario = Convert.ToInt32(lector["IdUsuario"]),
                                Nombres = lector["Nombres"].ToString(),
                                Apellidos = lector["Apellidos"].ToString(),
                                Correo = lector["Correo"].ToString(),
                                Clave = lector["Clave"].ToString(),
                                Activo = Convert.ToBoolean(lector["Activo"]),
                                Reestablecer = Convert.ToBoolean(lector["Reestablecer"])

                            });
                        }
                    }

                }
            }
            catch
            {
                lista = new List<Usuario>();
            }

            return lista;
        }
    }
}
