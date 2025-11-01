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

                                // --- FORMA MÁS SEGURA ---
                                // Revisa si es nulo antes de convertirlo
                                PasswordHash = lector["PasswordHash"] == DBNull.Value
                        ? null
                        : (byte[])lector["PasswordHash"],

                                PasswordSalt = lector["PasswordSalt"] == DBNull.Value
                        ? null
                        : (byte[])lector["PasswordSalt"]
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Lanza el error para que podamos verlo
                throw ex;
            }
            return usuario;
        }

    

        public int Registrar(Usuario obj, out string Mensaje)
        {
            int idUsuarioGenerado = 0;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection objConexion = new SqlConnection(Conexion.conex))
                {
                    // Usamos un Stored Procedure (SP) o un comando SQL directo.
                    // Es más seguro usar un SP, pero para este ejemplo,
                    // un comando parametrizado es suficiente.

                    // Esta consulta inserta el usuario y DEVUELVE el ID nuevo
                    string consulta = "INSERT INTO USUARIO(Nombres, Apellidos, Correo, Activo, Reestablecer, PasswordHash, PasswordSalt) " +
                                      "VALUES(@Nombres, @Apellidos, @Correo, @Activo, @Reestablecer, @PasswordHash, @PasswordSalt);" +
                                      "SELECT SCOPE_IDENTITY();"; // Devuelve el último ID insertado

                    SqlCommand comando = new SqlCommand(consulta, objConexion);

                    // Pasamos los parámetros de forma segura
                    comando.Parameters.AddWithValue("@Nombres", obj.Nombres);
                    comando.Parameters.AddWithValue("@Apellidos", obj.Apellidos);
                    comando.Parameters.AddWithValue("@Correo", obj.Correo);
                    comando.Parameters.AddWithValue("@Activo", obj.Activo);
                    comando.Parameters.AddWithValue("@Reestablecer", obj.Reestablecer);

                    // Pasamos los byte[]
                    comando.Parameters.AddWithValue("@PasswordHash", obj.PasswordHash);
                    comando.Parameters.AddWithValue("@PasswordSalt", obj.PasswordSalt);

                    // Le decimos a C# que esperamos un solo valor de vuelta (el ID)
                    comando.CommandType = CommandType.Text;

                    objConexion.Open();

                    // ExecuteScalar se usa para obtener el primer valor (el ID)
                    idUsuarioGenerado = Convert.ToInt32(comando.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                idUsuarioGenerado = 0;
                Mensaje = ex.Message; // Capturamos el error real
            }

            return idUsuarioGenerado;
        }

    }

}