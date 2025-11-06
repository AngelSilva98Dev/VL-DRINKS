using System;
using System.Collections.Generic;
using CapaEntidad;
using System.Data.SqlClient;
using System.Data;

namespace CapaDatos
{
    public class CD_Categorias
    {
        // LEER (Listar)
        public List<Categoria> Listar()
        {
            List<Categoria> lista = new List<Categoria>();
            try
            {
                using (SqlConnection objConexion = new SqlConnection(Conexion.conex))
                {
                    string consulta = "SELECT IdCategoria, Descripcion, Activo FROM CATEGORIA";
                    SqlCommand comando = new SqlCommand(consulta, objConexion);
                    comando.CommandType = CommandType.Text;
                    objConexion.Open();

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            lista.Add(new Categoria()
                            {
                                IdCategoria = Convert.ToInt32(lector["IdCategoria"]),
                                Descripcion = lector["Descripcion"].ToString(),
                                Activo = Convert.ToBoolean(lector["Activo"])
                            });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Categoria>();
            }
            return lista;
        }

        // CREAR (Registrar)
        public int Registrar(Categoria obj, out string Mensaje)
        {
            int idGenerado = 0;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection objConexion = new SqlConnection(Conexion.conex))
                {
                    string consulta = "INSERT INTO CATEGORIA(Descripcion, Activo) VALUES (@Descripcion, @Activo);" +
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
        public bool Modificar(Categoria obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection objConexion = new SqlConnection(Conexion.conex))
                {
                    string consulta = "UPDATE CATEGORIA SET Descripcion = @Descripcion, Activo = @Activo " +
                                      "WHERE IdCategoria = @IdCategoria";

                    SqlCommand comando = new SqlCommand(consulta, objConexion);
                    comando.Parameters.AddWithValue("@IdCategoria", obj.IdCategoria);
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
                    string consulta = "DELETE FROM CATEGORIA WHERE IdCategoria = @IdCategoria";
                    SqlCommand comando = new SqlCommand(consulta, objConexion);
                    comando.Parameters.AddWithValue("@IdCategoria", id);
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