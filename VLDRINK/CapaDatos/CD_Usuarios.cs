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

                    string consulta = "INSERT INTO USUARIO(Nombres, Apellidos, Correo, Activo, Reestablecer, PasswordHash, PasswordSalt) " +
                                      "VALUES(@Nombres, @Apellidos, @Correo, @Activo, @Reestablecer, @PasswordHash, @PasswordSalt);" +
                                      "SELECT SCOPE_IDENTITY();"; 

                    SqlCommand comando = new SqlCommand(consulta, objConexion);

                    
                    comando.Parameters.AddWithValue("@Nombres", obj.Nombres);
                    comando.Parameters.AddWithValue("@Apellidos", obj.Apellidos);
                    comando.Parameters.AddWithValue("@Correo", obj.Correo);
                    comando.Parameters.AddWithValue("@Activo", obj.Activo);
                    comando.Parameters.AddWithValue("@Reestablecer", obj.Reestablecer);

                 
                    comando.Parameters.AddWithValue("@PasswordHash", obj.PasswordHash);
                    comando.Parameters.AddWithValue("@PasswordSalt", obj.PasswordSalt);

                    comando.CommandType = CommandType.Text;

                    objConexion.Open();


                    idUsuarioGenerado = Convert.ToInt32(comando.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                idUsuarioGenerado = 0;
                Mensaje = ex.Message; 
            }

            return idUsuarioGenerado;
        }

 


public bool Modificar(Usuario obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection objConexion = new SqlConnection(Conexion.conex))
                {

                    string consulta = "UPDATE USUARIO SET " +
                                      "Nombres = @Nombres, " +
                                      "Apellidos = @Apellidos, " +
                                      "Correo = @Correo, " +
                                      "Activo = @Activo " +
                                      "WHERE IdUsuario = @IdUsuario";

                    SqlCommand comando = new SqlCommand(consulta, objConexion);
                    comando.Parameters.AddWithValue("@IdUsuario", obj.IdUsuario);
                    comando.Parameters.AddWithValue("@Nombres", obj.Nombres);
                    comando.Parameters.AddWithValue("@Apellidos", obj.Apellidos);
                    comando.Parameters.AddWithValue("@Correo", obj.Correo);
                    comando.Parameters.AddWithValue("@Activo", obj.Activo);
                    comando.CommandType = CommandType.Text;

                    objConexion.Open();

                    resultado = comando.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
                resultado = false;
            }
            return resultado;
        }

        public bool Eliminar(int id, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection objConexion = new SqlConnection(Conexion.conex))
                {
                    string consulta = "DELETE FROM USUARIO WHERE IdUsuario = @IdUsuario";

                    SqlCommand comando = new SqlCommand(consulta, objConexion);
                    comando.Parameters.AddWithValue("@IdUsuario", id);
                    comando.CommandType = CommandType.Text;

                    objConexion.Open();

                    resultado = comando.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
                resultado = false;
            }
            return resultado;
        }
    }
}