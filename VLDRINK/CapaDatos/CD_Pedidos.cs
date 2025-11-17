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
    public class CD_Pedidos
    {
        public List<Pedido> Listar(string estado)
        {
            List<Pedido> lista = new List<Pedido>();
            try
            {
                using (SqlConnection objConexion = new SqlConnection(Conexion.conex))
                {

                    string consulta = "SELECT IdPedido, Contacto, MontoTotal, MetodoPago, Estado, FechaPedido FROM PEDIDO";

                    if (!string.IsNullOrEmpty(estado) && estado != "Todos")
                    {
                        consulta += " WHERE Estado = @Estado";
                    }

                    consulta += " ORDER BY FechaPedido DESC";

                    SqlCommand comando = new SqlCommand(consulta, objConexion);

                    if (!string.IsNullOrEmpty(estado) && estado != "Todos")
                    {
                        comando.Parameters.AddWithValue("@Estado", estado);
                    }

                    comando.CommandType = CommandType.Text;
                    objConexion.Open();

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            lista.Add(new Pedido()
                            {
                                IdPedido = Convert.ToInt32(lector["IdPedido"]),
                                Contacto = lector["Contacto"].ToString(),
                                MontoTotal = Convert.ToDecimal(lector["MontoTotal"]),
                                MetodoPago = lector["MetodoPago"].ToString(),
                                Estado = lector["Estado"].ToString(),
                                FechaPedido = Convert.ToDateTime(lector["FechaPedido"])
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lista = new List<Pedido>();
            }
            return lista;
        }

        public bool CambiarEstado(int idPedido, string nuevoEstado, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection objConexion = new SqlConnection(Conexion.conex))
                {
                    string consulta = "UPDATE PEDIDO SET Estado = @NuevoEstado WHERE IdPedido = @IdPedido";

                    SqlCommand comando = new SqlCommand(consulta, objConexion);
                    comando.Parameters.AddWithValue("@IdPedido", idPedido);
                    comando.Parameters.AddWithValue("@NuevoEstado", nuevoEstado);
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

        public int Registrar(Pedido objPedido, List<DetallePedido> listaDetallePedido, out string Mensaje)
        {
            int idPedidoGenerado = 0;
            Mensaje = string.Empty;

            using (SqlConnection objConexion = new SqlConnection(Conexion.conex))
            {
                string queryInsertPedido = @"
            INSERT INTO PEDIDO (IdCliente, TotalProducto, Subtotal, CostoEnvio, MontoTotal, Contacto, Telefono, Direccion, MetodoPago, Estado, FechaPedido) 
            VALUES 
            (@IdCliente, @TotalProducto, @Subtotal, @CostoEnvio, @MontoTotal, @Contacto, @Telefono, @Direccion, @MetodoPago, @Estado, GETDATE());
            SELECT SCOPE_IDENTITY();";

                string queryInsertDetalle = @"INSERT INTO DETALLE_PEDIDO(IdPedido, IdProducto, NombreProducto, Cantidad, PrecioUnitario, Total)
            VALUES
            (@IdPedido, @IdProducto, @NombreProducto, @Cantidad, @PrecioUnitario, @Total);";

                SqlTransaction transaction = null;

                try
                {
                    objConexion.Open();
                    transaction = objConexion.BeginTransaction();

                    SqlCommand cmdPedido = new SqlCommand(queryInsertPedido, objConexion, transaction);

                    cmdPedido.Parameters.AddWithValue("@IdCliente", objPedido.objCliente.IdCliente);
                    cmdPedido.Parameters.AddWithValue("@TotalProducto", objPedido.TotalProducto);
                    cmdPedido.Parameters.AddWithValue("@Subtotal", objPedido.Subtotal);
                    cmdPedido.Parameters.AddWithValue("@CostoEnvio", objPedido.CostoEnvio);
                    cmdPedido.Parameters.AddWithValue("@MontoTotal", objPedido.MontoTotal);
                    cmdPedido.Parameters.AddWithValue("@Contacto", objPedido.Contacto);
                    cmdPedido.Parameters.AddWithValue("@Telefono", objPedido.Telefono);
                    cmdPedido.Parameters.AddWithValue("@Direccion", objPedido.Direccion);
                    cmdPedido.Parameters.AddWithValue("@MetodoPago", objPedido.MetodoPago);
                    cmdPedido.Parameters.AddWithValue("@Estado", objPedido.Estado);

                    idPedidoGenerado = Convert.ToInt32(cmdPedido.ExecuteScalar());

                    foreach (DetallePedido detalle in listaDetallePedido)
                    {
                        SqlCommand cmdDetalle = new SqlCommand(queryInsertDetalle, objConexion, transaction);

                        cmdDetalle.Parameters.AddWithValue("@IdPedido", idPedidoGenerado);
                        cmdDetalle.Parameters.AddWithValue("@IdProducto", detalle.objProducto.IdProducto);
                        cmdDetalle.Parameters.AddWithValue("@NombreProducto", detalle.NombreProducto);
                        cmdDetalle.Parameters.AddWithValue("@Cantidad", detalle.Cantidad);
                        cmdDetalle.Parameters.AddWithValue("@PrecioUnitario", detalle.PrecioUnitario);
                        cmdDetalle.Parameters.AddWithValue("@Total", detalle.Total);

                        cmdDetalle.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    if (transaction != null)
                        transaction.Rollback();

                    idPedidoGenerado = 0;
                    Mensaje = "Error al registrar el pedido: " + ex.Message;
                }
                finally
                {
                    if (objConexion.State == ConnectionState.Open)
                        objConexion.Close();
                }
            }

            return idPedidoGenerado;
        }
    }
}
