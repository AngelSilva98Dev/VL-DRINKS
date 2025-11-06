using System;
using System.Collections.Generic;
using CapaEntidad;
using System.Data.SqlClient;
using System.Data;

namespace CapaDatos
{
    public class CD_Marcas
    {
        // LEER (Listar)
        public List<Marca> Listar()
        {
            List<Marca> lista = new List<Marca>();
            try
            {
                using (SqlConnection objConexion = new SqlConnection(Conexion.conex))
                {
                    string consulta = "SELECT IdMarca, Descripcion, Activo FROM MARCA";
                    SqlCommand comando = new SqlCommand(consulta, objConexion);
                    comando.CommandType = CommandType.Text;
                    objConexion.Open();

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            lista.Add(new Marca()
                            {
                                IdMarca = Convert.ToInt32(lector["IdMarca"]),
                                Descripcion = lector["Descripcion"].ToString(),
                                Activo = Convert.ToBoolean(lector["Activo"])
                            });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Marca>();
            }
            return lista;
        }

        // CREAR (Registrar)
        public int Registrar(Marca obj, out string Mensaje)
        {
            int idGenerado = 0;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection objConexion = new SqlConnection(Conexion.conex))
                {
                    string consulta = "INSERT INTO MARCA(Descripcion, Activo) VALUES (@Descripcion, @Activo);" +
                                      "SELECT SCOPE_IDENTITY();";

                    SqlCommand comando = new SqlCommand(consulta, objConexion);
                    comando.Parameters.AddWithValue("@Descripcion", obj.Descripcion);
                    comando.Parameters.AddWithValue("@Activo", obj.Activo);
                    comando.CommandType = CommandType.Text;

                    objConexion.Open();
                    idGenerado = Convert.ToInt32(comando.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                idGenerado = 0;
                Mensaje = ex.Message;
            }
            return idGenerado;
        }

        // MODIFICAR
        public bool Modificar(Marca obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection objConexion = new SqlConnection(Conexion.conex))
                {
                    string consulta = "UPDATE MARCA SET Descripcion = @Descripcion, Activo = @Activo " +
                                      "WHERE IdMarca = @IdMarca";

                    SqlCommand comando = new SqlCommand(consulta, objConexion);
                    comando.Parameters.AddWithValue("@IdMarca", obj.IdMarca);
                    comando.Parameters.AddWithValue("@Descripcion", obj.Descripcion);
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

        // ELIMINAR
        public bool Eliminar(int id, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection objConexion = new SqlConnection(Conexion.conex))
                {
                    string consulta = "DELETE FROM MARCA WHERE IdMarca = @IdMarca";
                    SqlCommand comando = new SqlCommand(consulta, objConexion);
                    comando.Parameters.AddWithValue("@IdMarca", id);
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