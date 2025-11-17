using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CapaDatos
{
    public  class CD_Productos
    {
        public List<Producto> Listar()
        {
            List<Producto> lista = new List<Producto>();

            try
            {
                using (SqlConnection objConexion = new SqlConnection(Conexion.conex))
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.AppendLine(" select p.IdProducto, p.Nombre, p.Descripcion,");
                    stringBuilder.AppendLine("m.IdMarca,m.Descripcion[DescMarca],");
                    stringBuilder.AppendLine("c.IdCategoria,c.Descripcion[DesCategoria],");
                    stringBuilder.AppendLine("p.Precio, p.Stock, p.Imagen, p.NombreImagen, p.Activo");
                    stringBuilder.AppendLine("from PRODUCTO p");
                    stringBuilder.AppendLine("inner join MARCA m on m.IdMarca = p.IdMarca");
                    stringBuilder.AppendLine("inner join CATEGORIA c on c.IdCategoria = p.IdCategoria");
                   
                    SqlCommand comando = new SqlCommand(stringBuilder.ToString(), objConexion);
                    comando.CommandType = CommandType.Text;

                    objConexion.Open();

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            lista.Add(new Producto
                            {
                                IdProducto = Convert.ToInt32(lector["IdProducto"]),
                                Nombre = lector["Nombre"].ToString(),
                                Descripcion = lector["Descripcion"].ToString(),                             
                                objMarca = new Marca() { IdMarca = Convert.ToInt32(lector["IdMarca"]), Descripcion= lector["DescMarca"].ToString(), },
                                objCategoria = new Categoria() { IdCategoria = Convert.ToInt32(lector["IdCategoria"]), Descripcion = lector["DesCategoria"].ToString() },
                                Precio = Convert.ToDecimal(lector["Precio"], new CultureInfo("es-AR")),
                                Stock = Convert.ToInt32(lector["Stock"]),
                                Imagen = lector["Imagen"].ToString(),
                                NombreImagen = lector["NombreImagen"].ToString(),
                                Activo = Convert.ToBoolean(lector["Activo"])

                            });
                        }
                    }

                }
            }
            catch
            {
                lista = new List<Producto>();
            }

            return lista;
        }

        public int Registrar(Producto objeto, out string Mensaje)
        {
            int idGenerado = 0;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection objConexion = new SqlConnection(Conexion.conex))
                {
                    SqlCommand comando = new SqlCommand("sp_RegistrarProducto", objConexion);
                    comando.Parameters.AddWithValue("Nombre", objeto.Nombre);
                    comando.Parameters.AddWithValue("Descripcion", objeto.Descripcion);
                    comando.Parameters.AddWithValue("IdMarca", objeto.objMarca.IdMarca);
                    comando.Parameters.AddWithValue("IdCategoria", objeto.objCategoria.IdCategoria);
                    comando.Parameters.AddWithValue("Precio", objeto.Precio);
                    comando.Parameters.AddWithValue("Stock", objeto.Stock);
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


        public bool Editar(Producto objeto, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection objConexion = new SqlConnection(Conexion.conex))
                {
                    SqlCommand comando = new SqlCommand("sp_EditarProducto", objConexion);
                    comando.Parameters.AddWithValue("IdProducto", objeto.IdProducto);
                    comando.Parameters.AddWithValue("Nombre", objeto.Nombre);
                    comando.Parameters.AddWithValue("Descripcion", objeto.Descripcion);
                    comando.Parameters.AddWithValue("IdMarca", objeto.objMarca.IdMarca);
                    comando.Parameters.AddWithValue("IdCategoria", objeto.objCategoria.IdCategoria);
                    comando.Parameters.AddWithValue("Precio", objeto.Precio);
                    comando.Parameters.AddWithValue("Stock", objeto.Stock);
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


        public bool GuardarInfoImagen(Producto objProducto, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection objConexion = new SqlConnection(Conexion.conex))
                {
                    string query = "update PRODUCTO set Imagen = @imagen, NombreImagen =@nombreimagen where IdProducto =@idproducto";

                    SqlCommand comando = new SqlCommand(query, objConexion);                
                    comando.Parameters.AddWithValue("@imagen", objProducto.Imagen);
                    comando.Parameters.AddWithValue("@nombreimagen", objProducto.NombreImagen);
                    comando.Parameters.AddWithValue("@idproducto", objProducto.IdProducto);
                    comando.CommandType = CommandType.Text;

                    objConexion.Open();
                    if(comando.ExecuteNonQuery() > 0)
                    {
                        resultado = true;
                    }
                    else
                    {
                        Mensaje = "No se pudo actualizar imagen";
                    }
                  

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
                    SqlCommand comando = new SqlCommand("sp_EliminarProducto", objConexion);
                    comando.Parameters.AddWithValue("IdProducto", id);
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
