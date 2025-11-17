using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_Categorias
    {
        public List<Categoria> Listar()
        {
            List<Categoria> lista = new List<Categoria>();

            try
            {
                using (SqlConnection objConexion = new SqlConnection(Conexion.conex))
                {
                    string consulta = "select IdCategoria, Descripcion, Activo from CATEGORIA";

                    SqlCommand comando = new SqlCommand(consulta, objConexion);
                    comando.CommandType = CommandType.Text;

                    objConexion.Open();

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            lista.Add(new Categoria
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

        public int Registrar(Categoria objeto, out string Mensaje)
        {
            int idGenerado = 0;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection objConexion = new SqlConnection(Conexion.conex))
                {
                    SqlCommand comando = new SqlCommand("sp_RegistrarCategoria", objConexion);
                    comando.Parameters.AddWithValue("Descripcion", objeto.Descripcion);
                    comando.Parameters.AddWithValue("Activo", objeto.Activo);                  
                    comando.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    comando.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    comando.CommandType = CommandType.StoredProcedure;

                    objConexion.Open();
                    comando.ExecuteNonQuery();
                    idGenerado = Convert.ToInt32(comando.Parameters["Resultado"].Value);
                    Mensaje = comando.Parameters["Mensaje"].Value.ToString();

                }
            }
            catch (Exception ex)
            {
                idGenerado = 0;
                Mensaje = ex.Message;
            }

            return idGenerado;
        }

        public bool Editar(Categoria objeto, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection objConexion = new SqlConnection(Conexion.conex))
                {
                    SqlCommand comando = new SqlCommand("sp_EditarCategoria", objConexion);
                    comando.Parameters.AddWithValue("IdCategoria", objeto.IdCategoria);
                    comando.Parameters.AddWithValue("Descripcion", objeto.Descripcion);           
                    comando.Parameters.AddWithValue("Activo", objeto.Activo);
                    comando.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    comando.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    comando.CommandType = CommandType.StoredProcedure;

                    objConexion.Open();
                    comando.ExecuteNonQuery();
                    resultado = Convert.ToBoolean(comando.Parameters["Resultado"].Value);
                    Mensaje = comando.Parameters["Mensaje"].Value.ToString();

                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
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
                    SqlCommand comando = new SqlCommand("sp_EliminarCategoria", objConexion);
                    comando.Parameters.AddWithValue("IdCategoria", id);                    
                    comando.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    comando.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    comando.CommandType = CommandType.StoredProcedure;

                    objConexion.Open();
                    comando.ExecuteNonQuery();
                    resultado = Convert.ToBoolean(comando.Parameters["Resultado"].Value);
                    Mensaje = comando.Parameters["Mensaje"].Value.ToString();

                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }

            return resultado;
        }
    }
}
