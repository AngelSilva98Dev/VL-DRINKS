using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CapaEntidad;

namespace CapaDatos
{
    public class CD_Clientes
    {
        public Cliente ObtenerClientePorCorreo(string correo)
        {
            Cliente cliente = null;
            try
            {
                using (SqlConnection objConexion = new SqlConnection(Conexion.conex))
                {
                    string consulta = "SELECT IdCliente, Nombres, Correo, Reestablecer, FechaUltimoReinicio, PasswordHash, PasswordSalt FROM CLIENTE WHERE Correo = @correo";

                    SqlCommand comando = new SqlCommand(consulta, objConexion);
                    comando.Parameters.AddWithValue("@correo", correo);
                    comando.CommandType = CommandType.Text;

                    objConexion.Open();

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        if (lector.Read()) 
                        {
                            cliente = new Cliente
                            {
                                IdCliente = Convert.ToInt32(lector["IdCliente"]),
                                Nombres = lector["Nombres"].ToString(),
                                Correo = lector["Correo"].ToString(),
                                Reestablecer = Convert.ToBoolean(lector["Reestablecer"]),
                                PasswordHash = (byte[])lector["PasswordHash"],
                                PasswordSalt = (byte[])lector["PasswordSalt"],
                                FechaUltimoReinicio = lector["FechaUltimoReinicio"] == DBNull.Value
                                                    ? (DateTime?)null
                                                    : Convert.ToDateTime(lector["FechaUltimoReinicio"])
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return cliente;
        }

        public int Registrar(Cliente obj, out string Mensaje)
        {
            int idClienteGenerado = 0;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection objConexion = new SqlConnection(Conexion.conex))
                {

                    string consulta = "INSERT INTO CLIENTE(Nombres, Apellidos, Correo, PasswordHash, PasswordSalt, Reestablecer, EsMayorDeEdad) " +
                                      "VALUES(@Nombres, @Apellidos, @Correo, @PasswordHash, @PasswordSalt, @Reestablecer, @EsMayorDeEdad);" +
                                      "SELECT SCOPE_IDENTITY();";

                    SqlCommand comando = new SqlCommand(consulta, objConexion);
                    comando.Parameters.AddWithValue("@Nombres", obj.Nombres);
                    comando.Parameters.AddWithValue("@Apellidos", obj.Apellidos);
                    comando.Parameters.AddWithValue("@Correo", obj.Correo);
                    comando.Parameters.AddWithValue("@PasswordHash", obj.PasswordHash);
                    comando.Parameters.AddWithValue("@PasswordSalt", obj.PasswordSalt);
                    comando.Parameters.AddWithValue("@Reestablecer", obj.Reestablecer);
                    comando.Parameters.AddWithValue("@EsMayorDeEdad", obj.EsMayorDeEdad);

                    comando.CommandType = CommandType.Text;

                    objConexion.Open();
                    idClienteGenerado = Convert.ToInt32(comando.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                idClienteGenerado = 0;
                Mensaje = ex.Message;
            }
            return idClienteGenerado;
        }

        public Cliente ObtenerClientePorId(int idCliente)
        {
            Cliente cliente = null;
            try
            {
                using (SqlConnection objConexion = new SqlConnection(Conexion.conex))
                {
                    string consulta = "SELECT IdCliente, Nombres, Apellidos, Correo FROM CLIENTE WHERE IdCliente = @idCliente";

                    SqlCommand comando = new SqlCommand(consulta, objConexion);
                    comando.Parameters.AddWithValue("@idCliente", idCliente);
                    comando.CommandType = CommandType.Text;

                    objConexion.Open();

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        if (lector.Read())
                        {
                            cliente = new Cliente
                            {
                                IdCliente = Convert.ToInt32(lector["IdCliente"]),
                                Nombres = lector["Nombres"].ToString(),
                                Apellidos = lector["Apellidos"].ToString(),
                                Correo = lector["Correo"].ToString()
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                cliente = null;
            }
            return cliente;
        }

        public bool ActualizarPassword(int idCliente, byte[] passwordHash, byte[] passwordSalt, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection objConexion = new SqlConnection(Conexion.conex))
                {
                    string consulta = "UPDATE CLIENTE SET " +
                                      "PasswordHash = @PasswordHash, " +
                                      "PasswordSalt = @PasswordSalt, " +
                                      "Reestablecer = 0, " +
                                      "FechaUltimoReinicio = GETDATE() " +
                                      "WHERE IdCliente = @IdCliente";

                    SqlCommand comando = new SqlCommand(consulta, objConexion);
                    comando.Parameters.AddWithValue("@IdCliente", idCliente);
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