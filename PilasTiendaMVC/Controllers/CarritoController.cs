using PilasTiendaMVC.Pilas.BLL;
using PilasTiendaMVC.Pilas.Entities;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System;
using PilasTiendaMVC.Pilas.DAL;
using TiendaPilasMVC.Controllers;

namespace PilasTiendaMVC.Controllers
{
    public class CarritoController : CacheController
    {
        Negocios ng = new Negocios();

        // GET: Carrito
        public ActionResult Index()
        {
            if (Usuario_AUTHCompra())
            {
                No_cache(); // Deshabilitar la caché

                // Obtener el carrito de la sesión
                List<Pila> carrito = Session["Carrito"] as List<Pila>;
                return View(carrito ?? new List<Pila>());
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }

        }

        [HttpPost]
        public ActionResult AgregarAlCarrito(int id, double precio, string nombre, int cantidad, int stock)
        {

            if(stock >= cantidad)
            {
                // Obtener el carrito de la sesión
                List<Pila> carrito = Session["Carrito"] as List<Pila>;
                if (carrito == null)
                {
                    // Si el carrito aún no existe en la sesión, crear uno nuevo
                    carrito = new List<Pila>();
                    Session["Carrito"] = carrito;
                }

                // Buscar si el producto ya está en el carrito
                Pila productoExistente = carrito.FirstOrDefault(p => p.Nombre == nombre);

                if (productoExistente != null)
                {
                    // Si el producto ya está en el carrito, sumar la cantidad
                    productoExistente.Stock += cantidad;
                }
                else
                {
                    // Si el producto no está en el carrito, agregarlo
                    // Se añade el producto obtenido de la base de datos
                    carrito.Add(new Pila { PilaId = id, Nombre = nombre, Precio = precio, Stock = cantidad });

                }

                return RedirectToAction("Index");
            }
            else
            {
                return View("Stock");
            }
            
        }

        //[HttpPost]
        
        public ActionResult RemoverDelCarrito(int id)
        {
            // Obtener el carrito de la sesión
            List<Pila> carrito = Session["Carrito"] as List<Pila>;

            if (carrito != null)
            {
                // Buscar el producto en el carrito
                Pila productoExistente = carrito.FirstOrDefault(p => p.PilaId == id);

                if (productoExistente != null)
                {
                    // Si el producto está en el carrito, removerlo
                    carrito.Remove(productoExistente);
                }
            }

            return RedirectToAction("Index");
        }
        public ActionResult LimpiarCarrito()
        {
            // Obtener el carrito de la sesión
            List<Pila> carrito = Session["Carrito"] as List<Pila>;

            if (carrito != null)
            {
                // Limpiar el carrito
                carrito.Clear();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> ComprarCarrito(int cantidad, int stock)
        {

                int idUsuario = Convert.ToInt16(Session["UserId"]);
                List<Pila> carrito = Session["Carrito"] as List<Pila>;

                if (carrito != null && carrito.Any())
                {
                    await ng.Comprar(carrito, idUsuario, cantidad);


                    Session["Carrito"] = new List<Pila>();

                    return RedirectToAction("CompraExitosa");
                }
                else
                {
                    return RedirectToAction("CarritoVacio");
                }
            
        }
        public ActionResult Stock()
        {
            return View();
        }
        public ActionResult CompraExitosa()
        {

            if (Usuario_AUTHCompra())
            {
                No_cache(); // Deshabilitar la caché
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        public ActionResult CarritoVacio()
        {

            if (Usuario_AUTHCompra())
            {
                No_cache(); // Deshabilitar la caché
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }
    }
}
