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
        public List<Usuario> Listar(int idUsuarioLogueado, bool esAdminLogueado)
        {
            List<Usuario> lista = new List<Usuario>();
            try
            {
                using (SqlConnection objConexion = new SqlConnection(Conexion.conex))
                {
                    // La consulta base
                    string consulta = "select IdUsuario, Nombres, Apellidos, Correo, Activo, Reestablecer, esAdmin from USUARIO";

                    // --- LÓGICA DE FILTRADO ---
                    // Si el usuario NO es admin, le añadimos un WHERE
                    // para que solo pueda ver su propia fila.
                    if (!esAdminLogueado)
                    {
                        consulta += " WHERE IdUsuario = @IdUsuarioLogueado";
                    }

                    SqlCommand comando = new SqlCommand(consulta, objConexion);

                    if (!esAdminLogueado)
                    {
                        comando.Parameters.AddWithValue("@IdUsuarioLogueado", idUsuarioLogueado);
                    }

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
                                Activo = Convert.ToBoolean(lector["Activo"]),
                                Reestablecer = Convert.ToBoolean(lector["Reestablecer"]),
                                esAdmin = Convert.ToBoolean(lector["esAdmin"])
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lista = new List<Usuario>();
            }
            return lista;
        }

        public Usuario ObtenerUsuarioPorCorreo(string correo)
        {
            Usuario usuario = null; 
            try
            {
                using (SqlConnection objConexion = new SqlConnection(Conexion.conex))
                {
                    string consulta = "SELECT IdUsuario, Nombres, Apellidos, Correo, Reestablecer, Activo, esAdmin , FechaUltimoReinicio ,PasswordHash, PasswordSalt " +
                                      "FROM USUARIO WHERE Correo = @correo";

                    SqlCommand comando = new SqlCommand(consulta, objConexion);
                    comando.Parameters.AddWithValue("@correo", correo);
                    comando.CommandType = CommandType.Text;

                    objConexion.Open();

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        if (lector.Read()) 
                        {
                            usuario = new Usuario
                            {
                                IdUsuario = Convert.ToInt32(lector["IdUsuario"]),
                                Nombres = lector["Nombres"].ToString(),
                                Apellidos = lector["Apellidos"].ToString(),
                                Correo = lector["Correo"].ToString(),
                                Reestablecer = Convert.ToBoolean(lector["Reestablecer"]),
                                Activo = Convert.ToBoolean(lector["Activo"]),
                                esAdmin = Convert.ToBoolean(lector["esAdmin"]),


                                //LECTOR NULL RESTABLECIMIENTO DE PASSWORD
                                FechaUltimoReinicio = lector["FechaUltimoReinicio"] == DBNull.Value
                                            ? (DateTime?)null 
                                            : Convert.ToDateTime(lector["FechaUltimoReinicio"]),


                                //LECTORES NULL ENCRIPTADO PASSWORD
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

                    string consulta = "INSERT INTO USUARIO(Nombres, Apellidos, Correo, Activo, Reestablecer, esAdmin, PasswordHash, PasswordSalt) " +
                              "VALUES(@Nombres, @Apellidos, @Correo, @Activo, @Reestablecer, @esAdmin, @PasswordHash, @PasswordSalt);" +
                              "SELECT SCOPE_IDENTITY();";

                    SqlCommand comando = new SqlCommand(consulta, objConexion);

                    comando.Parameters.AddWithValue("@Nombres", obj.Nombres);
                    comando.Parameters.AddWithValue("@Apellidos", obj.Apellidos);
                    comando.Parameters.AddWithValue("@Correo", obj.Correo);
                    comando.Parameters.AddWithValue("@Activo", obj.Activo);
                    comando.Parameters.AddWithValue("@Reestablecer", obj.Reestablecer);
                    comando.Parameters.AddWithValue("@esAdmin", obj.esAdmin); 
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
                                              "Activo = @Activo, " +
                                              "esAdmin = @esAdmin " +
                                              "WHERE IdUsuario = @IdUsuario";

                            SqlCommand comando = new SqlCommand(consulta, objConexion);
                            comando.Parameters.AddWithValue("@IdUsuario", obj.IdUsuario);
                            comando.Parameters.AddWithValue("@Nombres", obj.Nombres);
                            comando.Parameters.AddWithValue("@Apellidos", obj.Apellidos);
                            comando.Parameters.AddWithValue("@Correo", obj.Correo);
                            comando.Parameters.AddWithValue("@Activo", obj.Activo);
                            comando.Parameters.AddWithValue("@esAdmin", obj.esAdmin);
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

        public bool CambiarPassword(int idUsuario, byte[] passwordHash, byte[] passwordSalt, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection objConexion = new SqlConnection(Conexion.conex))
                {
                    string consulta = "UPDATE USUARIO SET PasswordHash = @PasswordHash, PasswordSalt = @PasswordSalt WHERE IdUsuario = @IdUsuario";

                    SqlCommand comando = new SqlCommand(consulta, objConexion);
                    comando.Parameters.AddWithValue("@IdUsuario", idUsuario);
                    comando.Parameters.AddWithValue("@PasswordHash", passwordHash);
                    comando.Parameters.AddWithValue("@PasswordSalt", passwordSalt);
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

        public bool ActualizarPassword(int idUsuario, byte[] passwordHash, byte[] passwordSalt, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection objConexion = new SqlConnection(Conexion.conex))
                {
                    string consulta = "UPDATE USUARIO SET " +
                                      "PasswordHash = @PasswordHash, " +
                                      "PasswordSalt = @PasswordSalt, " +
                                      "Reestablecer = 0, " +
                                      "FechaUltimoReinicio = GETDATE() " +
                                      "WHERE IdUsuario = @IdUsuario";

                    SqlCommand comando = new SqlCommand(consulta, objConexion);
                    comando.Parameters.AddWithValue("@IdUsuario", idUsuario);
                    comando.Parameters.AddWithValue("@PasswordHash", passwordHash);
                    comando.Parameters.AddWithValue("@PasswordSalt", passwordSalt);
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