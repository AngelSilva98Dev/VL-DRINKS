using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using System.Data.SqlClient;
using System.Data;
using System.Runtime.InteropServices;

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

        public int Registrar(Usuario objeto, out string Mensaje)
        {
            int idGenerado = 0;
            Mensaje = string.Empty;
            try
            {
                using(SqlConnection objConexion = new SqlConnection(Conexion.conex))
                {
                    SqlCommand comando = new SqlCommand("sp_RegistrarUsuario", objConexion);
                    comando.Parameters.AddWithValue("Nombres", objeto.Nombres);
                    comando.Parameters.AddWithValue("Apellidos", objeto.Apellidos);
                    comando.Parameters.AddWithValue("Correo", objeto.Correo);
                    comando.Parameters.AddWithValue("Clave", objeto.Clave);
                    comando.Parameters.AddWithValue("Activo", objeto.Activo);
                    comando.Parameters.Add("Resultado",SqlDbType.Int).Direction= ParameterDirection.Output;
                    comando.Parameters.Add("Mensaje", SqlDbType.VarChar,500).Direction = ParameterDirection.Output;
                    comando.CommandType = CommandType.StoredProcedure;

                    objConexion.Open();
                    comando.ExecuteNonQuery();
                    idGenerado = Convert.ToInt32(comando.Parameters["Resultado"].Value);
                    Mensaje = comando.Parameters["Mensaje"].Value.ToString();

                }
            }
            catch(Exception ex)
            {
                idGenerado = 0;
                Mensaje = ex.Message;
            }

            return idGenerado;
        }

        public bool Editar(Usuario objeto, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection objConexion = new SqlConnection(Conexion.conex))
                {
                    SqlCommand comando = new SqlCommand("sp_EditarUsuario", objConexion);
                    comando.Parameters.AddWithValue("IdUsuario", objeto.IdUsuario);
                    comando.Parameters.AddWithValue("Nombres", objeto.Nombres);
                    comando.Parameters.AddWithValue("Apellidos", objeto.Apellidos);
                    comando.Parameters.AddWithValue("Correo", objeto.Correo);
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

        public bool Eliminar(int id, out  string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection objConexion = new SqlConnection(Conexion.conex))
                {
                    SqlCommand comando = new SqlCommand("delete top(1) from USUARIO where IdUsuario = @id", objConexion);
                    comando.Parameters.AddWithValue("@id", id);
                    comando.CommandType = CommandType.Text;
                    objConexion.Open();
                    resultado = comando.ExecuteNonQuery() > 0 ? true : false;
                }

            }catch (Exception ex)
            {
                resultado =false;
                Mensaje = ex.Message;
            }

            return resultado;
        }

        public bool CambiarClave (int idusuario, string nuevaclave, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;

            try
            {
                using (SqlConnection objconexion =new  SqlConnection(Conexion.conex))
                {
                    SqlCommand comando = new SqlCommand("update USUARIO set Clave = @nuevaclave, Reestablecer= 0 where IdUsuario = @id", objconexion);
                    comando.Parameters.AddWithValue("@id", idusuario);
                    comando.Parameters.AddWithValue("@nuevaclave", nuevaclave);
                    comando.CommandType = CommandType.Text;
                    objconexion.Open();
                    resultado = comando.ExecuteNonQuery() > 0 ? true : false;

                }


            }
            catch(Exception ex) 
            {
                resultado = false;
                mensaje = ex.Message;

            }

            return resultado;
        }

        public bool ReestablecerClave(int idusuario, string clave, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;

            try
            {
                using (SqlConnection objconexion = new SqlConnection(Conexion.conex))
                {
                    SqlCommand comando = new SqlCommand("update USUARIO set Clave = @clave, Reestablecer= 1 where IdUsuario = @id", objconexion);
                    comando.Parameters.AddWithValue("@id", idusuario);
                    comando.Parameters.AddWithValue("@clave", clave);
                    comando.CommandType = CommandType.Text;
                    objconexion.Open();
                    resultado = comando.ExecuteNonQuery() > 0 ? true : false;

                }


            }
            catch (Exception ex)
            {
                resultado = false;
                mensaje = ex.Message;

            }

            return resultado;
        }

    }
}
