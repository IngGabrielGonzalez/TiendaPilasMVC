using PilasTiendaMVC.Pilas.DAL;
using PilasTiendaMVC.Pilas.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PilasTiendaMVC.Pilas.BLL
{
    public class Negocios
    {
        pilaDAL pila;

        // Constructor
        public Negocios()
        {
            pila = new pilaDAL();
        }

        //---------------------- APARTADO DE USUARIO -----------------------------//

        public async Task<List<Usuarios>> ObtenerUsuarios()
        {
            return await pila.ObtenerUsuarios();
        }

        public async Task<Usuarios> AutenticarUsuario(string nomUser, string passwd)
        {
            return await pila.AutenticarUsuario(nomUser, passwd);
        }

        //---------------------- APARTADO DE VENTAS -----------------------------//

        // SELECT * FROM Ventas
        public async Task<List<Ventas>> ObtenerVentas()
        {
            return await pila.ObtenerVentas();
        }

        public async Task<List<Ventas>> ObtenerUsuariosVentas(int usuarioId)
        {
            return await pila.ObtenerVentas(usuarioId);
        }

        public async Task<List<UsuarioCompra>> ObtenerUsuariosCompras(int idProducto)
        {
            return await pila.ObtenerUsuariosVentas(idProducto);
        }

        //---------------------- APARTADO DE CARRITO(PRODUCTOS) ----------------------//
        //public AgregarAlCarrito()
        //{

          //  return pila.AgregarCarrito();
        //}


        //---------------------- APARTADO DE PILAS(PRODUCTOS) ----------------------//

        public async Task<bool> EliminarProd(int id)
        {
            return await pila.EliminarProd(id);
        }

        public async Task<Pila> EditarProducto(int id)
        {
            return await pila.ObtenerProd(id);
        }


        public async Task EditarProd(Pila pilas)
        {
            await pila.EditarProd(pilas);
        }



        public async Task<List<Pila>> ObtenerProductos()
        {
            return await pila.ObtenerProductos();
        }

        public async Task<List<Pila>> ObtenerProductosPorNombre(string nombre)
        {
            return await pila.ObtenerProductosPorNombre(nombre);
        }

        public async Task AgregarPila(Pila pilas)
        {
            await pila.AgregarPila(pilas);
        }

        // METODO DE COMPRA LLAMADO DESDE LA DAL
        public async Task Comprar(List<Pila> lista, int cantidad, int idUsuario)
        {
            await pila.ProcesarListaDePilas(lista, cantidad, idUsuario);
        }
    }
}
