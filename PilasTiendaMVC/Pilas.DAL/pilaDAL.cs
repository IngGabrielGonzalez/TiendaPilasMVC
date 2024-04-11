using PilasTiendaMVC.Pilas.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Configuration;

namespace PilasTiendaMVC.Pilas.DAL
{
    public class pilaDAL
    {
        string dbconexion;

        public pilaDAL()
        {
            //se define la cadena de conexion llamada ConectaProductos y se recuperan los datos del WebConfig
            dbconexion = ConfigurationManager.ConnectionStrings["ConectaProductos"].ConnectionString;
        }

        //---------------------- APARTADO DE USUARIO -----------------------------//

        public async Task<List<Usuarios>> ObtenerUsuarios()
        {
            List<Usuarios> ListaP = new List<Usuarios>();
            using (SqlConnection con = new SqlConnection(dbconexion))//Conexion que vamos a usar
            {
                SqlCommand cmd = new SqlCommand("ObtenerUsuarios", con);//Se llama al procedure
                cmd.CommandType = CommandType.StoredProcedure;// se elige comando de tipo Procedure

                try
                {
                    await con.OpenAsync();
                    SqlDataReader sdr = await cmd.ExecuteReaderAsync();
                    if (sdr.HasRows)
                    {//Mientras sdr pueda leer filas
                        while (sdr.Read())
                        {//Se agregan los productos obtenidos a la lista
                            ListaP.Add(new Usuarios
                            {
                                idUsuario = Convert.ToInt16(sdr["idUsuario"]),
                                nomUser = sdr["nomUser"].ToString(),
                                email = sdr["email"].ToString(),
                                passwd = sdr["passwd"].ToString(),
                                rol = sdr["rol"].ToString(),
                            });
                        }
                        con.Close(); //Cierre de conexion
                    }
                    else
                    { //Si no se obtuvo nada se retorna la lista vacía
                        ListaP = null;
                    }
                }
                catch (Exception)
                {
                    con.Close();
                }
                return ListaP; //Se retorna la lista con o sin valores
            }
        }

        public async Task<Usuarios> AutenticarUsuario(string nomUser, string passwd)
        {
            Usuarios usuarioAutenticado = null;

            using (SqlConnection connection = new SqlConnection(dbconexion))
            {
                using (SqlCommand command = new SqlCommand("Logueo", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@NomUser", nomUser);
                    command.Parameters.AddWithValue("@Passwd", passwd);
                    try
                    {
                        await connection.OpenAsync();
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (reader.HasRows) // Verificar si hay filas disponibles para leer
                            {
                                if (await reader.ReadAsync())
                                {
                                    usuarioAutenticado = new Usuarios
                                    {
                                        idUsuario = Convert.ToInt16(reader["idUsuario"]),
                                        nomUser = reader["nomUser"].ToString(),
                                        rol = reader["rol"].ToString()
                                    };

                                }
                                connection.Close();
                            }
                            else
                            {
                                usuarioAutenticado = null;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        connection.Close();
                    }
                    return usuarioAutenticado;
                }
            }
        }


        //---------------------- APARTADO DE VENTAS -----------------------------//
        public async Task<List<Ventas>> ObtenerVentas() // SELECT * FROM VENTAS
        {
            List<Ventas> ListaP = new List<Ventas>();
            using (SqlConnection con = new SqlConnection(dbconexion))//Conexion que vamos a usar
            {
                SqlCommand cmd = new SqlCommand("ObtenerVentas", con);//Se llama al procedure
                cmd.CommandType = CommandType.StoredProcedure;// se elige comando de tipo Procedure

                try
                {            
                    await con.OpenAsync();
                    SqlDataReader sdr = await cmd.ExecuteReaderAsync();
                    if (sdr.HasRows)
                    {//Mientras sdr pueda leer filas
                        while (sdr.Read())
                        {//Se agregan los productos obtenidos a la lista
                            ListaP.Add(new Ventas
                            {
                                idVenta = Convert.ToInt16(sdr["idVenta"]),
                                Nombre = Convert.ToString(sdr["Nombre"]),
                                Cantidad = Convert.ToInt16(sdr["Cantidad"]),
                                Precio = Convert.ToInt16(sdr["Precio"]),
                                Total = Convert.ToDouble(sdr["Total"]),
                                Fecha = Convert.ToDateTime(sdr["Fecha"]),
                                UsuarioId = Convert.ToInt16(sdr["UsuarioId"]),
                            });
                        }
                        con.Close(); //Cierre de conexion
                    }
                    else
                    { //Si no se obtuvo nada se retorna la lista vacía
                        ListaP = null;
                    }
                }
                catch (Exception)
                {
                    con.Close();
                }
                return ListaP; //Se retorna la lista con o sin valores

            }
        }

        public async Task AgregarVenta(Ventas venta)
        {
            using (SqlConnection con = new SqlConnection(dbconexion))
            {
                SqlCommand cmd = new SqlCommand("AgregarVenta", con);
                cmd.CommandType = CommandType.StoredProcedure;

                // Se agregan los parámetros a enviar al procedimiento almacenado
                cmd.Parameters.AddWithValue("@Nombre", venta.Nombre);
                cmd.Parameters.AddWithValue("@Cantidad", venta.Cantidad);
                cmd.Parameters.AddWithValue("@Precio", venta.Precio);
                cmd.Parameters.AddWithValue("@Total", venta.Total);
                cmd.Parameters.AddWithValue("@Fecha", venta.Fecha);
                cmd.Parameters.AddWithValue("@UsuarioId", venta.UsuarioId);

                try
                {
                    await con.OpenAsync(); // Se abre la conexión
                    await cmd.ExecuteNonQueryAsync(); // Se ejecuta el procedimiento almacenado
                    con.Close();
                }
                catch (Exception)
                {
                    con.Close();
                    // Manejo de excepciones si es necesario
                }
            }
        }


        public async Task<List<Ventas>> ObtenerVentas(int usuarioId)
        {
            List<Ventas> ventasUsuario = new List<Ventas>();

            using (SqlConnection con = new SqlConnection(dbconexion))
            {
                SqlCommand cmd = new SqlCommand("ObtenerUsuariosVentas", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UsuarioId", usuarioId);

                try
                {
                    await con.OpenAsync();
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            Ventas venta = new Ventas
                            {
                                idProducto = Convert.ToInt16(reader["idProducto"]),
                                Cantidad = Convert.ToInt32(reader["Cantidad"]),
                                Nombre = Convert.ToString(reader["Nombre"]),
                                Fecha = Convert.ToDateTime(reader["Fecha"])
                            };

                            ventasUsuario.Add(venta);
                        }
                    }

                    return ventasUsuario;
                }
                catch (Exception ex)
                {
                    // Manejar excepciones aquí
                    throw new Exception("Error al obtener ventas por ID de usuario", ex);
                }
            }
        }
        //Agregar venta
        

        public async Task<List<UsuarioCompra>> ObtenerUsuariosVentas(int idProd)
        {
            List<UsuarioCompra> comprasUsr = new List<UsuarioCompra>();

            using (SqlConnection con = new SqlConnection(dbconexion))
            {
                SqlCommand cmd = new SqlCommand("ObtenerUsuariosConCompras", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProductoId", idProd);
                try
                {
                    await con.OpenAsync();
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        UsuarioCompra compraUsuario = new UsuarioCompra
                        {
                            nomUser = reader["nomUser"].ToString(),
                            Cantidad = Convert.ToInt32(reader["Cantidad"]),
                            Fecha = Convert.ToDateTime(reader["Fecha"])
                        };

                        comprasUsr.Add(compraUsuario);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener usuarios con compras", ex);
                }
            }

            return comprasUsr;
        }










        //------------------ APARTADO DE PILAS(PRODUCTOS) ADMIN-USER -------------------//

        public async Task<bool> EliminarProd(int id)
        {
            using (SqlConnection con = new SqlConnection(dbconexion))
            {
                SqlCommand cmd = new SqlCommand("EliminarProd", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PilaId", id);

                try
                {
                    await con.OpenAsync();
                    int filas = cmd.ExecuteNonQuery();
                    con.Close();

                    if (filas == -1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception)
                {
                    return false;
                }

            }
        }

        public async Task<Pila> ObtenerProd(int id)
        {
            using (SqlConnection con = new SqlConnection(dbconexion))
            {
                SqlCommand cmd = new SqlCommand("ObtenerProd", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PilaId", id);

                try
                {
                    await con.OpenAsync();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.Read())
                        {
                            //Crear instancia de pila y le vamos asignando los valores desde el reader
                            Pila pila = new Pila();

                            pila.PilaId = (int)reader["PilaId"];
                            pila.Nombre = (string)reader["Nombre"];
                            pila.Descripcion = (string)reader["Descripcion"];
                            pila.Fecha = (DateTime)reader["Fecha"];
                            pila.Imagen = (string)reader["Imagen"];
                            pila.Stock = (int)reader["Stock"];

                            return pila;
                        }
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            }
            return null;
        }

        public async Task EditarProd(Pila pila)
        {
            using (SqlConnection con = new SqlConnection(dbconexion))
            {
                SqlCommand cmd = new SqlCommand("ActualizarProducto", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PilaId", pila.PilaId);
                cmd.Parameters.AddWithValue("@Nombre", pila.Nombre);
                cmd.Parameters.AddWithValue("@Descripcion", pila.Descripcion);
                cmd.Parameters.AddWithValue("@Fecha", pila.Fecha);
                cmd.Parameters.AddWithValue("@Imagen", pila.Imagen);
                cmd.Parameters.AddWithValue("@Stock", pila.Stock);

                try
                {
                    await con.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }
                catch (Exception ex)
                {
                    // Manejar excepciones aquí
                    throw new Exception("Error al editar el producto", ex);
                }
            }
        }




        public async Task<List<Pila>> ObtenerProductos()
        {
            List<Pila> ListaP = new List<Pila>();
            using (SqlConnection con = new SqlConnection(dbconexion))//Conexion que vamos a usar
            {
                SqlCommand cmd = new SqlCommand("ObtenerProductos", con);//Se llama al procedure
                cmd.CommandType = CommandType.StoredProcedure;// se elige comando de tipo Procedure

                try
                {
                    await con.OpenAsync();
                    SqlDataReader sdr = await cmd.ExecuteReaderAsync();
                    if (sdr.HasRows)
                    {//Mientras sdr pueda leer filas
                        while (sdr.Read())
                        {//Se agregan los productos obtenidos a la lista
                            ListaP.Add(new Pila
                            {
                                PilaId = Convert.ToInt16(sdr["PilaId"]),
                                Nombre = sdr["Nombre"].ToString(),
                                Descripcion = sdr["Descripcion"].ToString(),
                                Fecha = Convert.ToDateTime(sdr["Fecha"]),
                                Precio = Convert.ToDouble(sdr["Precio"]),
                                Imagen = sdr["Imagen"].ToString(),
                                Stock = Convert.ToInt16(sdr["Stock"]),

                            });
                        }
                        con.Close(); //Cierre de conexion
                    }
                    else
                    { //Si no se obtuvo nada se retorna la lista vacía
                        ListaP = null;
                    }
                }
                catch (Exception)
                {
                    con.Close();
                }
                return ListaP; //Se retorna la lista con o sin valores

            }
        }


        public async Task<List<Pila>> ObtenerProductosPorNombre(string nombre)
        {
            List<Pila> listaPilas = new List<Pila>();

            using (SqlConnection con = new SqlConnection(dbconexion))
            {
                SqlCommand cmd = new SqlCommand("ObtenerProductoPorNombre", con);//Se llama al procedure
                cmd.CommandType = CommandType.StoredProcedure;// se elige comando de tipo Procedure
                cmd.Parameters.AddWithValue("@NombreProducto", nombre);//se envia el parametro nombre
                try
                {
                    await con.OpenAsync();
                    SqlDataReader sdr = await cmd.ExecuteReaderAsync();
                    if (sdr.HasRows)
                    {//Mientras sdr pueda leer filas
                        while (sdr.Read())
                        {//Se agregan los productos obtenidos a la lista
                            listaPilas.Add(new Pila
                            {
                                Nombre = sdr["Nombre"].ToString(),
                                Descripcion = sdr["Descripcion"].ToString(),
                                Fecha = Convert.ToDateTime(sdr["Fecha"]),
                                Precio = Convert.ToDouble(sdr["Precio"]),
                                Imagen = sdr["Imagen"].ToString(),
                            });
                        }
                        con.Close(); //Cierre de conexion
                    }
                    else
                    { //Si no se obtuvo nada se retorna la lista vacía
                        listaPilas = null;
                    }
                }
                catch (Exception)
                {
                    con.Close();
                }
                return listaPilas; //Se retorna la lista con o sin valores

            }
        }

        public async Task AgregarPila(Pila pila)
        {
            using (SqlConnection con = new SqlConnection(dbconexion))
            {
                SqlCommand cmd = new SqlCommand("AgregarPila", con);
                cmd.CommandType = CommandType.StoredProcedure;

                //Se agregan los parametros a enviar al PROCEDURE
                cmd.Parameters.AddWithValue("@Nombre", pila.Nombre);
                cmd.Parameters.AddWithValue("@Descripcion", pila.Descripcion);
                cmd.Parameters.AddWithValue("@Fecha", pila.Fecha);
                cmd.Parameters.AddWithValue("@Precio", pila.Precio);
                cmd.Parameters.AddWithValue("@Imagen", pila.Imagen);
                cmd.Parameters.AddWithValue("@stock", pila.Stock);

                try
                {
                    await con.OpenAsync();//Se abre la conexion
                    await cmd.ExecuteNonQueryAsync();//Se ejecuta la query
                    con.Close();
                }
                catch (Exception)
                {
                    con.Close();
                }
            }
        }


        //METODO DE COMPRA
        public async Task ProcesarListaDePilas(List<Pila> listaDePilas, int usuarioId, int cantidadDeseada)
        {

            using (SqlConnection connection = new SqlConnection(dbconexion))
            {
                await connection.OpenAsync();
                foreach (Pila pila in listaDePilas)
                {
                    await ComprarPila(connection, pila.PilaId, cantidadDeseada, usuarioId); // Ajusta cantidadDeseada según tu necesidad
                }
                connection.Close();
            }
        }

        public async Task ComprarPila(SqlConnection connection, int pilaId, int cantidad, int usuarioId)
        {
            using (SqlCommand cmd = new SqlCommand("ComprarPila", connection))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idProducto", pilaId);
                cmd.Parameters.AddWithValue("@Cantidad", cantidad);
                cmd.Parameters.AddWithValue("@UsuarioId", usuarioId);
                await cmd.ExecuteNonQueryAsync();
            }
        }



    }
}