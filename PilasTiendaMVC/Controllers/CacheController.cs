using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TiendaPilasMVC.Controllers
{
    public class CacheController : Controller
    {
        protected bool Usuario_AUTH()
        {
            return Session["Username"] != null;
        }

        protected bool Usuario_AUTHCompra()
        {
            return Session["UsernameCompra"] != null;

        }

        protected void No_cache()
        {
            // Deshabilitar el almacenamiento en caché de la página
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();
        }
    }
}