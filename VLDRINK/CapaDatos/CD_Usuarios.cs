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
    public class CD_Usuarios
    {
        public List<Usuario> Listar()
        {
            List<Usuario> lista = new List<Usuario>();

            try
            {
                using (SqlConnection objConexion = new SqlConnection(Conexion.conex))
                {
                    string consulta = "select IdUsuario, Nombres, Apellidos, Correo, Reestablecer, Activo from USUARIO";

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
                                Reestablecer = Convert.ToBoolean(lector["Reestablecer"]),
                                Activo = Convert.ToBoolean(lector["Activo"]),
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

        public Usuario ObtenerUsuarioPorCorreo(string correo)
        {
            Usuario usuario = null; // Inicia como nulo
            try
            {
                using (SqlConnection objConexion = new SqlConnection(Conexion.conex))
                {
                    // Traemos TODOS los datos, incluido Hash y Salt
                    string consulta = "SELECT IdUsuario, Nombres, Apellidos, Correo, Reestablecer, Activo, PasswordHash, PasswordSalt FROM USUARIO WHERE Correo = @correo";

                    SqlCommand comando = new SqlCommand(consulta, objConexion);
                    comando.Parameters.AddWithValue("@correo", correo);
                    comando.CommandType = CommandType.Text;

                    objConexion.Open();

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        if (lector.Read()) // Si encontramos al usuario
                        {
                            usuario = new Usuario
                            {
                                IdUsuario = Convert.ToInt32(lector["IdUsuario"]),
                                Nombres = lector["Nombres"].ToString(),
                                Apellidos = lector["Apellidos"].ToString(),
                                Correo = lector["Correo"].ToString(),
                                Reestablecer = Convert.ToBoolean(lector["Reestablecer"]),
                                Activo = Convert.ToBoolean(lector["Activo"]),
                                PasswordHash = (byte[])lector["PasswordHash"],
                                PasswordSalt = (byte[])lector["PasswordSalt"]
                            };
                        }
                    }
                }
            }
            catch
            {
                usuario = null; // Si hay un error, devolvemos nulo
            }
            return usuario;
        }

    }

}
