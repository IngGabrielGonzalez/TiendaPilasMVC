using PilasTiendaMVC.Pilas.BLL;
using PilasTiendaMVC.Pilas.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TiendaPilasMVC.Controllers;

namespace PilasTiendaMVC.Controllers
{
    public class CompradorController : CacheController
    {
        Negocios neg = new Negocios();  
        // GET: Comprador
        public ActionResult Index()
        {
            if (Usuario_AUTHCompra())
            {
                return View();

            }
            else
            {
                return View("Login", "Login");
            }
        }

        [HttpGet]
        public async Task<ActionResult> Listar()
        {
            try
            {
                if (Usuario_AUTHCompra())
                {
                    No_cache(); // Lógica para deshabilitar el caché del controlador de caché
                    List<Pila> listaPilas = await neg.ObtenerProductos();
                    return View(listaPilas);
                }
                else
                {
                    return RedirectToAction("Login", "Login");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error al obtener la lista de pilas: " + ex.Message;
                return View();
            }
        }

        public async Task<ActionResult> MostrarCarrito(int id)
        {
            try
            {
                if (Usuario_AUTHCompra())
                {
                    No_cache();
                    Pila producto = await neg.EditarProducto(id);

                    if (producto != null)
                    {
                        return View(producto);
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "El producto no existe.";
                        return View();
                    }
                }
                else
                {
                    return RedirectToAction("Login", "Login");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error al obtener el producto para comprar: " + ex.Message;
                return View();
            }
        }
    }
}