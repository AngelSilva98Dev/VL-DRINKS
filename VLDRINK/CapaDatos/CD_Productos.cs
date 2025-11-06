using System;
using System.Collections.Generic;
using CapaEntidad;
using System.Data.SqlClient;
using System.Data;

namespace CapaDatos
{
    public class CD_Productos
    {
        // LEER (Listar)
        public List<Producto> Listar()
        {
            List<Producto> lista = new List<Producto>();
            try
            {
                using (SqlConnection objConexion = new SqlConnection(Conexion.conex))
                {
                    string consulta = @"
                SELECT 
                    p.IdProducto, p.Nombre, p.Descripcion, p.Precio, p.Stock, p.Activo,
                    p.NombreImagen, -- <--- AÑADIDO
                    m.IdMarca, m.Descripcion AS MarcaDescripcion,
                    c.IdCategoria, c.Descripcion AS CategoriaDescripcion
                FROM 
                    PRODUCTO p
                INNER JOIN 
                    MARCA m ON p.IdMarca = m.IdMarca
                INNER JOIN 
                    CATEGORIA c ON p.IdCategoria = c.IdCategoria";

                    SqlCommand comando = new SqlCommand(consulta, objConexion);
                    comando.CommandType = CommandType.Text;
                    objConexion.Open();

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            lista.Add(new Producto()
                            {
                                IdProducto = Convert.ToInt32(lector["IdProducto"]),
                                Nombre = lector["Nombre"].ToString(),
                                Descripcion = lector["Descripcion"].ToString(),
                                Precio = Convert.ToDecimal(lector["Precio"]),
                                Stock = Convert.ToInt32(lector["Stock"]),
                                Activo = Convert.ToBoolean(lector["Activo"]),
                                NombreImagen = lector["NombreImagen"].ToString(), 
                                objMarca = new Marca()
                                {
                                    IdMarca = Convert.ToInt32(lector["IdMarca"]),
                                    Descripcion = lector["MarcaDescripcion"].ToString()
                                },
                                objCategoria = new Categoria()
                                {
                                    IdCategoria = Convert.ToInt32(lector["IdCategoria"]),
                                    Descripcion = lector["CategoriaDescripcion"].ToString()
                                }
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lista;
        }

        // CREAR (Registrar)
        public int Registrar(Producto obj, out string Mensaje)
        {
            int idGenerado = 0;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection objConexion = new SqlConnection(Conexion.conex))
                {
                    string consulta = "INSERT INTO PRODUCTO(Nombre, Descripcion, IdMarca, IdCategoria, Precio, Stock, Activo, Imagen, NombreImagen) " +
                                      "VALUES (@Nombre, @Descripcion, @IdMarca, @IdCategoria, @Precio, @Stock, @Activo, @Imagen, @NombreImagen);" +
                                      "SELECT SCOPE_IDENTITY();";

                    SqlCommand comando = new SqlCommand(consulta, objConexion);
                    comando.Parameters.AddWithValue("@Nombre", obj.Nombre);
                    comando.Parameters.AddWithValue("@Descripcion", obj.Descripcion);
                    comando.Parameters.AddWithValue("@IdMarca", obj.objMarca.IdMarca);
                    comando.Parameters.AddWithValue("@IdCategoria", obj.objCategoria.IdCategoria);
                    comando.Parameters.AddWithValue("@Precio", obj.Precio);
                    comando.Parameters.AddWithValue("@Stock", obj.Stock);
                    comando.Parameters.AddWithValue("@Activo", obj.Activo);
                    comando.Parameters.AddWithValue("@Imagen", obj.Imagen); 
                    comando.Parameters.AddWithValue("@NombreImagen", obj.NombreImagen);
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
        public bool Modificar(Producto obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection objConexion = new SqlConnection(Conexion.conex))
                {
                    // La consulta SQL SÍ espera los parámetros de imagen
                    string consulta = @"UPDATE PRODUCTO SET 
                                Nombre = @Nombre, 
                                Descripcion = @Descripcion, 
                                IdMarca = @IdMarca, 
                                IdCategoria = @IdCategoria, 
                                Precio = @Precio, 
                                Stock = @Stock, 
                                Activo = @Activo,
                                NombreImagen = CASE WHEN @Imagen = '' THEN NombreImagen ELSE @NombreImagen END,
                                Imagen = CASE WHEN @Imagen = '' THEN Imagen ELSE @Imagen END
                              WHERE IdProducto = @IdProducto";

                    SqlCommand comando = new SqlCommand(consulta, objConexion);

                    // --- PARÁMETROS ---
                    comando.Parameters.AddWithValue("@IdProducto", obj.IdProducto);
                    comando.Parameters.AddWithValue("@Nombre", obj.Nombre);
                    comando.Parameters.AddWithValue("@Descripcion", obj.Descripcion);
                    comando.Parameters.AddWithValue("@IdMarca", obj.objMarca.IdMarca);
                    comando.Parameters.AddWithValue("@IdCategoria", obj.objCategoria.IdCategoria);
                    comando.Parameters.AddWithValue("@Precio", obj.Precio);
                    comando.Parameters.AddWithValue("@Stock", obj.Stock);
                    comando.Parameters.AddWithValue("@Activo", obj.Activo);
                    comando.Parameters.AddWithValue("@Imagen", obj.Imagen ?? "");
                    comando.Parameters.AddWithValue("@NombreImagen", obj.NombreImagen ?? "");

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
                    string consulta = "DELETE FROM PRODUCTO WHERE IdProducto = @IdProducto";
                    SqlCommand comando = new SqlCommand(consulta, objConexion);
                    comando.Parameters.AddWithValue("@IdProducto", id);
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

        // Leer Producto para modificar(+ imagen)
        public Producto ObtenerProducto(int id)
        {
            Producto obj = null;
            try
            {
                using (SqlConnection objConexion = new SqlConnection(Conexion.conex))
                {

                    string consulta = @"
                SELECT 
                    p.IdProducto, p.Nombre, p.Descripcion, p.Precio, p.Stock, p.Activo,
                    p.NombreImagen, p.Imagen, -- <-- ¡AQUÍ ESTÁ LA IMAGEN!
                    m.IdMarca, m.Descripcion AS MarcaDescripcion,
                    c.IdCategoria, c.Descripcion AS CategoriaDescripcion
                FROM 
                    PRODUCTO p
                INNER JOIN 
                    MARCA m ON p.IdMarca = m.IdMarca
                INNER JOIN 
                    CATEGORIA c ON p.IdCategoria = c.IdCategoria
                WHERE 
                    p.IdProducto = @idProducto";

                    SqlCommand comando = new SqlCommand(consulta, objConexion);
                    comando.Parameters.AddWithValue("@idProducto", id);
                    comando.CommandType = CommandType.Text;
                    objConexion.Open();

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        if (lector.Read())
                        {
                            obj = new Producto()
                            {
                                IdProducto = Convert.ToInt32(lector["IdProducto"]),
                                Nombre = lector["Nombre"].ToString(),
                                Descripcion = lector["Descripcion"].ToString(),
                                Precio = Convert.ToDecimal(lector["Precio"]),
                                Stock = Convert.ToInt32(lector["Stock"]),
                                Activo = Convert.ToBoolean(lector["Activo"]),
                                NombreImagen = lector["NombreImagen"].ToString(),
                                Imagen = lector["Imagen"].ToString(), 
                                objMarca = new Marca()
                                {
                                    IdMarca = Convert.ToInt32(lector["IdMarca"]),
                                    Descripcion = lector["MarcaDescripcion"].ToString()
                                },
                                objCategoria = new Categoria()
                                {
                                    IdCategoria = Convert.ToInt32(lector["IdCategoria"]),
                                    Descripcion = lector["CategoriaDescripcion"].ToString()
                                }
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                obj = null;
            }
            return obj;
        }
    }
}