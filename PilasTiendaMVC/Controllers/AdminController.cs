using PilasTiendaMVC.Pilas.BLL;
using PilasTiendaMVC.Pilas.DAL;
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
    public class AdminController : CacheController
    {
        Negocios neg = new Negocios();
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Listar()
        {
            try
            {
                if (Usuario_AUTH())
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

        public ActionResult Alta()
        {
            if (!Usuario_AUTH()) // Verifica si el usuario no está autenticado
            {
                return RedirectToAction("Login", "Login"); // Redirige a la página de inicio de sesión si no está autenticado
            }
            No_cache();

            // Devolver la vista de alta vacía para que el usuario pueda ingresar los datos
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Alta(Pila pila)
        {
            if (!Usuario_AUTH()) // Verifica si el usuario no está autenticado
            {
                return RedirectToAction("Login", "Login"); // Redirige a la página de inicio de sesión si no está autenticado
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Llama al método en la capa DAL para agregar la nueva pila
                    await neg.AgregarPila(pila);
                    return RedirectToAction("Listar");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Ocurrió un error al intentar guardar la pila: " + ex.Message);
                }
            }
            return View(pila);
        }

        [HttpGet]
        public async Task<ActionResult> Detalles()
        {
            try
            {
                if (Usuario_AUTH()) // Verificar autenticación del usuario
                {
                    No_cache(); // Deshabilitar la caché
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

        public async Task<ActionResult> Editar(int id)
        {
            try
            {
                if (Usuario_AUTH())
                {
                    No_cache();

                    // Obtener el producto a editar
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
                ViewBag.ErrorMessage = "Error al obtener el producto para editar: " + ex.Message;
                return View();
            }
        }

        [HttpPost]
        public async Task<ActionResult> Editar(Pila producto)
        {
            try
            {
                if (Usuario_AUTH())
                {
                    No_cache();

                    // Llamar al método de la capa de negocios para editar el producto
                    await neg.EditarProd(producto);

                    ViewBag.SuccessMessage = "Producto editado exitosamente.";

                    // Redirigir al usuario a alguna página después de editar
                    return RedirectToAction("Detalles");
                }
                else
                {
                    return RedirectToAction("Login", "Login");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error al guardar la edición del producto: " + ex.Message;
                return View();
            }
        }





        public async Task<ActionResult> Eliminar(int id)
        {
            try
            {
                if (Usuario_AUTH()) // Verificar autenticación del usuario
                {
                    No_cache(); // Deshabilitar la caché
                    await EliminarProducto(id);
                    return RedirectToAction("Detalles");
                }
                else
                {
                    return RedirectToAction("Login", "Login");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error al eliminar el producto: " + ex.Message;
                return RedirectToAction("Detalles");
            }
        }

        [HttpPost]
        public async Task<ActionResult> EliminarProducto(int id)
        {
            try
            {
                if (Usuario_AUTH()) // Verificar autenticación del usuario
                {
                    Pila pila = await neg.EditarProducto(id);
                    bool eliminacionExitosa = await neg.EliminarProd(pila.PilaId);

                    if (eliminacionExitosa)
                    {
                        return RedirectToAction("Detalles");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Error al eliminar el producto.";
                        return RedirectToAction("Detalles");
                    }
                }
                else
                {
                    return RedirectToAction("Login", "Login");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error al eliminar el producto: " + ex.Message;
                return RedirectToAction("Detalles");
            }
        }
        public async Task<ActionResult> ListaUsuarios()
        {
            try
            {
                if (Usuario_AUTH()) // Verificar autenticación del usuario
                {
                    No_cache(); // Deshabilitar la caché

                    // Obtener todos los usuarios
                    List<Usuarios> todosLosUsuarios = await neg.ObtenerUsuarios();

                    // Filtrar los usuarios con el rol de "Comprador" 
                    List<Usuarios> compradores = todosLosUsuarios.Where(u => u.rol == "Comprador").ToList();

                    // Pasa la lista de compradores a la vista para que se muestren al usuario
                    return View(compradores);
                }
                else
                {
                    return RedirectToAction("Login", "Login");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error al obtener la lista de compradores: " + ex.Message;
                return View();
            }
        }



        public async Task<ActionResult> ObtenerVentas(int usuarioId)
        {
            try
            {
                if (Usuario_AUTH())
                {
                    No_cache();
                    List<Ventas> usuariosVentas = await neg.ObtenerUsuariosVentas(usuarioId);
                    if (usuariosVentas.Count <= 0)
                    {
                        ViewBag.ErrorMessage = "No hay compras hechas por este usuario";
                        // Retornar la vista actual con el mensaje de error
                        return View(usuariosVentas);
                    }
                    return View(usuariosVentas); // Devuelve la vista con los usuarios de ventas
                }
                else
                {
                    return RedirectToAction("Login", "Login");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error al obtener usuarios de ventas: " + ex.Message;
                return View(); // Devuelve la vista de error
            }
        }

        public async Task<ActionResult> ObtenerUsuarios(int idProducto)
        {
            try
            {
                if (Usuario_AUTH())
                {
                    No_cache();
                    List<UsuarioCompra> usrCompras = await neg.ObtenerUsuariosCompras(idProducto);
                    return View(usrCompras);
                }
                else
                {
                    return RedirectToAction("Login", "Login");

                }

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "No hay compras de este producto de ningún usuario" + ex.Message;
                return View(); // Devuelve la vista de error
            }
        }


    }
}







